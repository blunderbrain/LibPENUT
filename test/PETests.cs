using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace LibPENUT.Test
{
    [TestClass]
    public class PETests
    {
        public TestContext TestContext { get; set; }

        public string TestsRootFolder { get; set; }

        [TestInitialize]
        public void TestRunInitialize()
        {
#if NET35
            TestsRootFolder = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\"));
#else
            TestsRootFolder = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\..\"));
#endif
        }

        /// <summary>
        /// Test that we can read and write back a basic exe file without loss/changes
        /// </summary>
        [TestMethod]
        public void TestReadWriteExe()
        {
            foreach(string testFile in new string[]
            {
                @"TestData\CL_v19\TestData2_Release_Win32.exe",
                @"TestData\CL_v19\TestData2_Release_x64.exe",
            })
            {
                string testInputDataFile = Path.Combine(TestsRootFolder, testFile);
                string testOutputDataFile = Path.Combine(TestContext.TestRunResultsDirectory, testFile);

                Directory.CreateDirectory(Path.GetDirectoryName(testOutputDataFile));

                PEImage exe = new PEImage(testInputDataFile);

                exe.Write(testOutputDataFile);

                byte[] originalExe = File.ReadAllBytes(testInputDataFile);
                byte[] newExe = File.ReadAllBytes(testOutputDataFile);

                PEImage newExeImage = new PEImage(testOutputDataFile);

                Assert.IsTrue(originalExe.SequenceEqual(newExe), string.Format("Unmodified saved .exe file '{0}' does not match original", testOutputDataFile));
            }
        }

#if NET6_0_OR_GREATER
        /// <summary>
        /// Lazy stresstest, try to parse every .exe/dll in the current program files folders
        /// </summary>
#if false
        [TestMethod]
#endif
        public void TestReadWriteRandomImages()
        {
            EnumerationOptions enumOptions = new EnumerationOptions();
            enumOptions.IgnoreInaccessible = true;
            enumOptions.RecurseSubdirectories = true;
            IEnumerable<string> programFilesExe = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "*.exe", enumOptions);
            IEnumerable<string> programFilesx86Exe = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "*.exe", enumOptions);

            IEnumerable<string> programFilesDll = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "*.dll", enumOptions);
            IEnumerable<string> programFilesx86Dll = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "*.dll", enumOptions);

            // Any files to exclude
            string[] excludes = new string[] {
            };

            IEnumerable<string> testFilesExe = programFilesExe.Concat(programFilesx86Exe).Except(excludes);
            IEnumerable<string> testFilesDll = programFilesDll.Concat(programFilesx86Dll);

            IEnumerable<string> testFiles = testFilesExe.Concat(testFilesDll);

            byte[] originalFile = new byte[0];
            byte[] newFile = new byte[0];
            List<string> failures = new List<string>();

            foreach (string testFile in testFiles)
            {
                // Skip empty files
                FileInfo testFileInfo = new FileInfo(testFile);
                if (testFileInfo.Length == 0)
                    continue; ;

                TestContext.WriteLine("Processing {0}", testFile);

                try
                {
                    originalFile = File.ReadAllBytes(testFile);
                    newFile = new byte[originalFile.Length];

                    PEImage image = new PEImage();
                    image.Read(new MemoryStream(originalFile));

                    image.Write(new MemoryStream(newFile, true));

                    if (!originalFile.SequenceEqual(newFile))
                    {
                        string outputDir = Path.Combine(TestContext.TestRunResultsDirectory, Path.GetFileName(Path.GetDirectoryName(testFile)));
                        Directory.CreateDirectory(outputDir);
                        File.WriteAllBytes(Path.Combine(outputDir, Path.GetFileName(testFile)), newFile);
                        string failure = string.Format("Unmodified saved file '{0}' does not match original, see outputfile in {1} for analysis", testFile, TestContext.TestRunResultsDirectory);
                        TestContext.WriteLine(failure);
                        failures.Add(failure);
                    }
                }
                catch(Exception ex)
                {
                    string failure = string.Format("{0} occured while processing '{1}' - {2}\n{3}", ex.GetType().Name, testFile, ex.Message, ex.StackTrace);
                    TestContext.WriteLine(failure);
                    failures.Add(failure);
                }
            }

            TestContext.WriteLine("***************************** Results *****************************");

            foreach (string failure in failures)
             TestContext.WriteLine(failure);

            Assert.IsTrue(failures.Count == 0, string.Format("{0} errors occured during test", failures.Count));
        }
#endif

        /// <summary>
        /// Test that we can re-calculate the same checksum as the existing one kernel32.dll
        /// </summary>
        [TestMethod]
        public void TestCheckSumCalculation()
        {
            string kernel32Path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "kernel32.dll");

            PEImage kernel32 = new PEImage(kernel32Path);
            UInt32 originalCheckSum = kernel32.PEOptionalHeader.CheckSum;

            UInt32 newCheckSum = kernel32.CalculateCheckSum();

            Assert.IsTrue(originalCheckSum == newCheckSum, "Checksum mismatch. Calculated value for Kernel32.dll does not match the image checksum");
        }
        #region CheckSumMappedFile PINVOKE declarations
        [DllImport("kernel32.dll", EntryPoint = "CreateFileMappingA", CharSet = CharSet.Ansi, SetLastError = true)]
        static extern IntPtr CreateFileMapping(
            Microsoft.Win32.SafeHandles.SafeFileHandle hFile,
            IntPtr lpFileMappingAttributes,
            UInt32 flProtect,
            UInt32 dwMaximumSizeHigh,
            UInt32 dwMaximumSizeLow,
            string lpName
            );

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr MapViewOfFile(
              IntPtr hFileMappingObject,
              UInt32 dwDesiredAccess,
              UInt32 dwFileOffsetHigh,
              UInt32 dwFileOffsetLow,
              UIntPtr dwNumberOfBytesToMap
            );
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr UnmapViewOfFile(
            IntPtr lpBaseAddress
            );
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool CloseHandle(
            IntPtr hObject
            );

        [DllImport("Imagehlp.dll", SetLastError = true)]
        static extern IntPtr CheckSumMappedFile(
            IntPtr baseAddress,
            UInt32 fileLength,
            ref UInt32 originalCheckSum,
            ref UInt32 checkSum);

        #endregion

        /// <summary>
        /// Test that we can properly calculate the checksum for odd sized files that do not contain an even number of DWORDs.
        /// This also tests that we can achive the same result as the CheckSumMappedFile() API
        /// </summary>
        [TestMethod]
        public void TestCheckSumCalculation_OddFileSize()
        {
            string testinputFile = Path.Combine(TestsRootFolder, @"TestData\\CL_v19\\TestData2_Release_x64_OddSize.exe");
            File.Copy(Path.Combine(TestsRootFolder, @"TestData\\CL_v19\\TestData2_Release_x64.exe"), testinputFile, true);
            File.AppendAllText(testinputFile, "x", System.Text.Encoding.ASCII);
            
            PEImage testImage = new PEImage(testinputFile);
            UInt32 libPENUTCheckSum = testImage.CalculateCheckSum();
            UInt32 imagehlpCheckSum = 0;
            UInt32 imagehlpOrigCheckSum = 0;

            using (var inputFileStream = File.OpenRead(testinputFile))
            {
                // Create a read only native file mapping for the input file
                UInt32 PAGE_READONLY = 0x02;
                UInt32 FILE_MAP_READ = 0x04;
                IntPtr hFileMap = CreateFileMapping(inputFileStream.SafeFileHandle, IntPtr.Zero, PAGE_READONLY, 0, 0, string.Empty);
                IntPtr fileBase = MapViewOfFile(hFileMap, FILE_MAP_READ, 0, 0, UIntPtr.Zero);

                // Use the official ImageHlp API to calculate checksum from the file mapping
                IntPtr headerPtr = CheckSumMappedFile(fileBase, (UInt32)inputFileStream.Length, ref imagehlpOrigCheckSum, ref imagehlpCheckSum);

                // Cleanup
                if (fileBase != IntPtr.Zero)
                    UnmapViewOfFile(fileBase);

                if (hFileMap != IntPtr.Zero)
                    CloseHandle(hFileMap);
            }

            Assert.IsTrue(libPENUTCheckSum == imagehlpCheckSum, "Checksum mismatch - result from CheckSumMappedFile() does not match calculated value");
        }
    }
}
