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
        /// Событие обновления сайта.
        /// </summary>
        public event EventHandler<SiteEventArgs> SiteUpdated = delegate { };

        /// <summary>
        /// Конфигурация Агента.
        /// </summary>
        public AgentConfiguration Configuration
        {
            get { return _configuration; }
        }

        /// <summary>
        /// Хранит конфигурацию Агента.
        /// </summary>
        protected AgentConfiguration _configuration;

        /// <summary>
        /// Коллекция состояния сайтов.
        /// </summary>
        public List<ISite> Sites 
        {
            get { return _sites.Select(s => s).ToList(); }
        }

        /// <summary>
        /// Хранит коллекцию состояния сайтов.
        /// </summary>
        protected List<ISite> _sites = new List<ISite>();

        /// <summary>
        /// Основной конструктор.
        /// </summary>
        public AgentModel()
        {
            _configuration = new AgentConfiguration();
        }

        public bool AddSiteToConfig(SiteDescriptor descriptor)
        {
            if (_configuration.AddNewSite(descriptor.SiteName, descriptor.ExecutableFilePath, descriptor.StartArguments))
            {
                _sites.Add(new SiteInstance(descriptor.SiteName, descriptor.ExecutableFilePath));

                return true;
            }

            return false;
        }
    }
}
