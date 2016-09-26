using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AlfaBank.AlfaRobot.ControlCenter.Agent.Logic;
using AlfaBank.AlfaRobot.ControlCenter.Common;

namespace Agent.Logic.Tests
{
    [TestClass]
    public class AgentModelTests
    {
        /// <summary>
        /// Добавляем сайт в конфигурацию.
        /// </summary>
        [TestMethod]
        public void AgentModelAddNewSiteToConfig()
        {
            AgentModel model = new AgentModel();

            model.AddSiteToConfig(
                new SiteDescriptor()
                {
                    SiteName = "testSite",
                    ExecutableFilePath = "testPath",
                    StartArguments = new List<string>() { "arg0", "arg1", "arg2" }
                });

            Assert.AreEqual("testSite", model.Configuration.Sites[0].SiteName);
            Assert.AreEqual("testPath", model.Configuration.Sites[0].ExecutableFilePath);
            Assert.AreEqual("arg0", model.Configuration.Sites[0].StartArguments[0]);
            Assert.AreEqual("arg1", model.Configuration.Sites[0].StartArguments[1]);
            Assert.AreEqual("arg2", model.Configuration.Sites[0].StartArguments[2]);

            Assert.AreEqual("testSite", model.Sites[0].SiteName);
            Assert.AreEqual("testPath", model.Sites[0].FilePath);
            Assert.AreEqual(SiteStatus.FILE_NOT_EXISTS, model.Sites[0].Status);
        }

        /// <summary>
        /// Добавляем сайт с уже существующим именем в конфигурацию.
        /// </summary>
        [TestMethod]
        public void AgentModelAddDuplicateSiteToConfig()
        {
            AgentModel model = new AgentModel();

            bool result1 = model.AddSiteToConfig(
                new SiteDescriptor()
                {
                    SiteName = "testSite",
                    ExecutableFilePath = "testPath",
                    StartArguments = new List<string>() { "arg0", "arg1", "arg2" }
                });

            bool result2 = model.AddSiteToConfig(
                new SiteDescriptor()
                {
                    SiteName = "testSite",
                    ExecutableFilePath = "nullPath",
                    StartArguments = new List<string>() { "argN" }
                });

            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
            Assert.AreEqual(1, model.Sites.Count);
            Assert.AreEqual(1, model.Configuration.Sites.Count);

            Assert.AreEqual("testSite", model.Configuration.Sites[0].SiteName);
            Assert.AreEqual("testPath", model.Configuration.Sites[0].ExecutableFilePath);
            Assert.AreEqual("arg0", model.Configuration.Sites[0].StartArguments[0]);
            Assert.AreEqual("arg1", model.Configuration.Sites[0].StartArguments[1]);
            Assert.AreEqual("arg2", model.Configuration.Sites[0].StartArguments[2]);

            Assert.AreEqual("testSite", model.Sites[0].SiteName);
            Assert.AreEqual("testPath", model.Sites[0].FilePath);
            Assert.AreEqual(SiteStatus.FILE_NOT_EXISTS, model.Sites[0].Status);
        }
    }
}
