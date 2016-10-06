using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlfaBank.AlfaRobot.ControlCenter.Configuration;
using AlfaBank.AlfaRobot.ControlCenter.Common;

namespace AlfaBank.AlfaRobot.ControlCenter.Agent.Logic
{
    public class AgentModel : IAgentModel
    {
        /// <summary>
        /// Событие обновления сайта.
        /// </summary>
        public event EventHandler<SiteEventArgs> SiteUpdated = delegate { };

        /// <summary>
        /// Событие добавления сайта.
        /// </summary>
        public event EventHandler<SiteEventArgs> SiteAdded = delegate { };

        /// <summary>
        /// Событие удаления сайта.
        /// </summary>
        public event EventHandler<SiteEventArgs> SiteRemoved = delegate { };

        /// <summary>
        /// Имя файла конфигурации Агента.
        /// </summary>
        public const string CONFIG_FILE_NAME = "agent.config.xml";

        /// <summary>
        /// Конфигурация Агента.
        /// </summary>
        public AgentConfiguration Configuration
        {
            get { return _configuration; }
        }

        /// <summary>
        /// Хранит конфигурацию Агента.
        /// </summary>
        protected AgentConfiguration _configuration;

        /// <summary>
        /// Коллекция состояния сайтов.
        /// </summary>
        public List<ISite> Sites 
        {
            get { return _sites.Select(s => s).ToList(); }
        }

        /// <summary>
        /// Хранит коллекцию состояния сайтов.
        /// </summary>
        protected List<ISite> _sites = new List<ISite>();

        /// <summary>
        /// Основной конструктор.
        /// </summary>
        public AgentModel()
        {
            try
            {
                _configuration = AgentConfiguration.ReadConfiguration(CONFIG_FILE_NAME);

                foreach (var site in _configuration.Sites)
                {
                    _sites.Add(new SiteInstance(site.SiteName, site.ExecutableFilePath));
                }
            }
            catch (Exception e)
            {
                _configuration = new AgentConfiguration();
            }
        }

        /// <summary>
        /// Добавить сайт в конфигурацию.
        /// </summary>
        /// <param name="descriptor">Дескриптор сайта.</param>
        /// <returns>Результат выполнения.</returns>
        public bool AddSiteToConfig(SiteConfiguration siteConfig)
        {
            if (_configuration.AddNewSite(siteConfig))
            {
                SaveConfiguration();

                ISite siteInstance = new SiteInstance(siteConfig.SiteName, siteConfig.ExecutableFilePath);

                _sites.Add(siteInstance);

                if (SiteAdded != null)
                {
                    SiteAdded(this, new SiteEventArgs(siteInstance));
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Удалить сайт из конфигурации.
        /// </summary>
        /// <param name="siteName">Имя сайта.</param>
        /// <returns>Результат выполнения.</returns>
        public bool RemoveSiteFromConfig(string siteName)
        {
            if (_configuration.RemoveSite(siteName))
            {
                SaveConfiguration();

                ISite siteInstance = Sites.First(s => s.SiteName == siteName);

                _sites.Remove(siteInstance);

                if (SiteRemoved != null)
                {
                    SiteRemoved(this, new SiteEventArgs(siteInstance));
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Обновить сайт в конфигурации.
        /// </summary>
        /// <param name="descriptor">Дескриптор сайта.</param>
        /// <returns>Результат выполнения.</returns>
        public bool UpdateSiteConfig(SiteConfiguration siteConfig)
        {
            if (_configuration.UpdateSite(siteConfig))
            {
                SaveConfiguration();

                ISite siteInstance = Sites.First(s => s.SiteName == siteConfig.SiteName);

                _sites.Remove(siteInstance);
                siteInstance = new SiteInstance(siteConfig.SiteName, siteConfig.ExecutableFilePath);
                _sites.Add(siteInstance);

                if (SiteUpdated != null)
                {
                    SiteUpdated(this, new SiteEventArgs(siteInstance));
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Запущен ли сайт.
        /// </summary>
        /// <param name="siteName">Имя сайта.</param>
        /// <returns>Результат проверки.</returns>
        public bool IsSiteRunning(string siteName)
        {
            try
            {
                SiteStatus status = _sites.First(s => s.SiteName == siteName).Status;

                if (status == SiteStatus.RUNNING || status == SiteStatus.STARTING || 
                    status == SiteStatus.RUN_RUNTIME_ERROR || status == SiteStatus.RUN_STARTING_ERROR)
                {
                    return true;
                }
            }
            catch (InvalidOperationException e) { }

            return false;
        }

        /// <summary>
        /// Сохранение конфигурации в файл.
        /// </summary>
        private void SaveConfiguration()
        {
            try
            {
                AgentConfiguration.WriteConfiguration(_configuration, CONFIG_FILE_NAME);
            }
            catch (Exception e) { }
        }
    }
}
