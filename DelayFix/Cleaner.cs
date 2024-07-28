using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;


namespace DelayFix
{
    internal class Cleaner
    {

        public static string userpath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

        private void CleanDirectory(string path)
        {
            foreach (var file in Directory.GetFiles(path))
            {
                try { File.Delete(file); } catch { }
            }

            foreach (var dir in Directory.GetDirectories(path))
            {
                try
                {
                    CleanDirectory(dir);
                    Directory.Delete(dir, true);
                }
                catch { }
            }
        }

        public void CleanRegedit(string keypath)
        {

            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(keypath, RegistryKeyPermissionCheck.ReadWriteSubTree))
            {
                if (key != null)
                {
                    foreach (string valueName in key.GetValueNames())
                    {
                        key.DeleteValue(valueName);
                    }
                }
            }
        }

        public void CleanRegeditKey(string keypath)
        {

            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(keypath, RegistryKeyPermissionCheck.ReadWriteSubTree))
            {
                if (key != null)
                {
                    foreach (string subKeyName in key.GetSubKeyNames())
                    {
                        try
                        {
                            key.DeleteSubKeyTree(subKeyName);
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
            }
        }

        public void tempClean()
        {
            CleanDirectory(@"C:\Windows\Temp");
            CleanDirectory(userpath + @"\AppData\Local\Temp");
        }

        public void prefetchClean()
        {
            CleanDirectory(@"C:\Windows\Prefetch");
        }

        public void crashClean()
        {
            CleanDirectory(userpath + @"\AppData\Local\CrashReportClient\Saved\Logs");
            CleanDirectory(userpath + @"\AppData\Local\D3DSCache");
        }

        public void regExeClean()
        {
            CleanRegedit(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\FeatureUsage\AppSwitched");
            CleanRegedit(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\FeatureUsage\ShowJumpView");
            CleanRegedit(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Compatibility Assistant\Store");
            CleanRegedit(@"Software\Classes\Local Settings\Software\Microsoft\Windows\Shell\BagMRU");
            CleanRegeditKey(@"SOFTWARE\Microsoft\DirectInput");
        }

        public void winrarClean()
        {
            CleanRegedit(@"SOFTWARE\WinRAR\ArcHistory");
        }

        public void junkClean()
        {
            CleanRegedit(@"Software\Microsoft\Windows\CurrentVersion\Explorer\TypedPaths");
            CleanRegeditKey(@"SOFTWARE\Classes\Local Settings\Software\Microsoft\Windows\Shell\BagMRU");
        }

        public void journalDelete()
        {
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "/c fsutil usn deletejournal /d c: && fsutil usn deletejournal /d d:";
            process.StartInfo.CreateNoWindow = true;
            process.Start();
        }

        public void disableFirewall()
        {
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "/c netsh advfirewall set allprofiles state off";
            process.StartInfo.CreateNoWindow = true;
            process.Start();
        }

        public void eventlogClean()
        {
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "/c for /F \"tokens=*\" %1 in ('wevtutil.exe el') DO wevtutil.exe cl \"%1\"";
            process.StartInfo.CreateNoWindow = true;
            process.Start();
        }

        public void activitiesClean()
        {
            string baseDirectory = userpath + @"\AppData\Local\ConnectedDevicesPlatform";

            try
            {
                string[] subdirectories = Directory.GetDirectories(baseDirectory);

                string firstSubdirectory = subdirectories[0];

                string dbFilePath = Path.Combine(firstSubdirectory, "ActivitiesCache.db");

                if (File.Exists(dbFilePath))
                {

                    using (FileStream fs = new FileStream(dbFilePath, FileMode.Truncate, FileAccess.Write))
                    {
                    
                    }
;
                }

            }
            catch (Exception ex)
            {
 
            }
        }

    }
}
