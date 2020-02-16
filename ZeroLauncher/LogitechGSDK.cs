using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroLauncher
{
    public class LogitechGSDK
    {
        //LCD SDK
        public const int LOGI_LCD_COLOR_BUTTON_LEFT = (0x00000100);
        public const int LOGI_LCD_COLOR_BUTTON_RIGHT = (0x00000200);
        public const int LOGI_LCD_COLOR_BUTTON_OK = (0x00000400);
        public const int LOGI_LCD_COLOR_BUTTON_CANCEL = (0x00000800);
        public const int LOGI_LCD_COLOR_BUTTON_UP = (0x00001000);
        public const int LOGI_LCD_COLOR_BUTTON_DOWN = (0x00002000);
        public const int LOGI_LCD_COLOR_BUTTON_MENU = (0x00004000);
        public const int LOGI_LCD_MONO_BUTTON_0 = (0x00000001);
        public const int LOGI_LCD_MONO_BUTTON_1 = (0x00000002);
        public const int LOGI_LCD_MONO_BUTTON_2 = (0x00000004);
        public const int LOGI_LCD_MONO_BUTTON_3 = (0x00000008);
        public const int LOGI_LCD_MONO_WIDTH = 160;
        public const int LOGI_LCD_MONO_HEIGHT = 43;
        public const int LOGI_LCD_COLOR_WIDTH = 320;
        public const int LOGI_LCD_COLOR_HEIGHT = 240;
        public const int LOGI_LCD_TYPE_MONO = (0x00000001);
        public const int LOGI_LCD_TYPE_COLOR = (0x00000002);

        [DllImport("LogitechLcd", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool LogiLcdInit(String friendlyName, int lcdType);

        [DllImport("LogitechLcd", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool LogiLcdIsConnected(int lcdType);

        [DllImport("LogitechLcd", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool LogiLcdIsButtonPressed(int button);

        [DllImport("LogitechLcd", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern void LogiLcdUpdate();

        [DllImport("LogitechLcd", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern void LogiLcdShutdown();

        // Monochrome LCD functions
        [DllImport("LogitechLcd", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool LogiLcdMonoSetBackground(byte[] monoBitmap);

        [DllImport("LogitechLcd", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool LogiLcdMonoSetText(int lineNumber, String text);

        // Color LCD functions
        [DllImport("LogitechLcd", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool LogiLcdColorSetBackground(byte[] colorBitmap);

        [DllImport("LogitechLcd", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool LogiLcdColorSetTitle(String text, int red, int green, int blue);

        [DllImport("LogitechLcd", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool LogiLcdColorSetText(int lineNumber, String text, int red, int green, int blue);

        [DllImport("kernel32", SetLastError = true)]
        private static extern IntPtr LoadLibrary(string lpFileName);

        public static bool LibraryExists(string fileName)
        {
            return LoadLibrary(fileName) != IntPtr.Zero;
        }
    }
}

