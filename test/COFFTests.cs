using System;
using System.IO;
using System.Linq;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using LibPENUT;


namespace LibPENUT.Test
{
    [TestClass]
    public class COFFTests
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
        /// Test that we can read and write back a basic object file without loss/changes
        /// </summary>
        [TestMethod]
        public void TestReadWriteObjects()
        {
            foreach(string testFile in new string[]
            {
                @"TestData\CL_v19\TestData1_Debug_Win32.obj",
                @"TestData\CL_v19\TestData1_Debug_x64.obj",
            })
            {
                string testInputDataFile = Path.Combine(TestsRootFolder, testFile);
                string testOutputDataFile = Path.Combine(TestContext.TestRunResultsDirectory, testFile);

                Directory.CreateDirectory(Path.GetDirectoryName(testOutputDataFile));

                COFFObject obj = new COFFObject();
                obj.Read(testInputDataFile);

                obj.Write(testOutputDataFile);

                byte[] originalObj = File.ReadAllBytes(testInputDataFile);
                byte[] newObj = File.ReadAllBytes(testOutputDataFile);

                Assert.IsTrue(originalObj.SequenceEqual(newObj), string.Format("Unmodified saved object file '{0}' does not match original", testOutputDataFile));
            }
        }
    }
}
