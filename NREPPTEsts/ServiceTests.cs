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
            NrepServ aService = new NrepServ(ServiceTests.ConnString);
            NreppUser someUser = aService.LoginComplete("failusre", "failpass");

            Assert.IsTrue(someUser.Firstname == "Failed");

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

            int fileNum = aService.SaveFileToDB(fileBytes, nFileName, 1, "NOT IMPLEMENTED!", 1, false, -1, "Healer Calcs", 4);

            Assert.IsTrue(fileNum > 0, "No filenumber returned.");

            // Check the file

            byte[] outFile = aService.GetFileFromDB(fileNum);

            Assert.IsTrue(outFile.Length > 0, "Returned SOMETHING");

            // NOW, let's delete the file:

            //int secondNum = aService.SaveFileToDB(fileBytes, nFileName, 1, "NOT IMPLEMENTED!", 1, true, fileNum, "Healer Calcs");

            //Assert.IsTrue(true);
        }

        [TestMethod]
        public void CategoryTest()
        {
            NrepServ aService = new NrepServ(ServiceTests.ConnString);
            List<Answer> SomeAnswers = aService.GetAnswersByCategory("Y/N/NR").ToList<Answer>();

            Assert.AreEqual(SomeAnswers.Count, 3);
        }

    }
}
