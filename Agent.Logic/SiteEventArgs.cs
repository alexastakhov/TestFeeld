using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlfaBank.AlfaRobot.ControlCenter.Common;

namespace AlfaBank.AlfaRobot.ControlCenter.Agent.Logic
{
    public class SiteEventArgs : EventArgs
    {
        public ISite Site { get; set; }

        public SiteEventArgs(ISite site)
        {
            Site = site;
        }
    }
}
