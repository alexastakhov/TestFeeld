using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AlfaBank.AlfaRobot.ControlCenter.Agent.Configuration
{
    [Serializable]
    public class SiteConfiguration
    {
        /// <summary>
        /// Имя сайта.
        /// </summary>
        public string SiteName;

        /// <summary>
        /// Путь к исполняемому файлу сайта.
        /// </summary>
        public string ExecutableFilePath;

        /// <summary>
        /// Аргументы командной строки при запуске сайта.
        /// </summary>
        public string[] StartArguments;
    }
}
