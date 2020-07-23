using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ZeroLauncher.SetupWizard
{
    /// <summary>
    /// Interaction logic for Controller.xaml
    /// </summary>
    public partial class Prereqs : UserControl
    {
        bool downloadComplete = false;
        MainWindow_new mainWindow;
        public Prereqs()
        {
            InitializeComponent();

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            mainWindow = Window.GetWindow(this) as MainWindow_new;
            beginCheck();
        }

        private async void beginCheck()
        {
            /* FUCK MICROSOFT
             * Why the fuck is there no easy way to check for the DirectX Redistributables
             * 
             * ok real shit
             * step 1: "check" for directx
             * step 2: check for node (gonna use clever shit)
             * step 3: check for VC2012
             * step 4: install missing redists
             * 
             */
            //BEGIN DX
            textBlockStatus.Text = "Installing DirectX Redistributibles...";
            await Task.Run(() => downloadFile("https://download.microsoft.com/download/1/7/1/1718CCC4-6315-4D8E-9543-8E28A4E18C4C/dxwebsetup.exe"));
            Process p = new Process();
            ProcessStartInfo si = new ProcessStartInfo();
            si.FileName = "dxwebsetup.exe";
            //si.Arguments = "/Q";
            p.StartInfo = si;
            p.Start();
            await Task.Run(() => p.WaitForExit());
            textBlockStatus.Text += "COMPLETE\n";
            File.Delete("dxwebsetup.exe");
            //END DX

            //BEGIN NODE
            //fuck im a genius
            string testjs = "console.log(\"test\");";
            File.WriteAllText("test.js", testjs);

            var enviromentPath = System.Environment.GetEnvironmentVariable("PATH");

            Console.WriteLine(enviromentPath);
            var paths = enviromentPath.Split(';');
            var exePath = paths.Select(x => Path.Combine(x, "node.exe"))
                               .Where(x => File.Exists(x))
                               .FirstOrDefault();
            textBlockStatus.Text += "Checking for Node.js...";
            p = new Process();
            si = new ProcessStartInfo();
            si.FileName = exePath;
            si.Arguments = "test.js";
            si.RedirectStandardOutput = true;
            si.UseShellExecute = false;
            p.StartInfo = si;
            p.Start();
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            File.Delete("test.js");
            if (output == "testjh\n")
            {
                textBlockStatus.Text += "FOUND";
            }
            else
            {
                textBlockStatus.Text += "NOT FOUND, DOWNLOADING...";
                await Task.Run(() => downloadFile("https://nodejs.org/dist/v12.18.3/node-v12.18.3-x64.msi"));
                Process process = new Process();
                process.StartInfo.FileName = "msiexec";
                process.StartInfo.WorkingDirectory = @"C:\temp\";
                process.StartInfo.Arguments = "/quiet /i " + Directory.GetCurrentDirectory() + "\\node-v12.18.3-x64.msi";
                process.StartInfo.Verb = "runas";
                process.Start();
                await Task.Run(() => process.WaitForExit());
                textBlockStatus.Text += "COMPLETE\n";
                File.Delete("node-v12.18.3-x64.msi");
            }
        }


        void downloadFile(string fileToDownload)
        {
            using (WebClient wc = new WebClient())
            {
                this.Dispatcher.Invoke(() =>
                {
                    pbDl.IsIndeterminate = false;
                    pbDl.Value = 0;
                });
                downloadComplete = false;
                wc.DownloadProgressChanged += wc_DownloadProgressChanged;
                wc.DownloadFileCompleted += wc_DownloadFileCompleted;
                wc.DownloadFileAsync(
                    // Param1 = Link of file
                    new System.Uri(fileToDownload),
                    // Param2 = Path to save
                    Path.GetFileName(fileToDownload)
                );

                while (!downloadComplete)
                {
                    //wait
                }
            }
        }

        private void wc_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                downloadComplete = true;
                pbDl.IsIndeterminate = true;
            });            
        }

        void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                pbDl.Value = e.ProgressPercentage;
            });
        }


    }
}
