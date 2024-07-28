using System;
using System.Runtime.InteropServices;

// inspired: https://geekhack.org/index.php?topic=41881.0 (FilterKeys Setter)


namespace DelayFix
{
    public class FilterKeys
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct FILTERKEYS
        {
            public int cbSize;
            public int dwFlags;
            public int iWaitMSec;
            public int iDelayMSec;
            public int iRepeatMSec;
            public int iBounceMSec;
        }

        public const int SPI_SETFILTERKEYS = 0x0033;
        public const int FKF_FILTERKEYSON = 0x00000001;
        public const int FKF_AVAILABLE = 0x00000002;
        public const int FKF_HOTKEYACTIVE = 0x00000004;

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SystemParametersInfo(
            uint uiAction, uint uiParam, ref FILTERKEYS pvParam, uint fWinIni);

        public static void SetFilterKeys(int iWaitMSec, int iDelayMSec, int iRepeatMSec, int iBounceMSec, int dwFlags)
        {
            FILTERKEYS filterKeys = new FILTERKEYS
            {
                cbSize = Marshal.SizeOf(typeof(FILTERKEYS)),
                dwFlags = dwFlags,
                iWaitMSec = iWaitMSec,
                iDelayMSec = iDelayMSec,
                iRepeatMSec = iRepeatMSec,
                iBounceMSec = iBounceMSec
            };

            bool result = SystemParametersInfo(SPI_SETFILTERKEYS, (uint)Marshal.SizeOf(typeof(FILTERKEYS)), ref filterKeys, 0);
        }
    }
}