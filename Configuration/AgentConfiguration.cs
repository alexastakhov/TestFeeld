using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace AlfaBank.AlfaRobot.ControlCenter.Configuration
{
    /// <summary>
    /// Класс конфигурации приложения Агента.
    /// </summary>
    [DataContract]
    public class AgentConfiguration
    {
        /// <summary>
        /// URI локальной службы WCF.
        /// </summary>
        [DataMember]
        public string wcfServiceUri;

        /// <summary>
        /// URI службы WCF управляющего сервера.
        /// </summary>
        [DataMember]
        public string wcfControlServiceUri;

        /// <summary>
        /// Адрес управляющего сервера.
        /// </summary>
        [DataMember]
        public string ControlServerAddress;

        /// <summary>
        /// Строка доступа на управляющий сервер.
        /// </summary>
        [DataMember]
        public string ControlServerAccessString;

        /// <summary>
        /// Список конфигураций сайтов.
        /// </summary>
        [DataMember]
        public List<SiteConfiguration> Sites
        { 
            get { return _sites.Select(s => s).ToList(); } 
        }

        /// <summary>
        /// Хранит список конфигураций сайтов.
        /// </summary>
        [DataMember]
        protected List<SiteConfiguration> _sites;

        /// <summary>
        /// Основной конструктор.
        /// </summary>
        public AgentConfiguration()
        {
            _sites = new List<SiteConfiguration>();
        }

        /// <summary>
        /// Добавление сайта в конфигурацию.
        /// </summary>
        /// <param name="siteName">Имя сайта.</param>
        /// <param name="filePath">Путь к файлу.</param>
        /// <param name="args">Аргументы запуска.</param>
        /// <returns>Результат выполнения.</returns>
        public bool AddNewSite(string siteName, string filePath, List<string> args)
        {
            if (_sites.Where(s => s.SiteName == siteName).Count() == 0)
            {
                _sites.Add(
                    new SiteConfiguration()
                    {
                        SiteName = siteName,
                        ExecutableFilePath = filePath,
                        StartArguments = args
                    });

                return true;
            }
            return false;
        }

        /// <summary>
        /// Удаление сайта из конфигурации.
        /// </summary>
        /// <param name="siteName">Имя сайта.</param>
        /// <returns>Результат выполнения.</returns>
        public bool RemoveSite(string siteName)
        {
            if (_sites.Where(s => s.SiteName == siteName).Count() == 1)
            {
                _sites.Remove(_sites.First(s => s.SiteName == siteName));

                return true;
            }
            return false;
        }

        /// <summary>
        /// Обновление конфигурации сайта.
        /// </summary>
        /// <param name="siteName">Имя сайта.</param>
        /// <param name="filePath">Путь к файлу.</param>
        /// <param name="args">Аргументы запуска.</param>
        /// <returns>Результат выполнения.</returns>
        public bool UpdateSite(string siteName, string filePath, List<string> args)
        {
            if (_sites.Where(s => s.SiteName == siteName).Count() == 1)
            {
                SiteConfiguration site = _sites.First(s => s.SiteName == siteName);

                site.ExecutableFilePath = filePath;
                site.StartArguments = args;

                return true;
            }

            return false;
        }
    }
}
