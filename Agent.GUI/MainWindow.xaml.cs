using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AlfaBank.AlfaRobot.ControlCenter.Configuration;
using AlfaBank.AlfaRobot.ControlCenter.Common;
using AlfaBank.AlfaRobot.ControlCenter.Agent.Logic;

namespace AlfaBank.AlfaRobot.ControlCenter.Agent.GUI
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Иконка в трее.
        /// </summary>
        private System.Windows.Forms.NotifyIcon TrayIcon = null;

        /// <summary>
        /// Контекстное меню трея.
        /// </summary>
        private ContextMenu TrayMenu = null;

        /// <summary>
        /// Состояние окна.
        /// </summary>
        private WindowState _fCurrentWindowState = WindowState.Normal;

        /// <summary>
        /// Флаг закрытия окна.
        /// </summary>
        private bool CanCloseWindow = false;

        public WindowState CurrentWindowState
        {
            get { return _fCurrentWindowState; }
            set { _fCurrentWindowState = value; }
        }

        /// <summary>
        /// Модель Агента.
        /// </summary>
        private IAgentModel _model;

        /// <summary>
        /// Модель представления таблицы сайтов.
        /// </summary>
        private ObservableCollection<SiteRowModel> SiteRows = new ObservableCollection<SiteRowModel>();

        /// <summary>
        /// Основной конструктор.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            _model = new AgentModel();
            _model.SiteUpdated += model_UpdateSiteHandler;
            _model.SiteAdded += model_AddSiteHandler;
            _model.SiteRemoved += model_RemoveSiteHandler;

            SiteRows = InitSiteRows(_model.Sites);
            sitesDataGrid.ItemsSource = SiteRows;
        }

        /// <summary>
        /// Создание значка в трее.
        /// </summary>
        /// <returns></returns>
        private bool CreateTrayIcon()
        {
            bool result = false;
            if (TrayIcon == null)
            {
                TrayIcon = new System.Windows.Forms.NotifyIcon();
                TrayIcon.Icon = AlfaBank.AlfaRobot.ControlCenter.Agent.Gui.Properties.Resources.switch_tray_m;
                TrayIcon.Text = "AlfaRobot Control Agent.";
                TrayMenu = Resources["trayMenu"] as ContextMenu;

                TrayIcon.Click += delegate(object sender, EventArgs e)
                {
                    if ((e as System.Windows.Forms.MouseEventArgs).Button == System.Windows.Forms.MouseButtons.Left)
                    {
                        ShowHideMainWindow(sender, null);
                    }
                    else
                    {
                        TrayMenu.IsOpen = true;
                        Activate();
                    }
                };
                result = true;
            }
            else
            {
                result = true;
            }
            TrayIcon.Visible = true;
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowHideMainWindow(object sender, RoutedEventArgs e)
        {
            TrayMenu.IsOpen = false;
            if (IsVisible)
            {
                Hide();
            }
            else
            {
                Show();
                WindowState = CurrentWindowState;
                Activate();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void showWinContextMenuItemClick(object sender, RoutedEventArgs e)
        {
            Show();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stopContextMenuItemClick(object sender, RoutedEventArgs e)
        {
            CanCloseWindow = true;
            Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!CanCloseWindow)
            {
                e.Cancel = true;
                ShowHideMainWindow(this, new RoutedEventArgs());
            }
            else
            {
                TrayIcon.Visible = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            CreateTrayIcon();
        }

        /// <summary>
        /// Обработка нажатия кнопки добавления нового сайта.
        /// </summary>
        /// <param name="sender">Вызвавший объект.</param>
        /// <param name="e">Параметры события.</param>
        private void addSiteToolButton_Click(object sender, RoutedEventArgs e)
        {
            AddSiteForm addSiteForm = new AddSiteForm();

            addSiteForm.Left = this.Left + ((this.Width - addSiteForm.Width) / 2);
            addSiteForm.Top = this.Top + ((this.Height - addSiteForm.Height) / 2);

            SiteConfiguration siteConfig = addSiteForm.ShowDialogWithResult();

            if (siteConfig != null)
            {
                if (!_model.AddSiteToConfig(siteConfig))
                {
                    MessageBox.Show(
                        this,
                        string.Format("Sitename \"{0}\" already exists in the Agent configuration.", siteConfig.SiteName),
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// Добавление сайта.
        /// </summary>
        /// <param name="sender">Вызывающий объект.</param>
        /// <param name="arg">Добавляемый сайт.</param>
        protected void model_AddSiteHandler(object sender, SiteEventArgs arg)
        {
            if (SiteRows.Count(r => r.SiteName == arg.Site.SiteName) == 0)
            {
                SiteRows.Add(
                    new SiteRowModel()
                    {
                        SiteName = arg.Site.SiteName,
                        FilePath = arg.Site.FilePath,
                        SiteStatus = StateToString(arg.Site.Status),
                        StartTime = arg.Site.StartTime != null ? arg.Site.StartTime.ToString() : string.Empty
                    });
            }
        }

        /// <summary>
        /// Удаление сайта.
        /// </summary>
        /// <param name="sender">Вызывающий объект.</param>
        /// <param name="arg">Удаляемый сайт.</param>
        protected void model_RemoveSiteHandler(object sender, SiteEventArgs arg)
        {
            if (SiteRows.Count(r => r.SiteName == arg.Site.SiteName) > 0)
            {
                SiteRows.Remove(SiteRows.First(r => r.SiteName == arg.Site.SiteName));
            }
        }

        /// <summary>
        /// Обновление сайта.
        /// </summary>
        /// <param name="sender">Вызывающий объект.</param>
        /// <param name="arg">Сайт с изменениями.</param>
        protected void model_UpdateSiteHandler(object sender, SiteEventArgs arg)
        {
            if (SiteRows.Count(r => r.SiteName == arg.Site.SiteName) > 0)
            {
                SiteRowModel row = SiteRows.First(r => r.SiteName == arg.Site.SiteName);

                row.FilePath = arg.Site.FilePath;
                row.SiteStatus = StateToString(arg.Site.Status);
                row.StartTime = arg.Site.StartTime != null ? arg.Site.StartTime.ToString() : string.Empty;
            }
        }

        protected string StateToString(SiteStatus status)
        {
            switch (status)
            {
                case SiteStatus.FILE_NOT_EXISTS: return "File not found";
                case SiteStatus.RUNNING: return "Running";
                case SiteStatus.RUN_RUNTIME_ERROR: return "Running, error detected";
                case SiteStatus.STOP_RUNTIME_ERROR: return "Stopped, error detected";
                case SiteStatus.STARTING: return "Starting";
                case SiteStatus.RUN_STARTING_ERROR: return "Starting, error detected";
                case SiteStatus.STOP_STARTING_ERROR: return "Stopped, error while stirting";
                case SiteStatus.STOPPED: return "Stopped";
                case SiteStatus.UNAVAIBLE: return "Unavailable";
                default: return "Unavailable";
            }
        }

        /// <summary>
        /// Обработка нажатия кнопки удаления сайта.
        /// </summary>
        /// <param name="sender">Вызвавший объект.</param>
        /// <param name="e">Параметры события.</param>
        private void deleteSiteToolButton_Click(object sender, RoutedEventArgs e)
        {
            if (sitesDataGrid.SelectedItems.Count > 0)
            {
                if (MessageBox.Show(
                    "Are you sure that you want to remove selected site settings?", 
                    "Removing site settings", 
                    MessageBoxButton.YesNo, 
                    MessageBoxImage.Warning, 
                    MessageBoxResult.No) == MessageBoxResult.Yes)
                {
                    SiteRowModel[] rows = new SiteRowModel[sitesDataGrid.SelectedItems.Count];
                    sitesDataGrid.SelectedItems.CopyTo(rows, 0);

                    foreach (var row in rows)
                    {
                        string siteName = ((SiteRowModel)row).SiteName;

                        if (!_model.IsSiteRunning(siteName))
                        {
                            _model.RemoveSiteFromConfig(siteName);
                        }
                        else
                        {
                            //
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Инициализация списка строк сайтов.
        /// </summary>
        /// <param name="sites">Коллекция сайтов из конфигурации.</param>
        /// <returns>Модель таблицы.</returns>
        private ObservableCollection<SiteRowModel> InitSiteRows(List<ISite> sites)
        {
            ObservableCollection<SiteRowModel> list = new ObservableCollection<SiteRowModel>();

            foreach (var site in sites)
            {
                list.Add(new SiteRowModel()
                {
                    SiteName = site.SiteName,
                    FilePath = site.FilePath,
                    SiteStatus = StateToString(site.Status),
                    StartTime = site.StartTime.ToString()
                });
            }

            return list;
        }
    }
}
