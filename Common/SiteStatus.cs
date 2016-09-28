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
        /// Ошибка во время выполнения, сайт работает.
        /// </summary>
        RUN_RUNTIME_ERROR = 4,

        /// <summary>
        /// Ошибка во время выполнения, сайт остановлен.
        /// </summary>
        STOP_RUNTIME_ERROR = 5,

        /// <summary>
        /// Сайт не отвечает по WCF.
        /// </summary>
        UNAVAIBLE = 6,

        /// <summary>
        /// Исполняемый файл сайта не найден.
        /// </summary>
        FILE_NOT_EXISTS = 7,

        /// <summary>
        /// Ошибка во время запуска сайта, сайт работает.
        /// </summary>
        RUN_STARTING_ERROR = 8,

        /// <summary>
        /// Ошибка во время запуска сайта, сайт отсановлен.
        /// </summary>
        STOP_STARTING_ERROR = 9
    }
}
