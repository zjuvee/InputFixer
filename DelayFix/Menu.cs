using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Win32;
using Newtonsoft.Json;
using Siticone.Desktop.UI.AnimatorNS;

namespace DelayFix
{
    public partial class Menu : Form
    {
        // variables
        public string AutoRepeatDelay;
        public string AutoRepeatRate;
        public string BounceTime;
        public string DelayBeforeAcceptance;
        public string Flags;

        public string ActiveWindowTracking;
        public string Beep;
        public string DoubleClickHeight;
        public string DoubleClickSpeed;
        public string DoubleClickWidth;
        public string ExtendedSounds;
        public string MouseHoverHeight;
        public string MouseHoverTime;
        public string MouseHoverWidth;
        public string MouseSensitivity;
        public string MouseSpeed;
        public string MouseThreshold1;
        public string MouseThreshold2;
        public string MouseTrails;
        public string SmoothMouseXCurve;
        public string SmoothMouseYCurve;

        public static string userpath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        public static string configfolder = userpath + @"\AppData\Roaming\InputFix";
        public static string originalfile = "originalvalues.json";
        public static string mousefile = "originalmousevalues.json";
        public static string mousefile2 = "actualmousevalues.json";

        RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Accessibility\Keyboard Response\", true);
        RegistryKey keyMouse = Registry.CurrentUser.OpenSubKey(@"Control Panel\Mouse\", true);

        string logPath = Path.Combine(configfolder, "log.txt");

        public Menu()
        {
            InitializeComponent();
        }

        // keyboard
        public class JsonValues
        {
            public string RepeatDelay { get; set; }
            public string RepeatRate { get; set; }
            public string BounceTime { get; set; }
            public string Acceptance { get; set; }
            public string Flags { get; set; }
        }

        // mouse
        public class JsonValues2
        {
            public string WindowTracking { get; set; }
            public string Beep { get; set; }
            public string ClickHeight { get; set; }
            public string ClickSpeed { get; set; }
            public string ClickWidth { get; set; }
            public string ExtendedSounds { get; set; }
            public string HoverHeight { get; set; }
            public string HoverTime { get; set; }
            public string HoverWidth { get; set; }
            public string Sensitivity { get; set; }
            public string Speed { get; set; }
            public string Threshold1 { get; set; }
            public string Threshold2 { get; set; }
            public string Trails { get; set; }

        }

        // write mouse json
        private void writeMouseJson(string filepath, string value1, string value2, string value3, string value4, string value5, string value6, string value7, string value8, string value9, string value10, string value11, string value12, string value13, string value14)
        {

            string completepath = Path.Combine(configfolder, filepath);

            JsonValues2 originalvalues = new JsonValues2 { WindowTracking = value1, Beep = value2, ClickHeight = value3, ClickSpeed = value4, ClickWidth = value5, ExtendedSounds = value6, HoverHeight = value7, HoverTime = value8, HoverWidth = value9, Sensitivity = value10, Speed = value11, Threshold1 = value12, Threshold2 = value13, Trails = value14 };
            string jsonString = JsonConvert.SerializeObject(originalvalues, Formatting.Indented);

            File.WriteAllText(completepath, jsonString);
        }

        // write keyboard json
        private void writeJSON(string filepath, string value1, string value2, string value3, string value4, string value5)
        {

            string completepath = Path.Combine(configfolder, filepath);

            JsonValues originalvalues = new JsonValues { RepeatDelay = value1, RepeatRate = value2, BounceTime = value3, Acceptance = value4, Flags = value5 };
            string jsonString = JsonConvert.SerializeObject(originalvalues, Formatting.Indented);

            File.WriteAllText(completepath, jsonString);
        }

        
        // update the form regedit values
        private void refreshForm()
        {
            AutoRepeatDelay = key.GetValue("AutoRepeatDelay").ToString();
            AutoRepeatRate = key.GetValue("AutoRepeatRate").ToString();
            BounceTime = key.GetValue("BounceTime").ToString();
            DelayBeforeAcceptance = key.GetValue("DelayBeforeAcceptance").ToString();
            Flags = key.GetValue("Flags").ToString();

            delayBox.Text = AutoRepeatDelay;
            rateBox.Text = AutoRepeatRate;
            bounceBox.Text = BounceTime;
            acceptanceBox.Text = DelayBeforeAcceptance;
            flagsBox.Text = Flags;
        }


        // update the mouse boolean state and write the json, also compares with the original file to change the state
        private void refreshMouse()
        {
            string completePath = Path.Combine(configfolder + "\\" + mousefile);
            string completePath2 = Path.Combine(configfolder + "\\" + mousefile2);

            var expectedValues = new JsonValues2
            {
                WindowTracking = "0",
                Beep = "No",
                ClickHeight = "4",
                ClickSpeed = "500",
                ClickWidth = "4",
                ExtendedSounds = "No",
                HoverHeight = "4",
                HoverTime = "400",
                HoverWidth = "4",
                Sensitivity = "10",
                Speed = "0",
                Threshold1 = "0",
                Threshold2 = "0",
                Trails = "0",
            };

            ActiveWindowTracking = keyMouse.GetValue("ActiveWindowTracking").ToString();
            Beep = keyMouse.GetValue("Beep").ToString();
            DoubleClickHeight = keyMouse.GetValue("DoubleClickHeight").ToString();
            DoubleClickSpeed = keyMouse.GetValue("DoubleClickSpeed").ToString();
            DoubleClickWidth = keyMouse.GetValue("DoubleClickWidth").ToString();
            ExtendedSounds = keyMouse.GetValue("ExtendedSounds").ToString();
            MouseHoverHeight = keyMouse.GetValue("MouseHoverHeight").ToString();
            MouseHoverTime = keyMouse.GetValue("MouseHoverTime").ToString();
            MouseHoverWidth = keyMouse.GetValue("MouseHoverWidth").ToString();
            MouseSensitivity = keyMouse.GetValue("MouseSensitivity").ToString();
            MouseSpeed = keyMouse.GetValue("MouseSpeed").ToString();
            MouseThreshold1 = keyMouse.GetValue("MouseThreshold1").ToString();
            MouseThreshold2 = keyMouse.GetValue("MouseThreshold2").ToString();
            MouseTrails = keyMouse.GetValue("MouseTrails").ToString();

            writeMouseJson(mousefile2, ActiveWindowTracking, Beep, DoubleClickHeight, DoubleClickSpeed, DoubleClickWidth, ExtendedSounds, MouseHoverHeight, MouseHoverTime, MouseHoverWidth, MouseSensitivity, MouseSpeed, MouseThreshold1, MouseThreshold2, MouseTrails);

            if (File.Exists(completePath))
            {

                string jsonString = File.ReadAllText(completePath2);

                var currentValues = JsonConvert.DeserializeObject<JsonValues2>(jsonString);

                bool mouseSettingsAreCorrect =
                currentValues.WindowTracking == expectedValues.WindowTracking &&
                currentValues.Beep == expectedValues.Beep &&
                currentValues.ClickHeight == expectedValues.ClickHeight &&
                currentValues.ClickSpeed == expectedValues.ClickSpeed &&
                currentValues.ClickWidth == expectedValues.ClickWidth &&
                currentValues.ExtendedSounds == expectedValues.ExtendedSounds &&
                currentValues.HoverHeight == expectedValues.HoverHeight &&
                currentValues.HoverTime == expectedValues.HoverTime &&
                currentValues.HoverWidth == expectedValues.HoverWidth &&
                currentValues.Sensitivity == expectedValues.Sensitivity &&
                currentValues.Speed == expectedValues.Speed &&
                currentValues.Threshold1 == expectedValues.Threshold1 &&
                currentValues.Threshold2 == expectedValues.Threshold2 &&
                currentValues.Trails == expectedValues.Trails;

                if (mouseSettingsAreCorrect)
                {
                    mouseLabel.ForeColor = Color.Yellow;
                    mouseLabel.Text = "true";
                }
                else
                {
                    mouseLabel.ForeColor = Color.Red;
                    mouseLabel.Text = "false";
                }
            }
            else
            {
                writeMouseJson(mousefile, ActiveWindowTracking, Beep, DoubleClickHeight, DoubleClickSpeed, DoubleClickWidth, ExtendedSounds, MouseHoverHeight, MouseHoverTime, MouseHoverWidth, MouseSensitivity, MouseSpeed, MouseThreshold1, MouseThreshold2, MouseTrails);
                
            }

            string logContent = File.ReadAllText(logPath);

            if (logContent == "1")
            {
                accelLabel.ForeColor = Color.Yellow;
                accelLabel.Text = "true";
            }
            else
            {
                accelLabel.ForeColor = Color.Red;
                accelLabel.Text = "false";
            }

        }

        // initialize the form 
        private void Menu_Load(object sender, EventArgs e)
        {
            refreshForm();
            refreshMouse();


            // create the directory if doesnt exist
            if (!Directory.Exists(configfolder))
            {
                Directory.CreateDirectory(configfolder);
            }


            // create the original file if doesnt exist
            if (!File.Exists(configfolder + "\\" + originalfile))
            {
                writeJSON(originalfile, AutoRepeatDelay, AutoRepeatRate, BounceTime, DelayBeforeAcceptance, Flags);
            }

            // set custom values
            AutoRepeatDelay = key.GetValue("AutoRepeatDelay").ToString();
            AutoRepeatRate = key.GetValue("AutoRepeatRate").ToString();
            BounceTime = key.GetValue("BounceTime").ToString();
            DelayBeforeAcceptance = key.GetValue("DelayBeforeAcceptance").ToString();
            Flags = key.GetValue("Flags").ToString();

            setDelay.Text = AutoRepeatDelay; // iDelayMSec
            setRate.Text = AutoRepeatRate; // iRepeatMSec
            setBounce.Text = BounceTime; // iBounceMSec;
            setAcceptance.Text = DelayBeforeAcceptance; // cbSize
            setFlags.Text = Flags; // dwFlags

        }

        // timer for the keyboard refresh
        private void refreshFormTimer_Tick(object sender, EventArgs e)
        {
            refreshForm();
        }

        // low button settings
        private void lowButton_Click(object sender, EventArgs e)
        {   
            setDelay.Text = "100"; // iDelayMSec
            setRate.Text = "6"; // iRepeatMSec
            setBounce.Text = "0"; // iBounceMSec;
            setAcceptance.Text = "0"; // cbSize
            setFlags.Text = "122"; // dwFlags

            refreshForm();
        }

        // medium button settings
        private void mediumButton_Click(object sender, EventArgs e)
        {
            setDelay.Text = "130"; // iDelayMSec
            setRate.Text = "13"; // iRepeatMSec
            setBounce.Text = "0"; // iBounceMSec;
            setAcceptance.Text = "0"; // cbSize
            setFlags.Text = "122"; // dwFlags

            refreshForm();
        }

        //high button settings
        private void highButton_Click(object sender, EventArgs e)
        {
            setDelay.Text = "220"; // iDelayMSec
            setRate.Text = "16"; // iRepeatMSec
            setBounce.Text = "0"; // iBounceMSec;
            setAcceptance.Text = "0"; // cbSize
            setFlags.Text = "122"; // dwFlags

            refreshForm();
        }

        // apply the custom values
        private void applyButton_Click(object sender, EventArgs e)
        {
            int delay = int.Parse(setDelay.Text);
            int rate = int.Parse(setRate.Text);
            int bounce = int.Parse(setBounce.Text);
            int acceptance = int.Parse(setAcceptance.Text);
            int flags = int.Parse(setFlags.Text);

            key.SetValue("AutoRepeatDelay", delay);
            key.SetValue("AutoRepeatRate", rate);
            key.SetValue("BounceTime", bounce);
            key.SetValue("DelayBeforeAcceptance", acceptance);
            key.SetValue("Flags", flags);

            try
            {
                int iWaitMSec = acceptance;
                int iDelayMSec = delay;
                int iRepeatMSec = rate;
                int iBounceMSec = bounce;
                int dwFlags = FilterKeys.FKF_FILTERKEYSON | FilterKeys.FKF_AVAILABLE | FilterKeys.FKF_HOTKEYACTIVE;

                FilterKeys.SetFilterKeys(iWaitMSec, iDelayMSec, iRepeatMSec, iBounceMSec, dwFlags);
            }
            catch (Exception ex)
            {

            }

            refreshForm();
        }

        // set the original settings stored in the originalffile
        private void originalButton_Click(object sender, EventArgs e)
        {
            string completepath = Path.Combine(configfolder, originalfile);

            if (File.Exists(completepath))
            {
                string jsonString = File.ReadAllText(completepath);
                JsonValues originalvalues = JsonConvert.DeserializeObject<JsonValues>(jsonString);

                setDelay.Text = originalvalues.RepeatDelay; // iDelayMSec
                setRate.Text = originalvalues.RepeatRate; // iRepeatMSec
                setBounce.Text = originalvalues.BounceTime; // iBounceMSec;
                setAcceptance.Text = originalvalues.Acceptance; // cbSize
                setFlags.Text = originalvalues.Flags; // dwFlags

                refreshForm();
            }
        }

        // apply mouseinput settings
        private void mouseinputButton_Click(object sender, EventArgs e)
        {
            keyMouse.SetValue("ActiveWindowTracking", "0");
            keyMouse.SetValue("Beep", "No");
            keyMouse.SetValue("DoubleClickHeight", "4");
            keyMouse.SetValue("DoubleClickSpeed", "500");
            keyMouse.SetValue("DoubleClickWidth", "4");
            keyMouse.SetValue("ExtendedSounds", "No");
            keyMouse.SetValue("MouseHoverHeight", "4");
            keyMouse.SetValue("MouseHoverTime", "400");
            keyMouse.SetValue("MouseHoverWidth", "4");
            keyMouse.SetValue("MouseSensitivity", "10");
            keyMouse.SetValue("MouseSpeed", "0");
            keyMouse.SetValue("MouseThreshold1", "0");
            keyMouse.SetValue("MouseThreshold2", "0");
            keyMouse.SetValue("MouseTrails", "0");

            refreshMouse();
        }

        // set the original mouse input settings
        private void mouseinputOrgButton_Click(object sender, EventArgs e)
        {
            string completePath = Path.Combine(configfolder + "\\" + mousefile);

            if (File.Exists(completePath))
            {
                string jsonString = File.ReadAllText(completePath);
                JsonValues2 originalvalues = JsonConvert.DeserializeObject<JsonValues2>(jsonString);

                keyMouse.SetValue("ActiveWindowTracking", originalvalues.WindowTracking);
                keyMouse.SetValue("Beep", originalvalues.Beep);
                keyMouse.SetValue("DoubleClickHeight", originalvalues.ClickHeight);
                keyMouse.SetValue("DoubleClickSpeed", originalvalues.ClickSpeed);
                keyMouse.SetValue("DoubleClickWidth", originalvalues.ClickWidth);
                keyMouse.SetValue("ExtendedSounds", originalvalues.ExtendedSounds);
                keyMouse.SetValue("MouseHoverHeight", originalvalues.HoverHeight);
                keyMouse.SetValue("MouseHoverTime", originalvalues.HoverTime);
                keyMouse.SetValue("MouseHoverWidth", originalvalues.HoverWidth);
                keyMouse.SetValue("MouseSensitivity", originalvalues.Sensitivity);
                keyMouse.SetValue("MouseSpeed", originalvalues.Speed);
                keyMouse.SetValue("MouseThreshold1", originalvalues.Threshold1);
                keyMouse.SetValue("MouseThreshold2", originalvalues.Threshold2);
                keyMouse.SetValue("MouseTrails", originalvalues.Trails);

                refreshMouse();
            }
        }

        // remove the mouse acceleration
        private void accelcommands()
        {

            

            File.WriteAllText(logPath, "1");

            Process process = new Process();       
            process.StartInfo.FileName = "cmd.exe";  
            process.StartInfo.Arguments = "/c reg add \"HKEY_CURRENT_USER\\Control Panel\\Mouse\" /v SmoothMouseXCurve /t REG_BINARY /d 0000000000000000156e00000000400100000029dc03000000280000000000 /f && reg add \"HKEY_CURRENT_USER\\Control Panel\\Mouse\" /v SmoothMouseYCurve /t REG_BINARY /d 0000000000000000fd11000000002404000000fc12000000c0bb0100000000 /f";      
            process.StartInfo.CreateNoWindow = true;
            process.Start();

        }

        // lol
        private void accelButton_Click(object sender, EventArgs e)
        {
            accelcommands();
            refreshMouse();
        }

        // mark all the cleaner checkbox
        private void selectAllButton_Click(object sender, EventArgs e)
        {
            tempCheckbox.Checked = true;
            prefetchCheckbox.Checked = true;
            crashCheckbox.Checked = true;
            regexeCheckbox.Checked = true;
            winrarCheckbox.Checked = true;
            junkCheckbox.Checked = true;
            journalCheckbox.Checked = true;
            firewallCheckbox.Checked = true;
            activitiesCheckbox.Checked = true;
            eventvwrCheckbox.Checked = true;
        }

        // cleanup button
        private void deleteButton_Click(object sender, EventArgs e)
        {

            DialogResult dialogResult = MessageBox.Show("this cleanup can be AntiSS, keep that in mind if you don't want to take an unfair ban.", "WARNING!!!", MessageBoxButtons.YesNo);

            Cleaner cleaner = new Cleaner();
            if (dialogResult == DialogResult.Yes)
            {
                if (tempCheckbox.Checked)
                {
                    cleaner.tempClean();
                }

                if (prefetchCheckbox.Checked)
                {
                    cleaner.prefetchClean();
                }

                if (crashCheckbox.Checked)
                {
                    cleaner.crashClean();
                }

                if (regexeCheckbox.Checked)
                {
                    cleaner.regExeClean();
                }

                if (winrarCheckbox.Checked)
                {
                    cleaner.winrarClean();
                }

                if (junkCheckbox.Checked)
                {
                    cleaner.junkClean();
                }

                if (journalCheckbox.Checked)
                {
                    cleaner.journalDelete();
                }

                if (firewallCheckbox.Checked)
                {
                    cleaner.disableFirewall();
                }

                if (activitiesCheckbox.Checked)
                {
                    cleaner.activitiesClean();
                }

                if (eventvwrCheckbox.Checked)
                {
                    cleaner.eventlogClean();
                }
                MessageBox.Show("everything has been cleaned properly.", "WARNING!!!", MessageBoxButtons.OK);
            }
            else
            {

            }

        }

        // meh, resource and contact links
        private void githubLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/zjuvee");
        }

        private void youtubeLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.youtube.com/channel/UCFi9INSjdVB7LIgc6cXDPjQ");
        }

        private void keyPathLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://pastebin.com/5hBEVfiW");
        }

        private void originalKeyLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(configfolder + @"\originalvalues.json");
        }

        private void presetsLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://pastebin.com/Y84MrAkF");
        }

        private void mouseOrgValuesLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(configfolder + @"\originalmousevalues.json");
        }

        private void mouseActLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(configfolder + @"\actualmousevalues.json");
        }

        private void accelLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://pastebin.com/RBM3ZabY");
        }

        private void mouseKeyLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://pastebin.com/8GrZhJQB");
        }

        private void cleanerLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://pastebin.com/QmJ5Z2JK");
        }
    }
}
