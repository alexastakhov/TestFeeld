using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlfaBank.AlfaRobot.ControlCenter.Common;

namespace AlfaBank.AlfaRobot.ControlCenter.Agent.Logic
{
    public class SiteEventArgs : EventArgs
    {
        /// <summary>
        /// Дескриптор сайта.
        /// </summary>
        public ISite Site { get; set; }

        /// <summary>
        /// Основной конструктор.
        /// </summary>
        /// <param name="site">Дескриптор сайта.</param>
        public SiteEventArgs(ISite site)
        {
            Site = site;
        }
    }
}
