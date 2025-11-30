using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using BetterJoyForCemu.Collections;
using Nefarius.ViGEm.Client;
using static BetterJoyForCemu._3rdPartyControllers;
using static BetterJoyForCemu.HIDapi;

namespace BetterJoyForCemu {
    // Forward declaration of Program class for JoyconManager to use
    class Program {
        public static PhysicalAddress btMAC = new PhysicalAddress(new byte[] { 0, 0, 0, 0, 0, 0 });
        public static UdpServer server;
        public static ViGEmClient emClient;
        public static JoyconManager mgr;
        static string pid;
        static MainForm form;
        static public bool useHidHide = false; // Will be auto-detected
        public static List<SController> thirdPartyCons = new List<SController>();
        private static WindowsInput.Events.Sources.IKeyboardEventSource keyboard;
        private static WindowsInput.Events.Sources.IMouseEventSource mouse;

        public static void Start() {
            pid = Process.GetCurrentProcess().Id.ToString(); // get current process id

            // Auto-detect and use HidHide if available
            if (HidHideApi.IsInstalled()) {
                form.console.AppendText("HidHide detected and will be used.\r\n");

                try {
                    // Add current process to whitelist but DON'T activate HidHide yet
                    string exePath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
                    if (HidHideApi.AddApplicationToWhitelist(exePath)) {
                        form.console.AppendText("Added AGCV to HidHide whitelist.\r\n");

                        // Verify we're actually in the whitelist
                        System.Threading.Thread.Sleep(100); // Small delay for driver to process
                        var whitelist = HidHideApi.GetWhitelist();
                        bool inWhitelist = whitelist.Any(x => x.Equals(exePath, StringComparison.OrdinalIgnoreCase));

                        if (inWhitelist) {
                            form.console.AppendText("Verified: AGCV is whitelisted.\r\n");
                            // DON'T activate HidHide here - it will be activated per-device
                            useHidHide = true;
                        } else {
                            form.console.AppendText("Warning: Could not verify whitelist. HidHide will not be used.\r\n");
                            useHidHide = false;
                        }
                    } else {
                        form.console.AppendText("Warning: Could not add AGCV to whitelist. HidHide will not be used.\r\n");
                        useHidHide = false;
                    }
                } catch (Exception ex) {
                    form.console.AppendText($"HidHide initialization error: {ex.Message}\r\n");
                    useHidHide = false;
                }
            } else {
                form.console.AppendText("HidHide not detected. Controllers will be visible to other applications.\r\n");
            }

            if (Boolean.Parse(ConfigurationManager.AppSettings["ShowAsXInput"]) || Boolean.Parse(ConfigurationManager.AppSettings["ShowAsDS4"])) {
                try {
                    emClient = new ViGEmClient(); // Manages emulated XInput
                } catch (Nefarius.ViGEm.Client.Exceptions.VigemBusNotFoundException) {
                    form.console.AppendText("Could not start VigemBus. Make sure drivers are installed correctly.\r\n");
                }
            }

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces()) {
                // Get local BT host MAC
                if (nic.NetworkInterfaceType != NetworkInterfaceType.FastEthernetFx && nic.NetworkInterfaceType != NetworkInterfaceType.Wireless80211) {
                    if (nic.Name.Split()[0] == "Bluetooth") {
                        btMAC = nic.GetPhysicalAddress();
                    }
                }
            }

            // a bit hacky
            _3rdPartyControllers partyForm = new _3rdPartyControllers();
            partyForm.CopyCustomControllers();

            mgr = new JoyconManager();
            mgr.form = form;
            mgr.Awake();
            mgr.CheckForNewControllers();
            mgr.Start();

            server = new UdpServer(mgr.j);
            server.form = form;

            server.Start(IPAddress.Parse(ConfigurationManager.AppSettings["IP"]), Int32.Parse(ConfigurationManager.AppSettings["Port"]));

            // Capture keyboard + mouse events for binding's sake
            keyboard = WindowsInput.Capture.Global.KeyboardAsync();
            keyboard.KeyEvent += Keyboard_KeyEvent;
            mouse = WindowsInput.Capture.Global.MouseAsync();
            mouse.MouseEvent += Mouse_MouseEvent;

            form.console.AppendText("All systems go\r\n");
        }

        private static void Mouse_MouseEvent(object sender, WindowsInput.Events.Sources.EventSourceEventArgs<WindowsInput.Events.Sources.MouseEvent> e) {
            if (e.Data.ButtonDown != null) {
                string res_val = Config.Value("reset_mouse");
                if (res_val.StartsWith("mse_"))
                    if ((int)e.Data.ButtonDown.Button == Int32.Parse(res_val.Substring(4)))
                        WindowsInput.Simulate.Events().MoveTo(Screen.PrimaryScreen.Bounds.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2).Invoke();

                res_val = Config.Value("active_gyro");
                if (res_val.StartsWith("mse_"))
                    if ((int)e.Data.ButtonDown.Button == Int32.Parse(res_val.Substring(4)))
                        // Use ToList() to avoid collection modification during enumeration
                        foreach (var i in mgr.j.ToList())
                            i.active_gyro = true;
            }

            if (e.Data.ButtonUp != null) {
                string res_val = Config.Value("active_gyro");
                if (res_val.StartsWith("mse_"))
                    if ((int)e.Data.ButtonUp.Button == Int32.Parse(res_val.Substring(4)))
                        // Use ToList() to avoid collection modification during enumeration
                        foreach (var i in mgr.j.ToList())
                            i.active_gyro = false;
            }
        }

        private static void Keyboard_KeyEvent(object sender, WindowsInput.Events.Sources.EventSourceEventArgs<WindowsInput.Events.Sources.KeyboardEvent> e) {
            if (e.Data.KeyDown != null) {
                string res_val = Config.Value("reset_mouse");
                if (res_val.StartsWith("key_"))
                    if ((int)e.Data.KeyDown.Key == Int32.Parse(res_val.Substring(4)))
                        WindowsInput.Simulate.Events().MoveTo(Screen.PrimaryScreen.Bounds.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2).Invoke();

                res_val = Config.Value("active_gyro");
                if (res_val.StartsWith("key_"))
                    if ((int)e.Data.KeyDown.Key == Int32.Parse(res_val.Substring(4)))
                        // Use ToList() to avoid collection modification during enumeration
                        foreach (var i in mgr.j.ToList())
                            i.active_gyro = true;
            }

            if (e.Data.KeyUp != null) {
                string res_val = Config.Value("active_gyro");
                if (res_val.StartsWith("key_"))
                    if ((int)e.Data.KeyUp.Key == Int32.Parse(res_val.Substring(4)))
                        // Use ToList() to avoid collection modification during enumeration
                        foreach (var i in mgr.j.ToList())
                            i.active_gyro = false;
            }
        }

        public static void Stop() {
            // Cleanup HidHide
            if (useHidHide) {
                try {
                    // Clear blacklist if configured
                    if (Boolean.Parse(ConfigurationManager.AppSettings["PurgeAffectedDevices"])) {
                        HidHideApi.ClearBlacklist();
                        form.console.AppendText("HidHide: Cleared device blacklist.\r\n");
                    }

                    // Deactivate HidHide
                    if (HidHideApi.IsActive()) {
                        HidHideApi.SetActive(false);
                        form.console.AppendText("HidHide deactivated.\r\n");
                    }

                    // Remove from whitelist
                    string exePath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
                    HidHideApi.RemoveApplicationFromWhitelist(exePath);
                    form.console.AppendText("Removed AGCV from HidHide whitelist.\r\n");
                } catch (Exception ex) {
                    form.console.AppendText($"HidHide cleanup error: {ex.Message}\r\n");
                }
            }

            keyboard.Dispose(); mouse.Dispose();
            server.Stop();
            mgr.OnApplicationQuit();
        }

        private static string appGuid = "1bf709e9-c133-41df-933a-c9ff3f664c7b"; // randomly-generated
        static void Main(string[] args) {

            // Setting the culturesettings so float gets parsed correctly
            CultureInfo.CurrentCulture = new CultureInfo("en-US", false);

            // Set the correct DLL for the current OS
            SetupDlls();

            using (Mutex mutex = new Mutex(false, "Global\\" + appGuid)) {
                if (!mutex.WaitOne(0, false)) {
                    MessageBox.Show("Instance already running.", "AGCV");
                    return;
                }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                form = new MainForm();
                Application.Run(form);
            }
        }

        static void SetupDlls() {
            string archPath = $"{AppDomain.CurrentDomain.BaseDirectory}{(Environment.Is64BitProcess ? "x64" : "x86")}\\";
            string pathVariable = Environment.GetEnvironmentVariable("PATH");
            pathVariable = $"{archPath};{pathVariable}";
            Environment.SetEnvironmentVariable("PATH", pathVariable);
        }

        // Helper funtions to set the hidapi dll location acording to the system instruction set.
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetDefaultDllDirectories(int directoryFlags);
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        static extern void AddDllDirectory(string lpPathName);
    }

    public class JoyconManager {
        public bool EnableIMU = true;
        public bool EnableLocalize = false;

        private const ushort vendor_id = 0x57e;
        private const ushort product_l = 0x2006;
        private const ushort product_r = 0x2007;
        private const ushort product_pro = 0x2009;
        private const ushort product_snes = 0x2017;
        private const ushort product_n64 = 0x2019;

        public ConcurrentList<Joycon> j { get; private set; } // Array of all connected Joy-Cons
        static JoyconManager instance;

        public MainForm form;

        System.Timers.Timer controllerCheck;

        // Track hidden devices for proper cleanup
        private static readonly System.Collections.Concurrent.ConcurrentDictionary<string, string> hiddenDevices =
            new System.Collections.Concurrent.ConcurrentDictionary<string, string>();

        // Lock to prevent timer re-entrancy
        private readonly object checkControllerLock = new object();

        public static JoyconManager Instance {
            get { return instance; }
        }

        public void Awake() {
            instance = this;
            j = new ConcurrentList<Joycon>();
            HIDapi.hid_init();
        }

        public void Start() {
            controllerCheck = new System.Timers.Timer(500); // check for new controllers every 500ms - faster detection
            controllerCheck.Elapsed += CheckForNewControllersTime;
            controllerCheck.Start();
        }

        bool ControllerAlreadyAdded(string path) {
            // Use ToList() to avoid enumeration issues
            foreach (Joycon v in j.ToList())
                if (v.path == path)
                    return true;
            return false;
        }

        void CleanUp() { // removes dropped controllers from list
            List<Joycon> rem = new List<Joycon>();
            // Use ToList() to create a snapshot and avoid collection modification during enumeration
            foreach (Joycon joycon in j.ToList()) {
                if (joycon.state == Joycon.state_.DROPPED) {
                    if (joycon.other != null)
                        joycon.other.other = null; // The other of the other is the joycon itself

                    // Unhide the device if it was hidden
                    if (Program.useHidHide && !string.IsNullOrEmpty(joycon.path)) {
                        if (hiddenDevices.TryRemove(joycon.path, out string instancePath)) {
                            try {
                                if (HidHideApi.RemoveDeviceFromBlacklistWithRetry(instancePath, 3)) {
                                    form.AppendTextBox($"Device unhidden: {instancePath}\r\n");
                                } else {
                                    form.AppendTextBox($"Warning: Could not unhide device: {instancePath}\r\n");
                                }
                            } catch (Exception ex) {
                                form.AppendTextBox($"Error unhiding device: {ex.Message}\r\n");
                            }
                        }
                    }

                    joycon.Detach(true);
                    rem.Add(joycon);

                    foreach (Button b in form.con) {
                        if (b.Enabled & b.Tag == joycon) {
                            try {
                                b.Invoke(new MethodInvoker(delegate {
                                    b.BackColor = System.Drawing.Color.FromArgb(0x00, System.Drawing.SystemColors.Control);
                                    b.Enabled = false;
                                    b.BackgroundImage = Properties.Resources.cross;
                                }));
                            } catch (Exception ex) {
                                form.AppendTextBox($"Error updating button UI: {ex.Message}\r\n");
                            }
                            break;
                        }
                    }

                    form.AppendTextBox("Removed dropped controller. Can be reconnected.\r\n");
                }
            }

            foreach (Joycon v in rem)
                j.Remove(v);

            // If no more devices are hidden, deactivate HidHide
            if (Program.useHidHide && hiddenDevices.Count == 0 && j.Count == 0) {
                try {
                    if (HidHideApi.IsActive()) {
                        HidHideApi.SetActive(false);
                        form.AppendTextBox("HidHide deactivated (no devices hidden).\r\n");
                    }
                } catch (Exception ex) {
                    form.AppendTextBox($"Error deactivating HidHide: {ex.Message}\r\n");
                }
            }
        }

        void CheckForNewControllersTime(Object source, ElapsedEventArgs e) {
            // Prevent timer re-entrancy - if already checking, skip this tick
            if (!Monitor.TryEnter(checkControllerLock)) {
                return;
            }

            try {
                CleanUp();
                if (Config.IntValue("ProgressiveScan") == 1) {
                    CheckForNewControllers();
                }
            } finally {
                Monitor.Exit(checkControllerLock);
            }
        }

        private ushort TypeToProdId(byte type) {
            switch (type) {
                case 1:
                    return product_pro;
                case 2:
                    return product_l;
                case 3:
                    return product_r;
            }
            return 0;
        }

        public void CheckForNewControllers() {
            // move all code for initializing devices here and well as the initial code from Start()
            bool isLeft = false;
            IntPtr ptr = HIDapi.hid_enumerate(0x0, 0x0);
            IntPtr top_ptr = ptr;

            hid_device_info enumerate; // Add device to list
            bool foundNew = false;
            List<string> devicesToHide = new List<string>(); // Collect devices to hide after opening all

            while (ptr != IntPtr.Zero) {
                SController thirdParty = null;
                enumerate = (hid_device_info)Marshal.PtrToStructure(ptr, typeof(hid_device_info));

                if (enumerate.serial_number == null) {
                    ptr = enumerate.next; // can't believe it took me this long to figure out why USB connections used up so much CPU.
                                          // it was getting stuck in an inf loop here!
                    continue;
                }

                bool validController = (enumerate.product_id == product_l || enumerate.product_id == product_r ||
                                        enumerate.product_id == product_pro || enumerate.product_id == product_snes || enumerate.product_id == product_n64) && enumerate.vendor_id == vendor_id;
                // check list of custom controllers specified
                foreach (SController v in Program.thirdPartyCons) {
                    if (enumerate.vendor_id == v.vendor_id && enumerate.product_id == v.product_id && enumerate.serial_number == v.serial_number) {
                        validController = true;
                        thirdParty = v;
                        break;
                    }
                }

                ushort prod_id = thirdParty == null ? enumerate.product_id : TypeToProdId(thirdParty.type);
                if (prod_id == 0) {
                    ptr = enumerate.next; // controller was not assigned a type, but advance ptr anyway
                    continue;
                }

                if (validController && !ControllerAlreadyAdded(enumerate.path)) {
                    switch (prod_id) {
                        case product_l:
                            isLeft = true;
                            form.AppendTextBox("Left Joy-Con connected.\r\n"); break;
                        case product_r:
                            isLeft = false;
                            form.AppendTextBox("Right Joy-Con connected.\r\n"); break;
                        case product_pro:
                            isLeft = true;
                            form.AppendTextBox("Pro controller connected.\r\n"); break;
                        case product_snes:
                            isLeft = true;
                            form.AppendTextBox("SNES controller connected.\r\n"); break;
                        case product_n64:
                            isLeft = true;
                            form.AppendTextBox("N64 controller connected.\r\n"); break;
                        default:
                            form.AppendTextBox("Non Joy-Con Nintendo input device skipped.\r\n"); break;
                    }

                    // Open the device FIRST before hiding it
                    IntPtr handle = HIDapi.hid_open_path(enumerate.path);
                    if (handle == IntPtr.Zero) {
                        form.AppendTextBox($"Failed to open device: {enumerate.path}\r\n");
                        ptr = enumerate.next;
                        continue;
                    }

                    HIDapi.hid_set_nonblocking(handle, 1);

                    // Add to list of devices to hide LATER (after all are opened)
                    if (Program.useHidHide) {
                        devicesToHide.Add(enumerate.path);
                    }

                    bool isPro = prod_id == product_pro;
                    bool isSnes = prod_id == product_snes;
                    bool is64 = prod_id == product_n64;
                    j.Add(new Joycon(handle, EnableIMU, EnableLocalize & EnableIMU, 0.05f, isLeft, enumerate.path, enumerate.serial_number, j.Count, isPro, isSnes, is64, thirdParty != null));

                    foundNew = true;
                    j.Last().form = form;

                    if (j.Count < 5) {
                        int ii = -1;
                        foreach (Button v in form.con) {
                            ii++;
                            if (!v.Enabled) {
                                System.Drawing.Bitmap temp;
                                switch (prod_id) {
                                    case (product_l):
                                        temp = Properties.Resources.jc_left_s; break;
                                    case (product_r):
                                        temp = Properties.Resources.jc_right_s; break;
                                    case (product_pro):
                                        temp = Properties.Resources.pro; break;
                                    case (product_snes):
                                        temp = Properties.Resources.snes; break;
                                    case (product_n64):
                                        temp = Properties.Resources.ultra; break;
                                    default:
                                        temp = Properties.Resources.cross; break;
                                }

                                v.Invoke(new MethodInvoker(delegate {
                                    v.Tag = j.Last(); // assign controller to button
                                    v.Enabled = true;
                                    v.Click += new EventHandler(form.conBtnClick);
                                    v.BackgroundImage = temp;
                                }));

                                form.loc[ii].Invoke(new MethodInvoker(delegate {
                                    form.loc[ii].Tag = v;
                                    form.loc[ii].Click += new EventHandler(form.locBtnClickAsync);
                                }));

                                break;
                            }
                        }
                    }

                    byte[] mac = new byte[6];
                    try {
                        for (int n = 0; n < 6; n++)
                            mac[n] = byte.Parse(enumerate.serial_number.Substring(n * 2, 2), System.Globalization.NumberStyles.HexNumber);
                    } catch (Exception e) {
                        // could not parse mac address
                    }
                    j[j.Count - 1].PadMacAddress = new PhysicalAddress(mac);
                }

                ptr = enumerate.next;
            }

            HIDapi.hid_free_enumeration(top_ptr);

            // NOW hide all the devices we opened (after enumeration is complete)
            if (Program.useHidHide && devicesToHide.Count > 0) {
                try {
                    // Check if HidHide is active
                    bool wasActive = HidHideApi.IsActive();

                    // Activate HidHide if not already active
                    if (!wasActive) {
                        if (HidHideApi.SetActive(true)) {
                            form.AppendTextBox("HidHide activated.\r\n");
                            System.Threading.Thread.Sleep(50); // Small delay for driver
                        } else {
                            form.AppendTextBox("Warning: Could not activate HidHide.\r\n");
                        }
                    }

                    // Hide each device
                    foreach (string devicePath in devicesToHide) {
                        try {
                            string instancePath = HidHideApi.ConvertHidPathToInstancePath(devicePath);
                            if (!string.IsNullOrEmpty(instancePath)) {
                                form.AppendTextBox($"Attempting to hide device: {instancePath}\r\n");

                                if (HidHideApi.AddDeviceToBlacklistWithRetry(instancePath, 3)) {
                                    // Store the mapping for cleanup later
                                    hiddenDevices.TryAdd(devicePath, instancePath);
                                    form.AppendTextBox($"Device hidden successfully.\r\n");
                                } else {
                                    form.AppendTextBox($"Warning: Unable to hide device. Input may overlap with other applications.\r\n");
                                }
                            } else {
                                form.AppendTextBox($"Warning: Could not convert device path for hiding.\r\n");
                            }
                        } catch (Exception ex) {
                            form.AppendTextBox($"HidHide error: {ex.Message}\r\n");
                        }
                    }
                } catch (Exception ex) {
                    form.AppendTextBox($"Error during device hiding: {ex.Message}\r\n");
                }
            }

            if (foundNew) { // attempt to auto join-up joycons on connection
                Joycon temp = null;
                // Use ToList() to create a snapshot and avoid collection modification during enumeration
                foreach (Joycon v in j.ToList()) {
                    // Do not attach two controllers if they are either:
                    // - Not a Joycon
                    // - Already attached to another Joycon (that isn't itself)
                    if (v.isPro || (v.other != null && v.other != v)) {
                        continue;
                    }

                    // Otherwise, iterate through and find the Joycon with the lowest
                    // id that has not been attached already (Does not include self)
                    if (temp == null)
                        temp = v;
                    else if (temp.isLeft != v.isLeft && v.other == null) {
                        temp.other = v;
                        v.other = temp;

                        if (temp.out_xbox != null) {
                            try {
                                temp.out_xbox.Disconnect();
                            } catch (Exception e) {
                                // it wasn't connected in the first place, go figure
                            }
                        }
                        if (temp.out_ds4 != null) {
                            try {
                                temp.out_ds4.Disconnect();
                            } catch (Exception e) {
                                // it wasn't connected in the first place, go figure
                            }
                        }
                        temp.out_xbox = null;
                        temp.out_ds4 = null;

                        foreach (Button b in form.con)
                            if (b.Tag == v || b.Tag == temp) {
                                Joycon tt = (b.Tag == v) ? v : (b.Tag == temp) ? temp : v;
                                b.BackgroundImage = tt.isLeft ? Properties.Resources.jc_left : Properties.Resources.jc_right;
                            }

                        temp = null;    // repeat
                    }
                }
            }

            bool on = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).AppSettings.Settings["HomeLEDOn"].Value.ToLower() == "true";
            // Use ToList() to create a snapshot and avoid collection modification during enumeration
            foreach (Joycon jc in j.ToList()) { // Connect device straight away
                if (jc.state == Joycon.state_.NOT_ATTACHED) {
                    if (jc.out_xbox != null)
                        jc.out_xbox.Connect();
                    if (jc.out_ds4 != null)
                        jc.out_ds4.Connect();

                    try {
                        jc.Attach();
                    } catch (Exception e) {
                        jc.state = Joycon.state_.DROPPED;
                        continue;
                    }

                    jc.SetHomeLight(on);

                    jc.Begin();
                    if (form.allowCalibration) {
                        jc.getActiveData();
                    }
                }
            }
        }

        public void OnApplicationQuit() {
            // Use ToList() to create a snapshot and avoid collection modification during enumeration
            foreach (Joycon v in j.ToList()) {
                if (Boolean.Parse(ConfigurationManager.AppSettings["AutoPowerOff"]))
                    v.PowerOff();

                v.Detach();

                if (v.out_xbox != null) {
                    v.out_xbox.Disconnect();
                }

                if (v.out_ds4 != null) {
                    v.out_ds4.Disconnect();
                }
            }

            // Unhide all hidden devices and deactivate HidHide
            if (Program.useHidHide) {
                foreach (var kvp in hiddenDevices) {
                    try {
                        if (HidHideApi.RemoveDeviceFromBlacklistWithRetry(kvp.Value, 3)) {
                            form.AppendTextBox($"Unhidden device on exit: {kvp.Value}\r\n");
                        }
                    } catch (Exception ex) {
                        form.AppendTextBox($"Error unhiding device on exit: {ex.Message}\r\n");
                    }
                }
                hiddenDevices.Clear();

                // Deactivate HidHide on exit
                try {
                    if (HidHideApi.IsActive()) {
                        HidHideApi.SetActive(false);
                        form.AppendTextBox("HidHide deactivated on exit.\r\n");
                    }
                } catch (Exception ex) {
                    form.AppendTextBox($"Error deactivating HidHide: {ex.Message}\r\n");
                }
            }

            controllerCheck.Stop();
            HIDapi.hid_exit();
        }
    }
}
