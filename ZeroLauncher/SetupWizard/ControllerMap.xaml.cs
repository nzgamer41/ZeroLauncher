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

namespace ZeroLauncher.SetupWizard
{
    /// <summary>
    /// Interaction logic for ControllerMap.xaml
    /// </summary>
    public partial class ControllerMap : UserControl
    {
        MainWindow_new mainWindow;
        public ControllerMap()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            mainWindow = Window.GetWindow(this) as MainWindow_new;
        }
    }
}
