using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlfaBank.AlfaRobot.ControlCenter.Common
{
    public enum SiteStatus
    {
        /// <summary>
        /// Сайт в процессе запуска.
        /// </summary>
        STARTING = 1,

        /// <summary>
        /// Сайт запущен.
        /// </summary>
        RUNNING = 2,

        /// <summary>
        /// Сайт остановлен.
        /// </summary>
        STOPPED = 3,

        /// <summary>
        /// Ошибка во время выполнения.
        /// </summary>
        RUNTIME_ERROR  = 4,

        /// <summary>
        /// Сайт не отвечает по WCF.
        /// </summary>
        UNAVAIBLE = 5,

        /// <summary>
        /// Исполняемый файл сайта не найден.
        /// </summary>
        FILE_NOT_EXISTS = 6,

        /// <summary>
        /// Ошибка во время запуска сайта.
        /// </summary>
        STARTING_ERROR = 7
    }
}
