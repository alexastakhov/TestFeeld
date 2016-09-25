using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlfaBank.AlfaRobot.ControlCenter.Common;

namespace AlfaBank.AlfaRobot.ControlCenter.Agent.Logic
{
    public interface IAgentModel
    {
        bool AddSiteToConfig(SiteDescriptor descriptor);
    }
}
