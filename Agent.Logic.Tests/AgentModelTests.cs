using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AlfaBank.AlfaRobot.ControlCenter.Agent.Logic;
using AlfaBank.AlfaRobot.ControlCenter.Common;
using AlfaBank.AlfaRobot.ControlCenter.Configuration;
using System.Threading;
using System.IO;

namespace AlfaBank.AlfaRobot.ControlCenter.Agent.Logic.Tests
{
    [TestClass]
    public class AgentModelTests
    {
        /// <summary>
        /// Экземпляр дескриптора сайта.
        /// </summary>
        private ISite _siteInstance;

        /// <summary>
        /// Ожидание прихода события в миллисекундах.
        /// </summary>
        private const int WAIT_EVENT_MS = 20;

        /// <summary>
        /// Инициаизация теста.
        /// </summary>
        [TestInitialize]
        public void InitTest()
        {
            if (File.Exists(AgentModel.CONFIG_FILE_NAME))
            {
                File.Delete(AgentModel.CONFIG_FILE_NAME);
            }
        }

        /// <summary>
        /// Добавляем сайт в конфигурацию.
        /// </summary>
        [TestMethod]
        public void AgentModelAddNewSiteToConfigTest()
        {
            IAgentModel model = new AgentModel();

            bool result = model.AddSiteToConfig(
                new SiteConfiguration()
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
        public void AgentModelAddDuplicateSiteToConfigTest()
        {
            IAgentModel model = new AgentModel();

            bool result1 = model.AddSiteToConfig(
                new SiteConfiguration()
                {
                    SiteName = "testSite",
                    ExecutableFilePath = "testPath",
                    StartArguments = new List<string>() { "arg0", "arg1", "arg2" }
                });

            bool result2 = model.AddSiteToConfig(
                new SiteConfiguration()
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
        public void AgentModelRemoveSiteFromConfigTest()
        {
            IAgentModel model = new AgentModel();
            string siteName = "testSite";

            model.AddSiteToConfig(
                new SiteConfiguration()
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
        public void AgentModelRemoveUnexistingSiteFromConfigTest()
        {
            IAgentModel model = new AgentModel();

            model.AddSiteToConfig(
                new SiteConfiguration()
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
            IAgentModel model = new AgentModel();
            _siteInstance = null;

            model.SiteAdded += AddSiteHandler;

            model.AddSiteToConfig(
                new SiteConfiguration()
                {
                    SiteName = "testSite",
                    ExecutableFilePath = "testPath",
                    StartArguments = new List<string>() { "arg0", "arg1", "arg2" }
                });

            Thread.Sleep(WAIT_EVENT_MS);

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
            IAgentModel model = new AgentModel();

            model.SiteAdded += AddSiteHandler;

            model.AddSiteToConfig(
                new SiteConfiguration()
                {
                    SiteName = "testSite",
                    ExecutableFilePath = "testPath",
                    StartArguments = new List<string>() { "arg0", "arg1", "arg2" }
                });

            _siteInstance = null;

            model.AddSiteToConfig(
                new SiteConfiguration()
                {
                    SiteName = "testSite",
                    ExecutableFilePath = "testPath",
                    StartArguments = new List<string>() { "arg0", "arg1", "arg2" }
                });

            Thread.Sleep(WAIT_EVENT_MS);

            Assert.IsNull(_siteInstance);
        }

        /// <summary>
        /// Проверяем событие удаления сайта.
        /// </summary>
        [TestMethod]
        public void AgentModelRemoveSiteToConfigEventTest()
        {
            IAgentModel model = new AgentModel();
            string siteName = "testSite";
            _siteInstance = null;

            model.SiteRemoved += RemoveSiteHandler;

            model.AddSiteToConfig(
                new SiteConfiguration()
                {
                    SiteName = siteName,
                    ExecutableFilePath = "testPath",
                    StartArguments = new List<string>() { "arg0", "arg1", "arg2" }
                });

            model.RemoveSiteFromConfig(siteName);

            Thread.Sleep(WAIT_EVENT_MS);

            Assert.IsNotNull(_siteInstance);
            Assert.AreEqual("testSite", _siteInstance.SiteName);
            Assert.AreEqual("testPath", _siteInstance.FilePath);
            Assert.AreEqual(SiteStatus.FILE_NOT_EXISTS, _siteInstance.Status);
        }

        /// <summary>
        /// Проверяем обновления сайта в конфигурации.
        /// </summary>
        [TestMethod]
        public void AgentModelUpdateSiteToConfigTest()
        {
            IAgentModel model = new AgentModel();

            model.AddSiteToConfig(
                new SiteConfiguration()
                {
                    SiteName = "testSite",
                    ExecutableFilePath = "testPath",
                    StartArguments = new List<string>() { "arg0", "arg1", "arg2" }
                });

            bool result = model.UpdateSiteConfig(
                new SiteConfiguration()
                {
                    SiteName = "testSite",
                    ExecutableFilePath = "pathTest",
                    StartArguments = new List<string>() { "arg5", "arg6", "arg7", "arg8" }
                });

            Assert.IsTrue(result);
            Assert.AreEqual(1, model.Sites.Count);
            Assert.AreEqual(1, model.Configuration.Sites.Count);

            Assert.AreEqual("testSite", model.Configuration.Sites[0].SiteName);
            Assert.AreEqual("pathTest", model.Configuration.Sites[0].ExecutableFilePath);
            Assert.AreEqual("arg5", model.Configuration.Sites[0].StartArguments[0]);
            Assert.AreEqual("arg6", model.Configuration.Sites[0].StartArguments[1]);
            Assert.AreEqual("arg7", model.Configuration.Sites[0].StartArguments[2]);
            Assert.AreEqual("arg8", model.Configuration.Sites[0].StartArguments[3]);

            Assert.AreEqual("testSite", model.Sites[0].SiteName);
            Assert.AreEqual("pathTest", model.Sites[0].FilePath);
            Assert.AreEqual(SiteStatus.FILE_NOT_EXISTS, model.Sites[0].Status);
        }

        /// <summary>
        /// Проверяем обновление несуществующего сайта в конфигурации.
        /// </summary>
        [TestMethod]
        public void AgentModelUpdateUnexistingSiteToConfigAndEventTest()
        {
            IAgentModel model = new AgentModel();

            model.SiteUpdated += UpdateSiteHandler;

            model.AddSiteToConfig(
                new SiteConfiguration()
                {
                    SiteName = "testSite",
                    ExecutableFilePath = "testPath",
                    StartArguments = new List<string>() { "arg0", "arg1", "arg2" }
                });

            _siteInstance = null;

            bool result = model.UpdateSiteConfig(
                new SiteConfiguration()
                {
                    SiteName = "testSite2",
                    ExecutableFilePath = "pathTest",
                    StartArguments = new List<string>() { "arg5", "arg6", "arg7", "arg8" }
                });

            Thread.Sleep(WAIT_EVENT_MS);

            Assert.IsFalse(result);
            Assert.IsNull(_siteInstance);
        }

        /// <summary>
        /// Проверяем событие обновления сайта.
        /// </summary>
        [TestMethod]
        public void AgentModelUpdateSiteToConfigEventTest()
        {
            IAgentModel model = new AgentModel();

            model.SiteUpdated += UpdateSiteHandler;

            model.AddSiteToConfig(
                new SiteConfiguration()
                {
                    SiteName = "testSite",
                    ExecutableFilePath = "testPath",
                    StartArguments = new List<string>() { "arg0", "arg1", "arg2" }
                });

            _siteInstance = null;

            model.UpdateSiteConfig(
                new SiteConfiguration()
                {
                    SiteName = "testSite",
                    ExecutableFilePath = "pathTest",
                    StartArguments = new List<string>() { "arg5", "arg6", "arg7", "arg8" }
                });

            Thread.Sleep(WAIT_EVENT_MS);

            Assert.IsNotNull(_siteInstance);
            Assert.AreEqual("testSite", _siteInstance.SiteName);
            Assert.AreEqual("pathTest", _siteInstance.FilePath);
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
            _siteInstance = eventArgs.Site;
        }
    }
}
