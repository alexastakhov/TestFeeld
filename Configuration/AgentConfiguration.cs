using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.IO;
using System.Xml;

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
        public string WcfServiceUri
        {
            get;
            set;
        }

        /// <summary>
        /// URI службы WCF управляющего сервера.
        /// </summary>
        [DataMember]
        public string WcfControlServiceUri
        {
            get;
            set;
        }

        /// <summary>
        /// Адрес управляющего сервера.
        /// </summary>
        [DataMember]
        public string ControlServerAddress
        {
            get;
            set;
        }

        /// <summary>
        /// Строка доступа на управляющий сервер.
        /// </summary>
        [DataMember]
        public string ControlServerAccessString
        {
            get;
            set;
        }

        /// <summary>
        /// Список конфигураций сайтов.
        /// </summary>
        [DataMember]
        public List<SiteConfiguration> Sites
        {
            get 
            { 
                return _sites; 
            }

            set 
            { 
                _sites = value; 
            }
        }

        /// <summary>
        /// Хранит список конфигураций сайтов.
        /// </summary>
        [DataMember]
        protected List<SiteConfiguration> _sites;

        /// <summary>
        /// Чтение конфигурации из файла.
        /// </summary>
        public static AgentConfiguration ReadConfiguration(string filePath)
        {
            try
            {
                AgentConfiguration deserializedConfig = null;

                using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
                {
                    XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(fileStream, new XmlDictionaryReaderQuotas());
                    DataContractSerializer serializer = new DataContractSerializer(typeof(AgentConfiguration));

                    deserializedConfig = (AgentConfiguration)serializer.ReadObject(reader, true);
                    reader.Close();
                }

                return deserializedConfig;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Запись конфигурацииы в файл.
        /// </summary>
        public static void WriteConfiguration(AgentConfiguration configuration, string filePath)
        {
            try
            {
                using (FileStream writer = new FileStream(filePath, FileMode.OpenOrCreate))
                {
                    DataContractSerializer serializer = new DataContractSerializer(typeof(AgentConfiguration));
                    serializer.WriteObject(writer, configuration);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

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
        public bool AddNewSite(SiteConfiguration siteConfig)
        {
            if (_sites.Where(s => s.SiteName.ToLower() == siteConfig.SiteName.ToLower()).Count() == 0)
            {
                _sites.Add(siteConfig);

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
            if (_sites.Where(s => s.SiteName.ToLower() == siteName.ToLower()).Count() == 1)
            {
                _sites.Remove(_sites.First(s => s.SiteName.ToLower() == siteName.ToLower()));

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
        public bool UpdateSite(SiteConfiguration siteConfig)
        {
            if (_sites.Where(s => s.SiteName.ToLower() == siteConfig.SiteName.ToLower()).Count() == 1)
            {
                SiteConfiguration site = _sites.First(s => s.SiteName.ToLower() == siteConfig.SiteName.ToLower());

                site.ExecutableFilePath = siteConfig.ExecutableFilePath;
                site.StartArguments = siteConfig.StartArguments;

                return true;
            }

            return false;
        }
    }
}
