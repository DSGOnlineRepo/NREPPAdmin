using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NREPPAdminSite;
using NREPPAdminSite.Models;
using System.Collections.Generic;
using System.IO;

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
        public void SaveFile()
        {
            NrepServ aService = new NrepServ(ServiceTests.ConnString);

            /*string fileName = file.FileName;
            string fileContentType = file.ContentType;
            byte[] fileBytes = new byte[file.ContentLength];
            file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));*/
            string fileName = AppDomain.CurrentDomain.BaseDirectory + "\\HealerCalcs-141111.xls";
            FileStream fs = new FileStream(fileName, FileMode.Open);
            byte[] fileBytes = new byte[fs.Length];
            fs.Read(fileBytes, 0, fileBytes.Length);

            aService.SaveFileToDB(fileBytes, fileName, 1, "NOT IMPLEMENTED!", 1, false, -1, "Healer Calcs");

            Assert.IsTrue(true);
        }

    }
}
