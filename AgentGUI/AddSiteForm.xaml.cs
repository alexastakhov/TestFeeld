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
using System.Windows.Shapes;
using AlfaBank.AlfaRobot.ControlCenter.Configuration;

namespace AlfaBank.AlfaRobot.ControlCenter.Agent
{
    /// <summary>
    /// Логика взаимодействия для AddSiteForm.xaml
    /// </summary>
    public partial class AddSiteForm : Window
    {
        private SiteConfiguration siteConfiguration;

        public AddSiteForm()
        {
            InitializeComponent();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            siteConfiguration = null;
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

            if (string.IsNullOrEmpty(argumentsText.Text))
            {
                SetLabelToError(argumentsLabel);
                isHide = false;
            }
            else
            {
                SetLabelToNormal(argumentsLabel);
            }

            if (isHide)
            {
                siteConfiguration = new SiteConfiguration()
                {
                    SiteName = siteNameText.Text,
                    ExecutableFilePath = filePathText.Text,
                    StartArguments = argumentsText.Text.Split(' ').ToList()
                };

                this.Close();
            }
        }

        public SiteConfiguration ShowDialogWithResult()
        {
            this.ShowDialog();
            return siteConfiguration;
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
