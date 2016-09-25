using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlfaBank.AlfaRobot.ControlCenter.Configuration;
using AlfaBank.AlfaRobot.ControlCenter.Common;

namespace AlfaBank.AlfaRobot.ControlCenter.Agent.Logic
{
    public class AgentModel : IAgentModel
    {
        /// <summary>
        /// Конфигурация Агента.
        /// </summary>
        private AgentConfiguration agentConfiguration;

        /// <summary>
        /// Основной конструктор.
        /// </summary>
        public AgentModel()
        {
            agentConfiguration = new AgentConfiguration();
        }

        public bool AddSiteToConfig(SiteDescriptor descriptor)
        {
            return false;
        }
    }
}
