using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;

namespace AlfaBank.AlfaRobot.ControlCenter.Configuration.Tests
{
    [TestClass]
    public class AgentConfigurationTests
    {
        /// <summary>
        /// Корректная конфигурация для тестов.
        /// </summary>
        AgentConfiguration correctConfig;

        /// <summary>
        /// Имя файла для хранения корректной конфигурации.
        /// </summary>
        string correctConfigFileName;

        /// <summary>
        /// Инициализация объектов для тестов.
        /// </summary>
        [TestInitialize]
        public void Init()
        {
            correctConfigFileName = Path.GetRandomFileName();

            correctConfig = new AgentConfiguration()
            {
                WcfServiceUri = @"http://localhost/incoming_agent_service",
                WcfControlServiceUri = @"http://127.0.0.5/control_hub_service",
                ControlServerAccessString = "access_alowed",
                ControlServerAddress = "127.0.0.7"
            };

            correctConfig.AddNewSite(
                new SiteConfiguration()
                {
                    SiteName = "firstSite",
                    ExecutableFilePath = "firstSite.exe",
                    StartArguments = new List<string>() { "arg1", "arg2" }
                });

            correctConfig.AddNewSite(
                new SiteConfiguration()
                {
                    SiteName = "secondSite",
                    ExecutableFilePath = "secondSite.exe",
                    StartArguments = new List<string>() { "arg5", "arg6" }
                });

            correctConfig.AddNewSite(
                new SiteConfiguration()
                {
                    SiteName = "thirdSite",
                    ExecutableFilePath = "thirdSite.exe",
                    StartArguments = new List<string>() { "arg8", "arg9" }
                });

            AgentConfiguration.WriteConfiguration(correctConfig, correctConfigFileName);
        }

        /// <summary>
        /// Очистка данных после тестов.
        /// </summary>
        [TestCleanup]
        public void Clean()
        {
            if (File.Exists(correctConfigFileName))
            {
                try
                {
                    File.Delete(correctConfigFileName);
                }
                catch (Exception e) 
                {
                    throw e;
                }
            }
        }

        /// <summary>
        /// Тестирование записи-чтения корректной конфигурации.
        /// </summary>
        [TestMethod]
        public void AgentConfigurationWriteAndReadConfiguration()
        {
            AgentConfiguration loadedConfig = AgentConfiguration.ReadConfiguration(correctConfigFileName);

            Assert.AreEqual(@"http://localhost/incoming_agent_service", loadedConfig.WcfServiceUri);
            Assert.AreEqual(@"http://127.0.0.5/control_hub_service", loadedConfig.WcfControlServiceUri);
            Assert.AreEqual("access_alowed", loadedConfig.ControlServerAccessString);
            Assert.AreEqual("127.0.0.7", loadedConfig.ControlServerAddress);

            Assert.AreEqual(3, loadedConfig.Sites.Count);

            Assert.AreEqual("arg1", loadedConfig.Sites[0].StartArguments[0]);
            Assert.AreEqual("arg2", loadedConfig.Sites[0].StartArguments[1]);
            Assert.AreEqual("arg5", loadedConfig.Sites[1].StartArguments[0]);
            Assert.AreEqual("arg6", loadedConfig.Sites[1].StartArguments[1]);
            Assert.AreEqual("arg8", loadedConfig.Sites[2].StartArguments[0]);
            Assert.AreEqual("arg9", loadedConfig.Sites[2].StartArguments[1]);
        }
    }
}
