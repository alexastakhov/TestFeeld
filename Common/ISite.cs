using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlfaBank.AlfaRobot.ControlCenter.Common
{
    public interface ISite
    {
        /// <summary>
        /// Имя сайта.
        /// </summary>
        string SiteName { get; }

        /// <summary>
        /// Путь к исполняему файлу.
        /// </summary>
        string FilePath { get; }

        /// <summary>
        /// Статус сайта.
        /// </summary>
        SiteStatus Status { get; }

        /// <summary>
        /// Время запуска сайта.
        /// </summary>
        DateTime? StartTime { get; }

        /// <summary>
        /// Отмечаем сайт как запущенный.
        /// </summary>
        void SetToStarting();
    }
}
