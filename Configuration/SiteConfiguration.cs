using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AlfaBank.AlfaRobot.ControlCenter.Configuration
{
    [Serializable]
    public class SiteConfiguration
    {
        /// <summary>
        /// Имя сайта.
        /// </summary>
        public string SiteName
        { get; set; }

        /// <summary>
        /// Путь к исполняемому файлу сайта.
        /// </summary>
        public string ExecutableFilePath
        { get; set; }

        /// <summary>
        /// Аргументы командной строки при запуске сайта.
        /// </summary>
        public List<string> StartArguments;
    }
}
