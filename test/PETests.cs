using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using LibPENUT;


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
        /// Lazy stresstest, try to parse every .exe in the current program files folders
        /// </summary>
        [TestMethod]
        public void TestReadWriteRandomExe()
        {
            EnumerationOptions enumOptions = new EnumerationOptions();
            enumOptions.IgnoreInaccessible = true;
            enumOptions.RecurseSubdirectories = true;
            IEnumerable<string> programFilesExe = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "*.exe", enumOptions);
            IEnumerable<string> programFilesx86Exe = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "*.exe", enumOptions);

            foreach (string testFile in programFilesExe.Concat(programFilesx86Exe))
            {
                // Skip empty files
                FileInfo testFileInfo = new FileInfo(testFile);
                if (testFileInfo.Length == 0)
                    continue; ;

                PEImage exe = new PEImage(testFile);

                byte[] originalExe = File.ReadAllBytes(testFile);

                byte[] newExe = new byte[originalExe.Length];

                exe.Write(new MemoryStream(newExe, true));

                try
                {
                    Assert.IsTrue(originalExe.SequenceEqual(newExe), string.Format("Unmodified saved .exe file '{0}' does not match original, see {1}", testFile, TestContext.TestRunResultsDirectory));
                }
                catch(Exception)
                {
                    // For analysis
                    File.WriteAllBytes(Path.Combine(TestContext.TestRunResultsDirectory, Path.GetFileName(testFile)), newExe);
                    throw;
                }
            }
        }
#endif
    }
}
