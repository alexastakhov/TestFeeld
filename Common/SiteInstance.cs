using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlfaBank.AlfaRobot.ControlCenter.Common
{
    /// <summary>
    /// Класс, представляющий собой дескриптор сайта.
    /// </summary>
    public class SiteInstance : ISite
    {
        /// <summary>
        /// Имя сайта.
        /// </summary>
        public string SiteName 
        { 
            get { return _siteName; } 
        }

        /// <summary>
        /// Хранит имя сайта.
        /// </summary>
        protected string _siteName;

        /// <summary>
        /// Путь к исполняему файлу.
        /// </summary>
        public string FilePath 
        { 
            get { return _filePath; } 
        }

        /// <summary>
        /// Хранит путь к исполняему файлу.
        /// </summary>
        protected string _filePath;

        /// <summary>
        /// Статус сайта.
        /// </summary>
        public SiteStatus Status 
        {
            get { return _status; }
            set { _status = value; }
        }

        /// <summary>
        /// Хранит статус сайта.
        /// </summary>
        protected SiteStatus _status;

        /// <summary>
        /// Время запуска сайта.
        /// </summary>
        public DateTime? StartTime 
        {
            get { return _startTime; }
            set { _startTime = value; }
        }

        /// <summary>
        /// Хранит время запуска сайта.
        /// </summary>
        protected DateTime? _startTime;

        public SiteInstance(string siteName, string filePath)
        {
            _siteName = siteName;
            _filePath = filePath;
            _status = File.Exists(filePath) ? SiteStatus.STOPPED : SiteStatus.FILE_NOT_EXISTS;
        }

        /// <summary>
        /// Отмечаем сайт как запущенный.
        /// </summary>
        public void SetToStarting()
        {
            _startTime = DateTime.Now;
        }
    }
}
