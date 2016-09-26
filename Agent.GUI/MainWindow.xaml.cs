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
        private AgentModel _model;

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
            sitesDataGrid.ItemsSource = SiteRows;

            _model = new AgentModel();
            _model.SiteUpdated += model_UpdateSite;
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addSiteToolButton_Click(object sender, RoutedEventArgs e)
        {
            AddSiteForm addSiteForm = new AddSiteForm();

            addSiteForm.Left = this.Left + ((this.Width - addSiteForm.Width) / 2);
            addSiteForm.Top = this.Top + ((this.Height - addSiteForm.Height) / 2);

            SiteDescriptor siteDescriptor = addSiteForm.ShowDialogWithResult();

            if (siteDescriptor != null)
            {
                if (!_model.AddSiteToConfig(siteDescriptor))
                {
                    MessageBox.Show(
                        this,
                        string.Format("Sitename \"{0}\" already exists in the Agent configuration.", siteDescriptor.SiteName),
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
            else 
            {
                MessageBox.Show(
                        this,
                        string.Format("Unexpexting Site addition error!"),
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Обновление сайта.
        /// </summary>
        /// <param name="sender">Вызывающий объект.</param>
        /// <param name="arg">Сайт с изменениями.</param>
        private void model_UpdateSite(object sender, SiteEventArgs arg)
        {

        }
    }
}
