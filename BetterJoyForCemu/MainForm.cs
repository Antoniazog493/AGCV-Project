using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BetterJoyForCemu {
    public partial class MainForm : Form {
        public bool allowCalibration = Boolean.Parse(ConfigurationManager.AppSettings["AllowCalibration"]);
        public List<Button> con, loc;
        public bool calibrate;
        public List<KeyValuePair<string, float[]>> caliData;
        private Timer countDown;
        private int count;
        public List<int> xG, yG, zG, xA, yA, zA;
        public bool shakeInputEnabled = Boolean.Parse(ConfigurationManager.AppSettings["EnableShakeInput"]);
        public float shakeSesitivity = float.Parse(ConfigurationManager.AppSettings["ShakeInputSensitivity"]);
        public float shakeDelay = float.Parse(ConfigurationManager.AppSettings["ShakeInputDelay"]);

        // Settings que requieren reinicio de la aplicación
        private static readonly HashSet<string> RestartRequiredSettings = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "ShowAsXInput",
            "ShowAsDS4",
            "EnableRumble",
            "IP",
            "Port",
            "PurgeWhitelist",
            "PurgeAffectedDevices",
            "AllowCalibration",
            "UseHIDHide",
            "UseIncrementalLights",
            "AutoPowerOff",
            "DoNotRejoinJoycons",
            "EnableShakeInput",
            "ShakeInputSensitivity",
            "ShakeInputDelay"
        };

        // Track if changes require restart
        private bool pendingRestartRequired = false;
        
        // Track original values to detect changes
        private Dictionary<string, string> originalValues = new Dictionary<string, string>();
        

        public MainForm() {
            xG = new List<int>(); yG = new List<int>(); zG = new List<int>();
            xA = new List<int>(); yA = new List<int>(); zA = new List<int>();
            caliData = new List<KeyValuePair<string, float[]>> {
                new KeyValuePair<string, float[]>("0", new float[6] {0,0,0,-710,0,0})
            };

            InitializeComponent();

            if (!allowCalibration)
                AutoCalibrate.Hide();

            con = new List<Button> { con1, con2, con3, con4 };
            loc = new List<Button> { loc1, loc2, loc3, loc4 };

            //list all options
            string[] myConfigs = ConfigurationManager.AppSettings.AllKeys;
            Size childSize = new Size(150, 20);
            for (int i = 0; i != myConfigs.Length; i++) {
                settingsTable.RowCount++;

                // Store original values
                var value = ConfigurationManager.AppSettings[myConfigs[i]];
                originalValues[myConfigs[i]] = value;
                
                // Add setting name label with color coding and spaces for readability
                string displayName = AddSpacesToCamelCase(myConfigs[i]);
                
                Label settingLabel = new Label() {
                    Text = displayName,
                    TextAlign = ContentAlignment.BottomLeft,
                    AutoEllipsis = true,
                    Size = childSize,
                    Tag = myConfigs[i] // Store original key name in Tag for later retrieval
                };

                // Color code settings that require restart
                if (RestartRequiredSettings.Contains(myConfigs[i])) {
                    settingLabel.ForeColor = System.Drawing.Color.DarkOrange;
                }

                settingsTable.Controls.Add(settingLabel, 0, i);

                Control childControl;
                if (value == "true" || value == "false") {
                    childControl = new CheckBox() { Checked = Boolean.Parse(value), Size = childSize };
                } else {
                    childControl = new TextBox() { Text = value, Size = childSize };
                }

                childControl.MouseClick += cbBox_Changed;
                settingsTable.Controls.Add(childControl, 1, i);
            }

            // Update button text initially
            UpdateApplyButtonState();
        }

        // Helper method to add spaces to CamelCase strings for better readability
        private string AddSpacesToCamelCase(string text) {
            if (string.IsNullOrWhiteSpace(text))
                return text;
            
            System.Text.StringBuilder newText = new System.Text.StringBuilder(text.Length * 2);
            newText.Append(text[0]);
            
            for (int i = 1; i < text.Length; i++) {
                char current = text[i];
                char previous = text[i - 1];
                
                // Add space before uppercase letters (but not if previous was also uppercase, or if it's a number following a letter)
                if (char.IsUpper(current) && !char.IsUpper(previous)) {
                    newText.Append(' ');
                }
                // Add space before numbers if previous was a letter
                else if (char.IsDigit(current) && char.IsLetter(previous)) {
                    newText.Append(' ');
                }
                
                newText.Append(current);
            }
            
            return newText.ToString();
        }

        private void UpdateApplyButtonState() {
            if (pendingRestartRequired) {
                settingsApply.Text = "Apply && Restart";
                settingsApply.BackColor = System.Drawing.Color.LightCoral;
            } else {
                settingsApply.Text = "Apply";
                settingsApply.BackColor = System.Drawing.SystemColors.Control;
            }
        }

        private void HideToTray() {
            this.WindowState = FormWindowState.Minimized;
            notifyIcon.Visible = true;
            notifyIcon.BalloonTipText = "Double click the tray icon to maximise!";
            notifyIcon.ShowBalloonTip(0);
            this.ShowInTaskbar = false;
            this.Hide();
        }

        private void ShowFromTray() {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.Icon = Properties.Resources.betterjoyforcemu_icon;
            notifyIcon.Visible = false;
        }

        private void MainForm_Resize(object sender, EventArgs e) {
            if (this.WindowState == FormWindowState.Minimized) {
                HideToTray();
            }
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e) {
            ShowFromTray();
        }

        private void MainForm_Load(object sender, EventArgs e) {
            Config.Init(caliData);

            Program.Start();

            passiveScanBox.Checked = Config.IntValue("ProgressiveScan") == 1;
            startInTrayBox.Checked = Config.IntValue("StartInTray") == 1;

            if (Config.IntValue("StartInTray") == 1) {
                HideToTray();
            } else {
                ShowFromTray();
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
            try {
                Program.Stop();
                Environment.Exit(0);
            } catch { }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) { // this does not work, for some reason. Fix before release
            try {
                Program.Stop();
                Close();
                Environment.Exit(0);
            } catch { }
        }

        private void passiveScanBox_CheckedChanged(object sender, EventArgs e) {
            Config.SetValue("ProgressiveScan", passiveScanBox.Checked ? "1" : "0");
            Config.Save();
        }

        public void AppendTextBox(string value) { // https://stackoverflow.com/questions/519233/writing-to-a-textbox-from-another-thread
            if (InvokeRequired) {
                this.Invoke(new Action<string>(AppendTextBox), new object[] { value });
                return;
            }
            console.AppendText(value);
        }

        bool toRumble = Boolean.Parse(ConfigurationManager.AppSettings["EnableRumble"]);
        bool showAsXInput = Boolean.Parse(ConfigurationManager.AppSettings["ShowAsXInput"]);
        bool showAsDS4 = Boolean.Parse(ConfigurationManager.AppSettings["ShowAsDS4"]);

        public async void locBtnClickAsync(object sender, EventArgs e) {
            Button bb = sender as Button;

            if (bb.Tag.GetType() == typeof(Button)) {
                Button button = bb.Tag as Button;

                if (button.Tag.GetType() == typeof(Joycon)) {
                    Joycon v = (Joycon)button.Tag;
                    v.SetRumble(160.0f, 320.0f, 1.0f);
                    await Task.Delay(300);
                    v.SetRumble(160.0f, 320.0f, 0);
                }
            }
        }

        bool doNotRejoin = Boolean.Parse(ConfigurationManager.AppSettings["DoNotRejoinJoycons"]);

        public void conBtnClick(object sender, EventArgs e) {
            Button button = sender as Button;

            if (button.Tag.GetType() == typeof(Joycon)) {
                Joycon v = (Joycon)button.Tag;

                if (v.other == null && !v.isPro) { // needs connecting to other joycon (so messy omg)
                    bool succ = false;

                    if (Program.mgr.j.Count == 1 || doNotRejoin) { // when want to have a single joycon in vertical mode
                        v.other = v; // hacky; implement check in Joycon.cs to account for this
                        succ = true;
                    } else {
                        foreach (Joycon jc in Program.mgr.j) {
                            if (!jc.isPro && jc.isLeft != v.isLeft && jc != v && jc.other == null) {
                                v.other = jc;
                                jc.other = v;

                                if (v.out_xbox != null) {
                                    v.out_xbox.Disconnect();
                                    v.out_xbox = null;
                                }

                                if (v.out_ds4 != null) {
                                    v.out_ds4.Disconnect();
                                    v.out_ds4 = null;
                                }

                                // setting the other joycon's button image
                                foreach (Button b in con)
                                    if (b.Tag == jc)
                                        b.BackgroundImage = jc.isLeft ? Properties.Resources.jc_left : Properties.Resources.jc_right;

                                succ = true;
                                break;
                            }
                        }
                    }

                    if (succ)
                        foreach (Button b in con)
                            if (b.Tag == v)
                                b.BackgroundImage = v.isLeft ? Properties.Resources.jc_left : Properties.Resources.jc_right;
                } else if (v.other != null && !v.isPro) { // needs disconnecting from other joycon
                    ReenableViGEm(v);
                    ReenableViGEm(v.other);

                    button.BackgroundImage = v.isLeft ? Properties.Resources.jc_left_s : Properties.Resources.jc_right_s;

                    foreach (Button b in con)
                        if (b.Tag == v.other)
                            b.BackgroundImage = v.other.isLeft ? Properties.Resources.jc_left_s : Properties.Resources.jc_right_s;

                    v.other.other = null;
                    v.other = null;
                }
            }
        }

        private void startInTrayBox_CheckedChanged(object sender, EventArgs e) {
            Config.SetValue("StartInTray", startInTrayBox.Checked ? "1" : "0");
            Config.Save();
        }

        private void btn_open3rdP_Click(object sender, EventArgs e) {
            _3rdPartyControllers partyForm = new _3rdPartyControllers();
            partyForm.ShowDialog();
        }

        private void settingsApply_Click(object sender, EventArgs e) {
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;

            bool hasChanges = false;
            bool needsRestart = false;

            for (int row = 0; row < settingsTable.RowCount; row++) {
                var valCtl = settingsTable.GetControlFromPosition(1, row);
                var labelCtl = settingsTable.GetControlFromPosition(0, row);
                if (labelCtl == null || valCtl == null) continue;

                // Use Tag to get the original config key name instead of the display text
                var KeyCtl = labelCtl.Tag?.ToString() ?? labelCtl.Text;

                if (settings[KeyCtl] == null) continue;
                if (!originalValues.ContainsKey(KeyCtl)) continue;

                string newValue = "";
                if (valCtl.GetType() == typeof(CheckBox)) {
                    newValue = ((CheckBox)valCtl).Checked.ToString().ToLower();
                } else if (valCtl.GetType() == typeof(TextBox)) {
                    newValue = ((TextBox)valCtl).Text.ToLower();
                }

                // Check if value actually changed from original
                if (originalValues[KeyCtl] != newValue) {
                    settings[KeyCtl].Value = newValue;
                    hasChanges = true;

                    // Check if this setting requires restart
                    if (RestartRequiredSettings.Contains(KeyCtl)) {
                        needsRestart = true;
                    }
                }
            }

            if (!hasChanges) {
                AppendTextBox("No changes to apply.\r\n");
                return;
            }

            try {
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
                AppendTextBox("Settings saved successfully.\r\n");
                
                // Update original values to reflect saved state
                for (int row = 0; row < settingsTable.RowCount; row++) {
                    var valCtl = settingsTable.GetControlFromPosition(1, row);
                    var labelCtl = settingsTable.GetControlFromPosition(0, row);
                    if (labelCtl == null || valCtl == null) continue;

                    // Use Tag to get the original config key name
                    var KeyCtl = labelCtl.Tag?.ToString() ?? labelCtl.Text;
                    
                    string newValue = "";
                    if (valCtl.GetType() == typeof(CheckBox)) {
                        newValue = ((CheckBox)valCtl).Checked.ToString().ToLower();
                    } else if (valCtl.GetType() == typeof(TextBox)) {
                        newValue = ((TextBox)valCtl).Text.ToLower();
                    }
                    
                    originalValues[KeyCtl] = newValue;
                }
                
            } catch (ConfigurationErrorsException) {
                AppendTextBox("Error writing app settings.\r\n");
                return;
            }

            if (needsRestart) {
                // Settings that require restart were changed
                DialogResult result = MessageBox.Show(
                    "Some settings require a restart to take effect.\n\nDo you want to restart AGCV now?",
                    "Restart Required",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes) {
                    AppendTextBox("Restarting application...\r\n");
                    Application.DoEvents(); // Ensure message is shown

                    try {
                        // Prevent joycons poweroff when restarting
                        var tempConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                        if (tempConfig.AppSettings.Settings["AutoPowerOff"] != null) {
                            tempConfig.AppSettings.Settings["AutoPowerOff"].Value = "false";
                            tempConfig.Save(ConfigurationSaveMode.Modified);
                            ConfigurationManager.RefreshSection("appSettings");
                        }

                        // Get the current process path
                        string exePath = Application.ExecutablePath;
                        
                        AppendTextBox($"Starting new instance: {exePath}\r\n");
                        
                        // Start new process with a delay
                        ProcessStartInfo startInfo = new ProcessStartInfo {
                            FileName = "cmd.exe",
                            Arguments = $"/c timeout /t 2 /nobreak > nul && start \"\" \"{exePath}\"",
                            UseShellExecute = false,
                            CreateNoWindow = true,
                            WorkingDirectory = System.IO.Path.GetDirectoryName(exePath)
                        };
                        
                        Process.Start(startInfo);
                        
                        // Close current application immediately
                        Application.Exit();
                    } catch (Exception ex) {
                        AppendTextBox($"Error restarting: {ex.Message}\r\n");
                        AppendTextBox($"Stack trace: {ex.StackTrace}\r\n");
                        MessageBox.Show(
                            $"Could not restart automatically: {ex.Message}\n\nPlease restart AGCV manually.",
                            "Restart Failed",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        );
                    }
                } else {
                    AppendTextBox("Settings saved. Please restart AGCV manually for changes to take effect.\r\n");
                    pendingRestartRequired = true;
                    UpdateApplyButtonState();
                }
            } else {
                // Only instant-apply settings changed
                AppendTextBox("Settings applied successfully (no restart needed).\r\n");
                pendingRestartRequired = false;
                UpdateApplyButtonState();

                // Apply instant settings immediately
                ApplyInstantSettings();
            }
        }

        private void ApplyInstantSettings() {
            // Apply settings that don't require restart
            
            AppendTextBox("Applying instant settings...\r\n");

            // Refresh the configuration to get the latest saved values
            ConfigurationManager.RefreshSection("appSettings");

            // HomeLEDOn
            try {
                if (ConfigurationManager.AppSettings["HomeLEDOn"] != null) {
                    bool on = ConfigurationManager.AppSettings["HomeLEDOn"].ToLower() == "true";
                    AppendTextBox($"Setting HomeLED to: {on}\r\n");
                    
                    if (Program.mgr != null && Program.mgr.j != null) {
                        foreach (Joycon j in Program.mgr.j) {
                            try {
                                j.SetHomeLight(on);
                                AppendTextBox($"HomeLED applied to controller {j.PadId}\r\n");
                            } catch (Exception ex) {
                                AppendTextBox($"Error setting HomeLED for controller {j.PadId}: {ex.Message}\r\n");
                            }
                        }
                    } else {
                        AppendTextBox("No controllers connected to apply HomeLED.\r\n");
                    }
                }
            } catch (Exception ex) {
                AppendTextBox($"Error applying HomeLEDOn: {ex.Message}\r\n");
            }

            // Reload shake settings if they're dynamic (if not in RestartRequiredSettings)
            if (!RestartRequiredSettings.Contains("EnableShakeInput")) {
                try {
                    shakeInputEnabled = Boolean.Parse(ConfigurationManager.AppSettings["EnableShakeInput"]);
                    AppendTextBox($"ShakeInput enabled: {shakeInputEnabled}\r\n");
                } catch (Exception ex) {
                    AppendTextBox($"Error applying EnableShakeInput: {ex.Message}\r\n");
                }
            }
            if (!RestartRequiredSettings.Contains("ShakeInputSensitivity")) {
                try {
                    shakeSesitivity = float.Parse(ConfigurationManager.AppSettings["ShakeInputSensitivity"]);
                    AppendTextBox($"ShakeSensitivity: {shakeSesitivity}\r\n");
                } catch (Exception ex) {
                    AppendTextBox($"Error applying ShakeInputSensitivity: {ex.Message}\r\n");
                }
            }
            if (!RestartRequiredSettings.Contains("ShakeInputDelay")) {
                try {
                    shakeDelay = float.Parse(ConfigurationManager.AppSettings["ShakeInputDelay"]);
                    AppendTextBox($"ShakeDelay: {shakeDelay}\r\n");
                } catch (Exception ex) {
                    AppendTextBox($"Error applying ShakeInputDelay: {ex.Message}\r\n");
                }
            }

            AppendTextBox("Instant settings applied successfully.\r\n");
        }

        void ReenableViGEm(Joycon v) {
            if (showAsXInput && v.out_xbox == null) {
                v.out_xbox = new Controller.OutputControllerXbox360();

                if (toRumble)
                    v.out_xbox.FeedbackReceived += v.ReceiveRumble;
                v.out_xbox.Connect();
            }

            if (showAsDS4 && v.out_ds4 == null) {
                v.out_ds4 = new Controller.OutputControllerDualShock4();

                if (toRumble)
                    v.out_ds4.FeedbackReceived += v.Ds4_FeedbackReceived;
                v.out_ds4.Connect();
            }
        }

        private void foldLbl_Click(object sender, EventArgs e) {
            rightPanel.Visible = !rightPanel.Visible;
            foldLbl.Text = rightPanel.Visible ? "<" : ">";
        }

        private void cbBox_Changed(object sender, EventArgs e) {
            var coord = settingsTable.GetPositionFromControl(sender as Control);

            var valCtl = settingsTable.GetControlFromPosition(coord.Column, coord.Row);
            var labelCtl = settingsTable.GetControlFromPosition(coord.Column - 1, coord.Row);
            if (labelCtl == null) return;

            // Use Tag to get the original config key name instead of the display text
            var KeyCtl = labelCtl.Tag?.ToString() ?? labelCtl.Text;

            try {
                string newValue = "";
                if (valCtl.GetType() == typeof(CheckBox)) {
                    newValue = ((CheckBox)valCtl).Checked.ToString().ToLower();
                } else if (valCtl.GetType() == typeof(TextBox)) {
                    newValue = ((TextBox)valCtl).Text.ToLower();
                }

                // Check if value changed from original
                if (originalValues.ContainsKey(KeyCtl) && originalValues[KeyCtl] != newValue) {
                    // Check if restart will be needed
                    if (RestartRequiredSettings.Contains(KeyCtl)) {
                        if (!pendingRestartRequired) {
                            pendingRestartRequired = true;
                            UpdateApplyButtonState();
                        }
                    }
                    
                    // Do NOT apply any settings automatically - user must click Apply button
                    
                } else if (originalValues.ContainsKey(KeyCtl) && originalValues[KeyCtl] == newValue) {
                    // Value was changed back to original - check if we still need restart
                    CheckIfRestartStillNeeded();
                }
            } catch (Exception ex) {
                AppendTextBox($"Error processing setting change: {ex.Message}\r\n");
                Trace.WriteLine(String.Format("rw {0}, column {1}, {2}, {3}", coord.Row, coord.Column, sender.GetType(), KeyCtl));
            }
        }

        private void CheckIfRestartStillNeeded() {
            // Check if any restart-required setting has been changed
            bool stillNeedsRestart = false;
            
            for (int row = 0; row < settingsTable.RowCount; row++) {
                var labelCtl = settingsTable.GetControlFromPosition(0, row);
                var valCtl = settingsTable.GetControlFromPosition(1, row);
                
                if (labelCtl == null || valCtl == null) continue;
                
                // Use Tag to get the original config key name instead of the display text
                string keyName = labelCtl.Tag?.ToString() ?? labelCtl.Text;
                
                if (!RestartRequiredSettings.Contains(keyName)) continue;
                if (!originalValues.ContainsKey(keyName)) continue;
                
                string currentValue = "";
                if (valCtl.GetType() == typeof(CheckBox)) {
                    currentValue = ((CheckBox)valCtl).Checked.ToString().ToLower();
                } else if (valCtl.GetType() == typeof(TextBox)) {
                    currentValue = ((TextBox)valCtl).Text.ToLower();
                }
                
                if (originalValues[keyName] != currentValue) {
                    stillNeedsRestart = true;
                    break;
                }
            }
            
            if (pendingRestartRequired != stillNeedsRestart) {
                pendingRestartRequired = stillNeedsRestart;
                UpdateApplyButtonState();
            }
        }

        private void StartCalibrate(object sender, EventArgs e) {
            if (Program.mgr.j.Count == 0) {
                this.console.Text = "Please connect a single pro controller.";
                return;
            }
            if (Program.mgr.j.Count > 1) {
                this.console.Text = "Please calibrate one controller at a time (disconnect others).";
                return;
            }
            this.AutoCalibrate.Enabled = false;
            countDown = new Timer();
            this.count = 4;
            this.CountDown(null, null);
            countDown.Tick += new EventHandler(CountDown);
            countDown.Interval = 1000;
            countDown.Enabled = true;
        }

        private void StartGetData() {
            this.xG.Clear(); this.yG.Clear(); this.zG.Clear();
            this.xA.Clear(); this.yA.Clear(); this.zA.Clear();
            countDown = new Timer();
            this.count = 3;
            this.calibrate = true;
            countDown.Tick += new EventHandler(CalcData);
            countDown.Interval = 1000;
            countDown.Enabled = true;
        }

        private void btn_reassign_open_Click(object sender, EventArgs e) {
            Reassign mapForm = new Reassign();
            mapForm.ShowDialog();
        }

        private void CountDown(object sender, EventArgs e) {
            if (this.count == 0) {
                this.console.Text = "Calibrating...";
                countDown.Stop();
                this.StartGetData();
            } else {
                this.console.Text = "Plese keep the controller flat." + "\r\n";
                this.console.Text += "Calibration will start in " + this.count + " seconds.";
                this.count--;
            }
        }
        private void CalcData(object sender, EventArgs e) {
            if (this.count == 0) {
                countDown.Stop();
                this.calibrate = false;
                string serNum = Program.mgr.j.First().serial_number;
                int serIndex = this.findSer(serNum);
                float[] Arr = new float[6] { 0, 0, 0, 0, 0, 0 };
                if (serIndex == -1) {
                    this.caliData.Add(new KeyValuePair<string, float[]>(
                         serNum,
                         Arr
                    ));
                } else {
                    Arr = this.caliData[serIndex].Value;
                }
                Random rnd = new Random();
                Arr[0] = (float)quickselect_median(this.xG, rnd.Next);
                Arr[1] = (float)quickselect_median(this.yG, rnd.Next);
                Arr[2] = (float)quickselect_median(this.zG, rnd.Next);
                Arr[3] = (float)quickselect_median(this.xA, rnd.Next);
                Arr[4] = (float)quickselect_median(this.yA, rnd.Next);
                Arr[5] = (float)quickselect_median(this.zA, rnd.Next) - 4010; //Joycon.cs acc_sen 16384
                this.console.Text += "Calibration completed!!!" + "\r\n";
                Config.SaveCaliData(this.caliData);
                Program.mgr.j.First().getActiveData();
                this.AutoCalibrate.Enabled = true;
            } else {
                this.count--;
            }

        }
        private double quickselect_median(List<int> l, Func<int, int> pivot_fn) {
            int ll = l.Count;
            if (ll % 2 == 1) {
                return this.quickselect(l, ll / 2, pivot_fn);
            } else {
                return 0.5 * (quickselect(l, ll / 2 - 1, pivot_fn) + quickselect(l, ll / 2, pivot_fn));
            }
        }

        private int quickselect(List<int> l, int k, Func<int, int> pivot_fn) {
            if (l.Count == 1 && k == 0) {
                return l[0];
            }
            int pivot = l[pivot_fn(l.Count)];
            List<int> lows = l.Where(x => x < pivot).ToList();
            List<int> highs = l.Where(x => x > pivot).ToList();
            List<int> pivots = l.Where(x => x == pivot).ToList();
            if (k < lows.Count) {
                return quickselect(lows, k, pivot_fn);
            } else if (k < (lows.Count + pivots.Count)) {
                return pivots[0];
            } else {
                return quickselect(highs, k - lows.Count - pivots.Count, pivot_fn);
            }
        }

        public float[] activeCaliData(string serNum) {
            for (int i = 0; i < this.caliData.Count; i++) {
                if (this.caliData[i].Key == serNum) {
                    return this.caliData[i].Value;
                }
            }
            return this.caliData[0].Value;
        }

        private int findSer(string serNum) {
            for (int i = 0; i < this.caliData.Count; i++) {
                if (this.caliData[i].Key == serNum) {
                    return i;
                }
            }
            return -1;
        }

        private void version_lbl_Click(object sender, EventArgs e) {

        }

        private void label1_Click(object sender, EventArgs e) {

        }
    }
}
