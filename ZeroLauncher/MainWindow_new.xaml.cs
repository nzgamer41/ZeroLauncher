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
using ZeroLauncher.SetupWizard;

namespace ZeroLauncher
{
    /// <summary>
    /// Interaction logic for MainWindow_new.xaml
    /// </summary>
    public partial class MainWindow_new : Window
    {
        int curControl = 0;
        public IDZConfig newConfig = new IDZConfig();
        public bool redistsComplete = false;
        public MainWindow_new()
        {
            InitializeComponent();
            setupWizard.Content = new SetupWizard.Intro();
        }

        private void buttonNext_Click(object sender, RoutedEventArgs e)
        {
            if (newConfig.XOrDInput && curControl == 1)
            {
                //Skip Controller Selector
                curControl = 3;
            }
            else
            {
                curControl++;
            }
            changeControl();
        }

        private void buttonPrev_Click(object sender, RoutedEventArgs e)
        {
            if (newConfig.XOrDInput && curControl == 3)
            {
                //Skip Controller Selector
                curControl = 1;
            }
            else
            {
                curControl--;
            }
            changeControl();
        }

        public void nextUC()
        {
            if (newConfig.XOrDInput && curControl == 1)
            {
                //Skip Controller Selector
                curControl = 3;
            }
            else
            {
                curControl++;
            }
            changeControl();
        }

        private void changeControl()
        {
            switch (curControl)
            {
                case 0:
                    setupWizard.Content = new SetupWizard.Intro();
                    buttonPrev.IsEnabled = false;
                    break;
                case 1:
                    setupWizard.Content = new SetupWizard.Controller();
                    buttonPrev.IsEnabled = true;
                    break;
                case 2:
                    setupWizard.Content = new SetupWizard.ControllerMap();
                    break;
                case 3:
                    setupWizard.Content = new SetupWizard.Prereqs();
                    break;
                case 4:
                    setupWizard.Content = new SetupWizard.GameSetup();
                    break;
                default:
                    this.Close();
                    break;
            }
        }
    }
}
