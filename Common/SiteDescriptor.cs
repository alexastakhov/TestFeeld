using System;
using System.Collections.Generic;
using System.Text;

namespace AlfaBank.AlfaRobot.ControlCenter.Common
{
    public class SiteDescriptor
    {
        /// <summary>
        /// Имя сайта.
        /// </summary>
        public string SiteName { get; set; }

        /// <summary>
        /// Путь к исполняемому файлу сайта.
        /// </summary>
        public string ExecutableFilePath { get; set; }

        /// <summary>
        /// Аргументы командной строки при запуске сайта.
        /// </summary>
        public List<string> StartArguments {get; set;}
    }
}
