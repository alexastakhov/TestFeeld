using System;
using System.Collections.Generic;
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
using System.Collections.ObjectModel;

namespace AlfaBank.AlfaRobot.ControlCenter.Agent
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private System.Windows.Forms.NotifyIcon TrayIcon = null;
        private ContextMenu TrayMenu = null;
        private WindowState fCurrentWindowState = WindowState.Normal;

        private bool CanCloseWindow = false;

        public WindowState CurrentWindowState
        {
            get { return fCurrentWindowState; }
            set { fCurrentWindowState = value; }
        }

        public ObservableCollection<SiteConfiguration> sites;

        /// <summary>
        /// Конфигурация Агента.
        /// </summary>
        private AgentConfiguration agentConfiguration = new AgentConfiguration();

        /// <summary>
        /// Основной конструктор.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            //sites = agentConfiguration.Sites;
            sitesDataGrid.ItemsSource = agentConfiguration.Sites;
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
                TrayIcon.Icon = AlfaBank.AlfaRobot.ControlCenter.Agent.Properties.Resources.switch_tray_m; 
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

        private void showWinContextMenuItemClick(object sender, RoutedEventArgs e)
        {
            Show();
        }

        private void stopContextMenuItemClick(object sender, RoutedEventArgs e)
        {
            CanCloseWindow = true;
            Close();
        }

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

        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            CreateTrayIcon();
        }

        /// <summary>
        /// Обработка нажатия кнопки добавления нового сайта.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addSiteToolButton_Click(object sender, RoutedEventArgs e)
        {
            AddSiteForm addSiteForm = new AddSiteForm();

            addSiteForm.Left = this.Left + ((this.Width - addSiteForm.Width) / 2);
            addSiteForm.Top = this.Top + ((this.Height - addSiteForm.Height) / 2);

            SiteConfiguration siteConfiguration = addSiteForm.ShowDialogWithResult();

            if (siteConfiguration != null)
            {
                if (agentConfiguration.Sites.Where(s => (s.SiteName == siteConfiguration.SiteName)).Count() == 0)
                {
                    agentConfiguration.Sites.Add(siteConfiguration);
                    //sitesDataGrid.ItemsSource = null;
                    //sitesDataGrid.ItemsSource = agentConfiguration.Sites;
                }
                else
                {
                    MessageBox.Show(
                        this,
                        string.Format("Sitename \"{0}\" already exists in the Agent configuration.",siteConfiguration.SiteName),
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }
    }
}
