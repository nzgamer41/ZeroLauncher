using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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
using Path = System.IO.Path;

namespace ZeroLauncher.SetupWizard
{
    /// <summary>
    /// Interaction logic for Intro.xaml
    /// </summary>
    public partial class GameSetup : UserControl
    {
        MainWindow_new mainWindow;
        public GameSetup()
        {
            InitializeComponent();
        }

        private void buttonBrowse_Click(object sender, RoutedEventArgs e)
        {
            bool check = false;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "InitialD0_DX11_Nu.exe|InitialD0_DX11_Nu.exe";
            if (openFileDialog.ShowDialog() == true)
            {
                textBoxLocation.Text = openFileDialog.FileName;
                check = checkGameFiles(openFileDialog.FileName);
            }
            if (check)
            {
                mainWindow.buttonNext.IsEnabled = true;
            }
            else
            {
                mainWindow.buttonNext.IsEnabled = false;
            }
        }

        private string calcMd5(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    byte[] bytearray = md5.ComputeHash(stream);
                    // Convert the byte array to hexadecimal string
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < bytearray.Length; i++)
                    {
                        sb.Append(bytearray[i].ToString("X2"));
                    }
                    return sb.ToString();
                }
            }
        }

        private bool checkGameFiles(string filename)
        {
            textBlockStatus.Text = "";
            /* MD5 LIST
             * SDDF v1.31 unhtp: 35c0777c23afd19cd41423e6adc92b00
             * SDDF v1.31 enc: 	 ba70fe2917d81bce46fe3e2fe78967e8
             * SDDF v2.01 unhtp: fd24303120aa5f44c52a074344fe1c3c
             * SDDF v2.11 unhtp (from SOWS): 
             * SDDF v2.12 enc (from SOWS): 6455f160b5e14ec9182afcc5b86a3ba1
             * As more versions appear, I'll update this list. This doesnt indicate anything hidden or secret.
             */
            bool answer;
            if (File.Exists(filename))
            {
                string exeMd5 = calcMd5(filename);
                exeMd5 = exeMd5.ToLower();

                switch (exeMd5)
                {
                    case "35c0777c23afd19cd41423e6adc92b00":
                        answer = true;
                        Console.WriteLine("Version of IDZ is v1.31, supported version!");
                        textBlockStatus.Text = "Initial D0 v1.31: UNENCRYPTED!\nYou are ready to go!";
                        break;
                    case "63b071c35242c035159de374e10caabe":
                        answer = true;
                        Console.WriteLine("Version of IDZ is v2.11, supported version!");
                        textBlockStatus.Text = "Initial D0 v2.11: UNENCRYPTED!\nYou are ready to go!";
                        break;
                    case "ba70fe2917d81bce46fe3e2fe78967e8":
                        answer = false;
                        Console.WriteLine("EXE is encrypted!");
                        textBlockStatus.Text = "Initial D0 v1.31: ENCRYPTED!\nYou need to find a copy of the EXE with the encryption removed, This Program cannot help you with that.";
                        break;
                    case "fd24303120aa5f44c52a074344fe1c3c":
                        answer = false;
                        Console.WriteLine("Unsupported private version!");
                        textBlockStatus.Text = "I'm not sure how you got this EXE, but it's not supported. You should probably use a private fork of a loader.\nIf you received this in error, contact nzgamer41!";
                        break;
                    case "6455f160b5e14ec9182afcc5b86a3ba1":
                        answer = false;
                        Console.WriteLine("Unsupported public version!");
                        Console.WriteLine("EXE is encrypted!");
                        textBlockStatus.Text = "Initial D0 v2.12: ENCRYPTED!\nDo you have a spare sows invite? Just kidding, but this dump is useless at the moment. The EXE is encrypted.";
                        break;
                    default:
                        answer = false;
                        Console.WriteLine("MD5 is " + exeMd5 + " and is not supported atm");
                        textBlockStatus.Text = "I don't think this is the right EXE. Check you've selected the right file.";
                        break;
                }

                if (answer)
                {
                    mainWindow.newConfig.AMFSDir = Path.GetDirectoryName(filename).Replace("package", "amfs");
                }
            }
            else
            {
                answer = false;
            }

            return answer;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            mainWindow = Window.GetWindow(this) as MainWindow_new;
            mainWindow.buttonNext.IsEnabled = false;
        }
    }
}
