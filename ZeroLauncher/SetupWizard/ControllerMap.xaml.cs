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
using SharpDX.DirectInput;

namespace ZeroLauncher.SetupWizard
{
    /// <summary>
    /// Interaction logic for ControllerMap.xaml
    /// </summary>
    public partial class ControllerMap : UserControl
    {
        List<DeviceInstance> controllers = new List<DeviceInstance>();
        private DirectInput dInput;
        MainWindow_new mainWindow;
        public ControllerMap()
        {
            InitializeComponent();
            dInput = new DirectInput();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            mainWindow = Window.GetWindow(this) as MainWindow_new;
            mainWindow.buttonNext.IsEnabled = true;
            controllerList();
        }

        private void controllerList()
        {
            foreach (var device in dInput.GetDevices(DeviceType.Gamepad, DeviceEnumerationFlags.AttachedOnly))
            {
                controllers.Add(device);
                listBoxDevices.Items.Add(device.ProductName);
            }
            foreach (var device in dInput.GetDevices(DeviceType.Joystick, DeviceEnumerationFlags.AttachedOnly))
            {
                controllers.Add(device);
                listBoxDevices.Items.Add(device.ProductName);
            }
            foreach (var device in dInput.GetDevices(DeviceType.Driving, DeviceEnumerationFlags.AttachedOnly))
            {
                controllers.Add(device);
                listBoxDevices.Items.Add(device.ProductName);
            }
            foreach (var device in dInput.GetDevices(DeviceType.Flight, DeviceEnumerationFlags.AttachedOnly))
            {
                controllers.Add(device);
                listBoxDevices.Items.Add(device.ProductName);
            }
            foreach (var device in dInput.GetDevices(DeviceType.FirstPerson, DeviceEnumerationFlags.AttachedOnly))
            {
                controllers.Add(device);
                listBoxDevices.Items.Add(device.ProductName);
            }
        }

        private void listBoxDevices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            mainWindow.newConfig.devName = controllers[listBoxDevices.SelectedIndex].ProductName;
        }
    }
}
