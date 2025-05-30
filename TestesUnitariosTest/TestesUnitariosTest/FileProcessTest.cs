using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;
using System.IO;
using TestesUnitarios;

namespace TestesUnitariosTest
{
    [TestClass]
    public class FileProcessTest
    {
        #region Variables

        private const string BAD_FILE_NAME = @"C:\\Users\\patrick.oliveira\\TesteUnitarioArquivoFALSE.txt";
        private string _GoodFileName;
        public TestContext TestContext { get; set; }

        #endregion

        #region Test Initialize e Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            if(TestContext.TestName == "FileNameDoesExists")
            {
                if (string.IsNullOrEmpty(_GoodFileName))
                {
                    SetGoodFileName();
                    TestContext.WriteLine($"Creating File: {_GoodFileName}");
                    File.AppendAllText(_GoodFileName, "Some Text");
                    TestContext.WriteLine($"Testing File: {_GoodFileName}");
                }
            }
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (TestContext.TestName == "FileNameDoesExist")
            {
                if (!string.IsNullOrEmpty(_GoodFileName))
                {
                    TestContext.WriteLine($"Deleting File: {_GoodFileName}");
                    File.Delete(_GoodFileName);
                }
            }
        }

        #endregion

        #region Methods

        [TestMethod]
        [Description("Check to see if a file does exists.")]
        [Owner("Patrick O")]
        [Priority(0)]
        [TestCategory("NoException")]
        public void FileNameDoesExists()
        {

            FileProcess fp = new FileProcess();
            bool fromCall;
            
            fromCall = fp.FileExists(_GoodFileName);
            
            Assert.IsTrue(fromCall);
        }

        [TestMethod]
        [Description("Check to see if a file does not exists.")]
        [Owner("Patrick O")]
        [Priority(1)]
        [TestCategory("NoException")]
        public void FileNameDoesNotExists()
        {
            FileProcess fp = new FileProcess();
            bool fromCall;

            fromCall = fp.FileExists(BAD_FILE_NAME);

            Assert.IsFalse(fromCall);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        [Description("Verify that the method throws ArgumentNullException when the file name is null or empty.")]
        [Owner("Patrick O")]        
        [Priority(1)]
        [TestCategory("Exception")]
        public void FileNameNullOrEmpty_TrowsArgumentNullException()
        {
            FileProcess fp = new FileProcess();            

            fp.FileExists("");
        }

        [TestMethod]
        [Description("Manually verifies that the method throws ArgumentNullException when the file name is null or empty, using try/catch.")]
        [Owner("Patrick O")]
        [Priority(1)]
        [TestCategory("Exception")]
        public void FileNameNullOrEmpty_TrowsArgumentNullException_UsingTryCatch()
        {
            FileProcess fp = new FileProcess();

            try
            {
                fp.FileExists(null);
            }
            catch (ArgumentException)
            {
                //The test was a sucess;
                return;
            }

            fp.FileExists("");
        }

        private const string FILE_NAME = @"FileToDeploy.txt";

        [TestMethod]
        [Owner("Patrick O")]
        public void FileNameDoesExistsUsingDeploymentItem()
        {
            FileProcess fp = new FileProcess();
            string fileName;
            bool fromCall;

            fileName = $@"TestContext.DeploymentDirectory\{FILE_NAME}";

            fromCall = fp.FileExists(BAD_FILE_NAME);

            Assert.IsFalse(fromCall);
        }

        [TestMethod]
        [Timeout(2000)]
        public void SimulateTime()
        {
            System.Threading.Thread.Sleep(3000);
        }

        #endregion

        #region Auxiliar Methods

        public void SetGoodFileName()
        {
            _GoodFileName = ConfigurationManager.AppSettings["GoodFileName"];
            if (_GoodFileName.Contains("[AppPath]"))
            {
                _GoodFileName = _GoodFileName.Replace("[AppPath]",
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
            }
        }  

        #endregion
    }
}