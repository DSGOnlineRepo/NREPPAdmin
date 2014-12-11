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
        [TestMethod]
        public void DummyMethod()
        {
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void TestDB()
        {
            NrepServ aService = new NrepServ("server=localhost;database=NREPPAdmin;uid=nrAgent;password=nr!Agent123456");
            List<Intervention> interVs = aService.GetInterventions();

            Assert.IsTrue(interVs.Count > 0);
            
        }
    }
}
