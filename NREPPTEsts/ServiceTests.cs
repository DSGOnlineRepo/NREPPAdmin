using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NREPPAdminSite;
using NREPPAdminSite.Models;
using System.Collections.Generic;

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

    }
}
