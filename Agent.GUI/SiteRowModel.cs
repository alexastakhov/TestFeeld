using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlfaBank.AlfaRobot.ControlCenter.Agent.GUI
{
    class SiteRowModel
    {
        /// <summary>
        /// Имя сайта.
        /// </summary>
        public string SiteName { get; set; }

        /// <summary>
        /// Путь к исполняемому файлу.
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Статус сайта.
        /// </summary>
        public string SiteStatus { get; set; }

        /// <summary>
        /// Время запуска сайта.
        /// </summary>
        public string StartTime { get; set; }
    }
}
