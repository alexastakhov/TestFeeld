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
        /// Событие добавления сайта.
        /// </summary>
        event EventHandler<SiteEventArgs> SiteAdded;

        /// <summary>
        /// Событие удаления сайта.
        /// </summary>
        event EventHandler<SiteEventArgs> SiteRemoved;

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
        bool AddSiteToConfig(SiteConfiguration siteConfig);

        /// <summary>
        /// Удалить сайт из конфигурации.
        /// </summary>
        /// <param name="siteName">Имя сайта.</param>
        /// <returns>Результат выполнения.</returns>
        bool RemoveSiteFromConfig(string siteName);

        /// <summary>
        /// Обновить сайт в конфигурации.
        /// </summary>
        /// <param name="descriptor">Дескриптор сайта.</param>
        /// <returns>Результат выполнения.</returns>
        bool UpdateSiteConfig(SiteConfiguration siteConfig);

        /// <summary>
        /// Запущен ли сайт.
        /// </summary>
        /// <param name="siteName">Имя сайта.</param>
        /// <returns>Результат проверки.</returns>
        bool IsSiteRunning(string siteName);
    }
}
