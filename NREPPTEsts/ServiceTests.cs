using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NREPPAdminSite;
using NREPPAdminSite.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NREPPTests
{
    [TestClass]
    public class ServiceTests
    {
        public static string ConnString = "server=localhost;database=NREPPAdmin;uid=nrAgent;password=nr!Agent123456";
        [TestMethod]
        public void DummyMethod()
        {
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void TestDB()
        {
            NrepServ aService = new NrepServ(ServiceTests.ConnString);

            List<Intervention> interVs = aService.GetInterventions();

            Assert.IsTrue(interVs.Count > 0);
            
        }

        [TestMethod]
        public void LoginFailure()
        {
            //NrepServ aService = new NrepServ(ServiceTests.ConnString);
            //NreppUser someUser = aService.LoginComplete("failusre", "failpass");

            //Assert.IsTrue(someUser.Firstname == "Failed");

        }

        [TestMethod]
        public void SaveInverventionFile()
        {
            NrepServ aService = new NrepServ(ServiceTests.ConnString);
            string baseFileName = "\\HealerCalcs-141111.xls";
            string nFileName = @"\nFileName" + (new Random()).Next() + ".xls";


            string fileName = AppDomain.CurrentDomain.BaseDirectory + baseFileName;
            FileStream fs = new FileStream(fileName, FileMode.Open);
            byte[] fileBytes = new byte[fs.Length];
            fs.Read(fileBytes, 0, fileBytes.Length);
            fs.Close();



            int fileNum = aService.SaveFileToDB(fileBytes, nFileName, "nrepptest1", "NOT IMPLEMENTED!", 1, false, -1, "Healer Calcs", 4);

            Assert.IsTrue(fileNum > 0, "No filenumber returned.");

            // Check the file

            byte[] outFile = aService.GetFileFromDB(fileNum);

            Assert.IsTrue(outFile.Length > 0, "Returned SOMETHING");
            
        }

        [TestMethod]
        public void CategoryTest()
        {
            NrepServ aService = new NrepServ(ServiceTests.ConnString);
            List<Answer> SomeAnswers = aService.GetAnswersByCategory("Y/N/NR").ToList<Answer>();

            Assert.AreEqual(SomeAnswers.Count, 3);
        }

        [TestMethod]
        public void RCDocEditTest()
        {
            NrepServ aService = new NrepServ(ServiceTests.ConnString);
            List<InterventionDoc> Docs = aService.GetDocuments(1, null, null).ToList<InterventionDoc>();

            Assert.IsTrue(Docs.Count > 0, "No Documents");
            
            int DocId = Docs[0].DocId;

            aService.UpdateRCDocInfo(-1, DocId, "Some Reference", "SomeDoc.jpg", null);

            List<RCDocument> RCDocs = aService.GetRCDocuments(null, 1);

            Assert.IsTrue(RCDocs.Count > 0, "No RC Data");
        }

        /// <summary>
        /// This test is pretty useless, since it passes even if the connection fails. :|
        /// </summary>
        [TestMethod]
        public void NoPermTest()
        {
            NrepServ aService = new NrepServ(ServiceTests.ConnString);

            Assert.IsFalse(aService.CanDo("TestPermission", "rev1", 2));
        }

        [TestMethod]
        public void HasGeneralPermTest()
        {
            NrepServ aService = new NrepServ(ServiceTests.ConnString);

            Assert.IsFalse(aService.CanDo("TestPermission", "rev1", null));
        }

        [TestMethod]
        public void HasSpecificPermTest()
        {
            NrepServ aService = new NrepServ(ServiceTests.ConnString);

            Assert.IsFalse(aService.CanDo("TestPermission", "rev1", 1));
        }

        [TestMethod]
        public void SymEncrypt()
        {
            //string SomeString = PasswordHash.GenKey();
            string initString = "someText";
            string SomeString = PasswordHash.AESCrypt(initString);
            string outString = PasswordHash.AESDecrypt(SomeString);

            Assert.IsTrue(initString.Equals(outString));
        }

    }
}
