using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ZeroLauncher
{
    [Serializable]
    public class IDZConfig
    {
        public string AMFSDir { get; set; }
        public bool JapOrExp { get; set; }
        public bool XOrDInput { get; set; }
        public string selectedNic { get; set; }
        public string selectedIP { get; set; }
        public bool IdealLan { get; set; }
        public bool DistServer { get; set; }
        public string brakeAxis { get; set; }
        public string accelAxis { get; set; }
        public string startButton { get; set; }
        public string viewChg { get; set; }
        public string shiftDn { get; set; }
        public string shiftUp { get; set; }
        public string gear1 { get; set; }
        public string gear2 { get; set; }
        public string gear3 { get; set; }
        public string gear4 { get; set; }
        public string gear5 { get; set; }
        public string gear6 { get; set; }
        public bool reverseAccelAxis { get; set; }
        public bool reverseBrakeAxis { get; set; }
        public string shifterName { get; set; }
        public string devName { get; set; }

        public int restriction { get; set; }

        public bool ImitateMe = false;
        public IDZConfig()
        {
        }
        public static IPAddress GetNetworkAddress(IPAddress address, IPAddress subnetMask)
        {
            byte[] ipAdressBytes = address.GetAddressBytes();
            byte[] subnetMaskBytes = subnetMask.GetAddressBytes();

            if (ipAdressBytes.Length != subnetMaskBytes.Length)
                throw new ArgumentException("Lengths of IP address and subnet mask do not match.");

            byte[] broadcastAddress = new byte[ipAdressBytes.Length];
            for (int i = 0; i < broadcastAddress.Length; i++)
            {
                broadcastAddress[i] = (byte)(ipAdressBytes[i] & (subnetMaskBytes[i]));
            }
            return new IPAddress(broadcastAddress);
        }
        public void ExportConfig()
        {
            //converts class data to segatools config file
            string fileOutput;
            fileOutput = "[vfs]\namfs=" + AMFSDir + "\nappdata=" +
                         AppDomain.CurrentDomain.BaseDirectory + "segaAppdata\n\n[dns]\ndefault=" +
                         selectedIP + "\n\n[ds]\nregion";
            if (JapOrExp)
            {
                fileOutput += "=4";
            }
            else
            {
                fileOutput += "=1";
            }

            fileOutput += "\n\n[netenv]";
            if (IdealLan)
            {
                fileOutput += "\nenable=1\n\n";
            }
            else
            {
                fileOutput += "\nenable=0\n\n";
            }
            IPAddress ip = IPAddress.Parse(selectedIP);
            fileOutput += "[keychip]\nsubnet=" + GetNetworkAddress(ip, IPAddress.Parse("255.255.255.0")) +
                          "\n\n[gpio]\ndipsw1=";
            if (DistServer)
            {
                fileOutput += "1\n\n";
            }
            else
            {
                fileOutput += "0\n\n";
            }

            fileOutput += "[io3]\nmode=";
            if (XOrDInput)
            {
                fileOutput += "xinput\nautoNeutral=1\nsingleStickSteering=1\nrestrict="+ restriction + "\n\n[dinput]\ndeviceName=\nshifterName=\nbrakeAxis=RZ\naccelAxis=Y\nstart=3\nviewChg=10\nshiftDn=1\nshiftUp=2\ngear1=1\ngear2=2\ngear3=3\ngear4=4\ngear5=5\ngear6=6\nreverseAccelAxis=0\nreverseBrakeAxis=0\n";
            }
            else
            {
                fileOutput += "dinput\nautoNeutral=1\nsingleStickSteering=1\nrestrict=" + restriction + "\n\n[dinput]\ndeviceName=";
                fileOutput += devName + "\nshifterName=" + shifterName + "\nbrakeAxis=" + brakeAxis + "\naccelAxis=" + accelAxis + "\nstart=" + startButton + "\nviewChg=" + viewChg + "\nshiftDn=" + shiftDn + "\nshiftUp=" + shiftUp + "\n";
                if(gear1 != "" && gear2 != "" && gear3 != "" && gear4 != "" && gear5 != "" && gear6 != "")
                {
                    fileOutput += "\ngear1="+ gear1 + "\ngear2=" + gear2 + "\ngear3=" + gear3 + "\ngear4=" + gear4 + "\ngear5=" + gear5 + "\ngear6=" + gear6;
                }
                else
                {
                    Debug.WriteLine("Not using shifter, or not all buttons mapped, sticking with defaults");
                    fileOutput += "\ngear1=1\ngear2=2\ngear3=3\ngear4=4\ngear5=5\ngear6=6";
                }
                fileOutput += "\nreverseAccelAxis=";
                if (reverseAccelAxis)
                {
                    fileOutput += "1";
                }
                else
                {
                    fileOutput += "0";
                }
                fileOutput += "\nreverseBrakeAxis=";
                if (reverseBrakeAxis)
                {
                    fileOutput += "1";
                }
                else
                {
                    fileOutput += "0";
                }
                fileOutput += "\n";

            }
                
            if (File.Exists(AMFSDir + "\\..\\app\\package\\segatools.ini"))
            {
                File.Delete(AMFSDir + "\\..\\app\\package\\segatools.ini");
            }
            File.WriteAllText((AMFSDir + "\\..\\app\\package\\segatools.ini"), fileOutput);
        }
    }
}
