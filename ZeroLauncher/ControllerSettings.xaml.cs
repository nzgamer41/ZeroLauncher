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
            foreach (var device in dInput.GetDevices(DeviceType.Driving, DeviceEnumerationFlags.AttachedOnly))
            {
                joystickGuid = device.InstanceGuid;
                break;
            }
            foreach (var device in dInput.GetDevices(DeviceType.Gamepad, DeviceEnumerationFlags.AttachedOnly))
            {
                joystickGuid = device.InstanceGuid;
                break;
            }
            foreach (var device in dInput.GetDevices(DeviceType.FirstPerson, DeviceEnumerationFlags.AttachedOnly))
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
                {
                    //better check for POV thing
                    if (state.Offset.ToString().Contains("PointOfViewControllers"))
                    {
                        MessageBox.Show("Whoops, you've tried to map the D-Pad of your Wheel/Controller to a button.\n\nThis is mapped to the IDZ cabinet's arrow keys already.", state.Offset.ToString() + " is not a valid input!");
                    }
                    else
                    {
                        if (state.Offset.ToString() == "X" || state.Offset.ToString() == "Y" || state.Offset.ToString() == "Z")
                        {
                            this.Dispatcher.Invoke(() => { try { selTextBox.Text = state.Offset.ToString() + " Axis"; } catch { } });
                        }
                        else
                        {
                            if (state.Offset.ToString().Contains("Buttons"))
                            {
                                /* FUCKING SHARPDX
                                 * Lemme tell you why this is here.
                                 * SharpDX for some fucking reason maps the buttons like "Button 1" on your DInput gamepad is called "Buttons0"
                                 * segatools however uses the numbers that you see in controller settings, that start at 1.
                                 * hence i need to increase the number.
                                 */
                              
                                string lemmefix = state.Offset.ToString().Replace("Buttons", "");
                                int buttonNo = int.Parse(lemmefix);
                                buttonNo++;
                                lemmefix = "Buttons" + buttonNo;
                                this.Dispatcher.Invoke(() => { try { selTextBox.Text = lemmefix; } catch { } });
                            }
                            else
                            {
                                this.Dispatcher.Invoke(() => { try { selTextBox.Text = state.Offset.ToString(); } catch { } });
                            }
                        }
                    }
                }
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
                //time to check if Rotation is used
                if (_gameProfile.brakeAxis.Contains("R"))
                {
                    controls[0].Text = _gameProfile.brakeAxis.Replace("R", "Rotation");
                }
                else
                {
                    controls[0].Text = _gameProfile.brakeAxis + " Axis";
                }
                if (controls[1].Text.Contains("R"))
                {
                    controls[1].Text = _gameProfile.accelAxis.Replace("R", "Rotation");
                }
                else
                {
                    controls[1].Text = _gameProfile.accelAxis + " Axis";
                }

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
                reverseAccAxis.IsChecked = _gameProfile.reverseAccelAxis;
                reverseBrakeAxis.IsChecked = _gameProfile.reverseBrakeAxis;
                textBoxDevice.Text = _gameProfile.devName;
                textBoxShifter.Text = _gameProfile.shifterName;
            }
            catch
            {

            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            List<TextBox> controls = GetAllTextBox();

            //time to check if Rotation is used
            if (controls[0].Text.Contains("Rotation"))
            {
                controls[0].Text = controls[0].Text.Replace("Rotation", "R");
            }
            if (controls[1].Text.Contains("Rotation"))
            {
                controls[1].Text = controls[1].Text.Replace("Rotation", "R");
            }

            if (controls[0].Text.Contains(" Axis"))
            {
                controls[0].Text = controls[0].Text.Replace(" Axis", "");
            }
            if (controls[1].Text.Contains(" Axis"))
            {
                controls[1].Text = controls[1].Text.Replace(" Axis", "");
            }

            //transfer settings to IDZConfig
            _gameProfile.brakeAxis = controls[0].Text;
            _gameProfile.accelAxis = controls[1].Text;
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
            _gameProfile.reverseAccelAxis = (bool)reverseAccAxis.IsChecked;
            _gameProfile.reverseBrakeAxis = (bool)reverseBrakeAxis.IsChecked;
            _gameProfile.devName = textBoxDevice.Text;
            _gameProfile.shifterName = textBoxShifter.Text;

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
