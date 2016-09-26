using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlfaBank.AlfaRobot.ControlCenter.Common;
using AlfaBank.AlfaRobot.ControlCenter.Configuration;

namespace AlfaBank.AlfaRobot.ControlCenter.Agent.Logic
{
    public interface IAgentModel
    {
        /// <summary>
        /// Событие изменения состояния сайта.
        /// </summary>
        event EventHandler<SiteEventArgs> SiteUpdated;

        /// <summary>
        /// Конфигурация агента.
        /// </summary>
        AgentConfiguration Configuration { get; }

        /// <summary>
        /// Коллекция состояния сайтов.
        /// </summary>
        List<ISite> Sites { get; }

        /// <summary>
        /// Добавить сайт в конфигурацию.
        /// </summary>
        /// <param name="descriptor">Дескриптор сайта.</param>
        /// <returns>Результат выполнения.</returns>
        bool AddSiteToConfig(SiteDescriptor descriptor);
    }
}
