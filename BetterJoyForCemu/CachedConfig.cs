using System;
using System.Configuration;

namespace BetterJoyForCemu {
    /// <summary>
    /// Cached configuration values to avoid repeated parsing
    /// </summary>
    public static class CachedConfig {
        // Rumble settings
        public static readonly int LowFreqRumble = Int32.Parse(ConfigurationManager.AppSettings["LowFreqRumble"]);
        public static readonly int HighFreqRumble = Int32.Parse(ConfigurationManager.AppSettings["HighFreqRumble"]);
        public static readonly bool EnableRumble = Boolean.Parse(ConfigurationManager.AppSettings["EnableRumble"]);

        // Display settings
        public static readonly bool ShowAsXInput = Boolean.Parse(ConfigurationManager.AppSettings["ShowAsXInput"]);
        public static readonly bool ShowAsDS4 = Boolean.Parse(ConfigurationManager.AppSettings["ShowAsDS4"]);

        // Input settings
        public static readonly bool SwapAB = Boolean.Parse(ConfigurationManager.AppSettings["SwapAB"]);
        public static readonly bool SwapXY = Boolean.Parse(ConfigurationManager.AppSettings["SwapXY"]);
        public static readonly bool DragToggle = Boolean.Parse(ConfigurationManager.AppSettings["DragToggle"]);
        public static readonly bool N64Range = Boolean.Parse(ConfigurationManager.AppSettings["N64Range"]);

        // Stick settings
        public static readonly float StickScalingFactor = float.Parse(ConfigurationManager.AppSettings["StickScalingFactor"]);
        public static readonly float StickScalingFactor2 = float.Parse(ConfigurationManager.AppSettings["StickScalingFactor2"]);

        // Gyro settings
        public static readonly string GyroToJoyOrMouse = ConfigurationManager.AppSettings["GyroToJoyOrMouse"];
        public static readonly bool UseFilteredIMU = Boolean.Parse(ConfigurationManager.AppSettings["UseFilteredIMU"]);
        public static readonly int GyroMouseSensitivityX = Int32.Parse(ConfigurationManager.AppSettings["GyroMouseSensitivityX"]);
        public static readonly int GyroMouseSensitivityY = Int32.Parse(ConfigurationManager.AppSettings["GyroMouseSensitivityY"]);
        public static readonly float GyroStickSensitivityX = float.Parse(ConfigurationManager.AppSettings["GyroStickSensitivityX"]);
        public static readonly float GyroStickSensitivityY = float.Parse(ConfigurationManager.AppSettings["GyroStickSensitivityY"]);
        public static readonly float GyroStickReduction = float.Parse(ConfigurationManager.AppSettings["GyroStickReduction"]);
        public static readonly bool GyroHoldToggle = Boolean.Parse(ConfigurationManager.AppSettings["GyroHoldToggle"]);
        public static readonly bool GyroAnalogSliders = Boolean.Parse(ConfigurationManager.AppSettings["GyroAnalogSliders"]);
        public static readonly int GyroAnalogSensitivity = Int32.Parse(ConfigurationManager.AppSettings["GyroAnalogSensitivity"]);

        // Power settings
        public static readonly bool HomeLongPowerOff = Boolean.Parse(ConfigurationManager.AppSettings["HomeLongPowerOff"]);
        public static readonly long PowerOffInactivityMins = Int32.Parse(ConfigurationManager.AppSettings["PowerOffInactivity"]);

        // IMU settings
        public static readonly float AHRS_Beta = float.Parse(ConfigurationManager.AppSettings["AHRS_beta"]);

        // Debug
        public static readonly int DebugType = int.Parse(ConfigurationManager.AppSettings["DebugType"]);
    }
}
