using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AlfaBank.AlfaRobot.ControlCenter.Agent.Logic;
using AlfaBank.AlfaRobot.ControlCenter.Common;
using System.Threading;

namespace Agent.Logic.Tests
{
    [TestClass]
    public class AgentModelTests
    {
        /// <summary>
        /// Экземпляр дескриптора сайта.
        /// </summary>
        private ISite _siteInstance;

        /// <summary>
        /// Добавляем сайт в конфигурацию.
        /// </summary>
        [TestMethod]
        public void AgentModelAddNewSiteToConfig()
        {
            AgentModel model = new AgentModel();

            bool result = model.AddSiteToConfig(
                new SiteDescriptor()
                {
                    SiteName = "testSite",
                    ExecutableFilePath = "testPath",
                    StartArguments = new List<string>() { "arg0", "arg1", "arg2" }
                });

            Assert.IsTrue(result);

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

        /// <summary>
        /// Удаляем сайт из конфигурации.
        /// </summary>
        [TestMethod]
        public void AgentModelRemoveSiteFromConfig()
        {
            AgentModel model = new AgentModel();
            string siteName = "testSite";

            model.AddSiteToConfig(
                new SiteDescriptor()
                {
                    SiteName = siteName,
                    ExecutableFilePath = "testPath",
                    StartArguments = new List<string>() { "arg0", "arg1", "arg2" }
                });

            bool result = model.RemoveSiteFromConfig(siteName);

            Assert.IsTrue(result);
            Assert.AreEqual(0, model.Sites.Count);
            Assert.AreEqual(0, model.Configuration.Sites.Count);
        }

        /// <summary>
        ///  Удаляем несуществующий сайт из конфигурации.
        /// </summary>
        [TestMethod]
        public void AgentModelRemoveUnexistingSiteFromConfig()
        {
            AgentModel model = new AgentModel();

            model.AddSiteToConfig(
                new SiteDescriptor()
                {
                    SiteName = "testSite",
                    ExecutableFilePath = "testPath",
                    StartArguments = new List<string>() { "arg0", "arg1", "arg2" }
                });

            bool result = model.RemoveSiteFromConfig("siteTest");

            Assert.IsFalse(result);
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

        /// <summary>
        /// Проверяем событие добавления сайта.
        /// </summary>
        [TestMethod]
        public void AgentModelAddNewSiteToConfigEventTest()
        {
            AgentModel model = new AgentModel();
            _siteInstance = null;

            model.SiteAdded += AddSiteHandler;

            model.AddSiteToConfig(
                new SiteDescriptor()
                {
                    SiteName = "testSite",
                    ExecutableFilePath = "testPath",
                    StartArguments = new List<string>() { "arg0", "arg1", "arg2" }
                });

            Thread.Sleep(50);

            Assert.IsNotNull(_siteInstance);
            Assert.AreEqual("testSite", _siteInstance.SiteName);
            Assert.AreEqual("testPath", _siteInstance.FilePath);
            Assert.AreEqual(SiteStatus.FILE_NOT_EXISTS, _siteInstance.Status);
        }

        /// <summary>
        /// Проверяем событие добавления сайта.
        /// </summary>
        [TestMethod]
        public void AgentModelAddDuplicateSiteToConfigEventTest()
        {
            AgentModel model = new AgentModel();

            model.SiteAdded += AddSiteHandler;

            model.AddSiteToConfig(
                new SiteDescriptor()
                {
                    SiteName = "testSite",
                    ExecutableFilePath = "testPath",
                    StartArguments = new List<string>() { "arg0", "arg1", "arg2" }
                });

            _siteInstance = null;

            model.AddSiteToConfig(
                new SiteDescriptor()
                {
                    SiteName = "testSite",
                    ExecutableFilePath = "testPath",
                    StartArguments = new List<string>() { "arg0", "arg1", "arg2" }
                });

            Thread.Sleep(50);

            Assert.IsNull(_siteInstance);
        }

        /// <summary>
        /// Проверяем событие удаления сайта.
        /// </summary>
        [TestMethod]
        public void AgentModelRemoveSiteToConfigEventTest()
        {
            AgentModel model = new AgentModel();
            string siteName = "testSite";
            _siteInstance = null;

            model.SiteRemoved += RemoveSiteHandler;

            model.AddSiteToConfig(
                new SiteDescriptor()
                {
                    SiteName = siteName,
                    ExecutableFilePath = "testPath",
                    StartArguments = new List<string>() { "arg0", "arg1", "arg2" }
                });

            model.RemoveSiteFromConfig(siteName);

            Thread.Sleep(50);

            Assert.IsNotNull(_siteInstance);
            Assert.AreEqual("testSite", _siteInstance.SiteName);
            Assert.AreEqual("testPath", _siteInstance.FilePath);
            Assert.AreEqual(SiteStatus.FILE_NOT_EXISTS, _siteInstance.Status);
        }

        /// <summary>
        /// Проверяем событие обновления сайта.
        /// </summary>
        [TestMethod]
        public void AgentModelUpdateSiteToConfigEventTest()
        {
            AgentModel model = new AgentModel();
            _siteInstance = null;

            model.SiteAdded += UpdateSiteHandler;

            model.AddSiteToConfig(
                new SiteDescriptor()
                {
                    SiteName = "testSite",
                    ExecutableFilePath = "testPath",
                    StartArguments = new List<string>() { "arg0", "arg1", "arg2" }
                });

            Thread.Sleep(50);

            Assert.IsNotNull(_siteInstance);
            Assert.AreEqual("testSite", _siteInstance.SiteName);
            Assert.AreEqual("testPath", _siteInstance.FilePath);
            Assert.AreEqual(SiteStatus.FILE_NOT_EXISTS, _siteInstance.Status);
        }

        /// <summary>
        /// Обработчик события добавления сайта.
        /// </summary>
        private void AddSiteHandler(object sender, SiteEventArgs eventArgs)
        {
            _siteInstance = eventArgs.Site;
        }

        /// <summary>
        /// Обработчик события удаления сайта.
        /// </summary>
        private void RemoveSiteHandler(object sender, SiteEventArgs eventArgs)
        {
            _siteInstance = eventArgs.Site;
        }

        /// <summary>
        /// Обработчик события обновления сайта.
        /// </summary>
        private void UpdateSiteHandler(object sender, SiteEventArgs eventArgs)
        {

        }
    }
}
