using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using AlfaBank.AlfaRobot.ControlCenter.Configuration;

namespace AlfaBank.AlfaRobot.ControlCenter.Agent.GUI
{
    /// <summary>
    /// Логика взаимодействия для AddSiteForm.xaml
    /// </summary>
    public partial class AddSiteForm : Window
    {
        private SiteConfiguration _siteConfiguration;

        public AddSiteForm()
        {
            InitializeComponent();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            _siteConfiguration = null;
            this.Close();
        }

        private void createButton_Click(object sender, RoutedEventArgs e)
        {
            bool isHide = true;

            if (string.IsNullOrEmpty(siteNameText.Text))
            {
                SetLabelToError(siteNameLabel);
                isHide = false;
            }
            else 
            {
                SetLabelToNormal(siteNameLabel);
            }

            if (string.IsNullOrEmpty(filePathText.Text))
            {
                SetLabelToError(filePathLabel);
                isHide = false;
            }
            else
            {
                SetLabelToNormal(filePathLabel);
            }

            if (isHide)
            {
                _siteConfiguration = new SiteConfiguration()
                {
                    SiteName = siteNameText.Text,
                    ExecutableFilePath = filePathText.Text,
                    StartArguments = argumentsText.Text.Length > 0 ? argumentsText.Text.Split(' ').ToList() : new List<string>()
                };

                this.Close();
            }
        }

        public SiteConfiguration ShowDialogWithResult()
        {
            this.ShowDialog();
            return _siteConfiguration;
        }

        private void SetLabelToError(Label label)
        {
            label.Foreground = Brushes.Red;
            label.FontWeight = FontWeights.Bold;
        }

        private void SetLabelToNormal(Label label)
        {
            label.Foreground = Brushes.Black;
            label.FontWeight = FontWeights.Normal;
        }
    }
}
