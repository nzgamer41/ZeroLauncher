using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SharpDX.DirectInput;
using System.Windows.Threading;

namespace ZeroLauncher
{
    /// <summary>
    /// Interaction logic for ControllerSettings.xaml
    /// </summary>
    public partial class ControllerSettings : Window
    {
        private bool _isXinput;
        private IDZConfig _gameProfile;
        private DirectInput dInput;
        private static TextBox selTextBox;
        private Joystick _joystick;
        private bool _listen;
        private Thread thControl;
        //private static Thread _inputListener;
        public ControllerSettings(IDZConfig gameProfile, bool isXinput)
        {
            InitializeComponent();
            _gameProfile = gameProfile;
            _isXinput = isXinput;
            if (_joystick != null)
            {
                _joystick.Dispose();
            }
            dInput = new DirectInput();
            var joystickGuid = Guid.Empty;
            foreach (var device in dInput.GetDevices(DeviceType.Joystick, DeviceEnumerationFlags.AttachedOnly))
            {
                joystickGuid = device.InstanceGuid;
                break;
            }
            if (joystickGuid == Guid.Empty)
            {
                MessageBox.Show("No DirectInput joystick connected");
                return;
            }

            _joystick = new Joystick(dInput, joystickGuid);
            _joystick.Properties.BufferSize = 512;
            _joystick.Acquire();
            ThreadStart thsControl = new ThreadStart(() => PollPad());
            thControl = new Thread(thsControl);
            thControl.Start();

        }

        private void PollPad()
        {
            while (!_listen)
            {
                _joystick.Poll();
                var datas = _joystick.GetBufferedData();
                foreach (var state in datas)
                    this.Dispatcher.Invoke(() => { try { selTextBox.Text = state.Offset.ToString(); } catch { } });
            }
        }
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_PreviewMouseDown_1(object sender, MouseButtonEventArgs e)
        {
            selTextBox = (sender as TextBox);
        }
        private List<TextBox> GetAllTextBox()
        {
            List<TextBox> textBoxes = new List<TextBox>();
            /// casting the content into panel
            Panel mainContainer = (Panel)this.Content;

            /// GetAll UIElement
            UIElementCollection element = mainContainer.Children;

            StackPanel controls = (StackPanel)element[0];

            UIElementCollection element2 = controls.Children;

            /// casting the UIElementCollection into List
            List<FrameworkElement> lstElement = element2.Cast<FrameworkElement>().ToList();

            /// Geting all Control from list
            var lstControl = lstElement.OfType<Control>();

            foreach (Control control in lstControl)
            {
                if (control is TextBox)
                {
                    textBoxes.Add((TextBox)control);
                }
            }
            return textBoxes;
        }
        private void StackPanel_Unloaded(object sender, RoutedEventArgs e)
        {

        }
        private void updateTextBoxes()
        {
            List<TextBox> controls = GetAllTextBox();
            try
            {
                controls[0].Text = "Buttons" + _gameProfile.brakeAxis;
                controls[1].Text = "Buttons" + _gameProfile.accelAxis;
                controls[2].Text = "Buttons" + _gameProfile.startButton;
                controls[3].Text = "Buttons" + _gameProfile.viewChg;
                controls[4].Text = "Buttons" + _gameProfile.shiftDn;
                controls[5].Text = "Buttons" + _gameProfile.shiftUp;
                controls[6].Text = "Buttons" + _gameProfile.gear1;
                controls[7].Text = "Buttons" + _gameProfile.gear2;
                controls[8].Text = "Buttons" + _gameProfile.gear3;
                controls[9].Text = "Buttons" + _gameProfile.gear4;
                controls[10].Text = "Buttons" + _gameProfile.gear5;
                controls[11].Text = "Buttons" + _gameProfile.gear6;
            }
            catch
            {

            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            List<TextBox> controls = GetAllTextBox();

            //transfer settings to IDZConfig
            _gameProfile.brakeAxis = controls[0].Text.Replace("Buttons", "");
            _gameProfile.accelAxis = controls[1].Text.Replace("Buttons", "");
            _gameProfile.startButton = controls[2].Text.Replace("Buttons", "");
            _gameProfile.viewChg = controls[3].Text.Replace("Buttons", "");
            _gameProfile.shiftDn = controls[4].Text.Replace("Buttons", "");
            _gameProfile.shiftUp = controls[5].Text.Replace("Buttons", "");
            _gameProfile.gear1 = controls[6].Text.Replace("Buttons", "");
            _gameProfile.gear2 = controls[7].Text.Replace("Buttons", "");
            _gameProfile.gear3 = controls[8].Text.Replace("Buttons", "");
            _gameProfile.gear4 = controls[9].Text.Replace("Buttons", "");
            _gameProfile.gear5 = controls[10].Text.Replace("Buttons", "");
            _gameProfile.gear6 = controls[11].Text.Replace("Buttons", "");

            MainWindow.gameConfig = _gameProfile;

            _listen = false;
            _joystick.Unacquire();
            thControl.Abort();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            updateTextBoxes();
        }
    }
}
