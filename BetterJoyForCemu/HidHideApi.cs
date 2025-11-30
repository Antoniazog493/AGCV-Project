using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace BetterJoyForCemu {
    /// <summary>
    /// HidHide API interop - Modern replacement for HidGuardian
    /// https://github.com/nefarius/HidHide
    /// </summary>
    public static class HidHideApi {
        private const string HidHideControlDevicePath = "\\\\.\\HidHide";

        // Thread synchronization lock
        private static readonly object _lockObject = new object();

        // IOCTL codes for HidHide device control
        private const uint IOCTL_GET_WHITELIST = 0x80016000;
        private const uint IOCTL_SET_WHITELIST = 0x80016004;
        private const uint IOCTL_GET_BLACKLIST = 0x80016008;
        private const uint IOCTL_SET_BLACKLIST = 0x8001600C;
        private const uint IOCTL_GET_ACTIVE = 0x80016010;
        private const uint IOCTL_SET_ACTIVE = 0x80016014;

        // Windows API imports
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern IntPtr CreateFile(
            string lpFileName,
            uint dwDesiredAccess,
            uint dwShareMode,
            IntPtr lpSecurityAttributes,
            uint dwCreationDisposition,
            uint dwFlagsAndAttributes,
            IntPtr hTemplateFile);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool DeviceIoControl(
            IntPtr hDevice,
            uint dwIoControlCode,
            IntPtr lpInBuffer,
            uint nInBufferSize,
            IntPtr lpOutBuffer,
            uint nOutBufferSize,
            out uint lpBytesReturned,
            IntPtr lpOverlapped);

        private const uint GENERIC_READ = 0x80000000;
        private const uint GENERIC_WRITE = 0x40000000;
        private const uint FILE_SHARE_READ = 0x00000001;
        private const uint FILE_SHARE_WRITE = 0x00000002;
        private const uint OPEN_EXISTING = 3;
        private static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

        /// <summary>
        /// Check if HidHide driver is installed and available
        /// </summary>
        public static bool IsInstalled() {
            lock (_lockObject) {
                try {
                    IntPtr handle = CreateFile(
                        HidHideControlDevicePath,
                        GENERIC_READ,
                        FILE_SHARE_READ | FILE_SHARE_WRITE,
                        IntPtr.Zero,
                        OPEN_EXISTING,
                        0,
                        IntPtr.Zero);

                    if (handle != INVALID_HANDLE_VALUE) {
                        CloseHandle(handle);
                        return true;
                    }
                    return false;
                } catch {
                    return false;
                }
            }
        }

        /// <summary>
        /// Enable or disable HidHide globally
        /// </summary>
        public static bool SetActive(bool active) {
            lock (_lockObject) {
                IntPtr handle = OpenDevice();
                if (handle == INVALID_HANDLE_VALUE) return false;

                try {
                    byte activeValue = (byte)(active ? 1 : 0);
                    IntPtr buffer = Marshal.AllocHGlobal(1);
                    try {
                        Marshal.WriteByte(buffer, activeValue);

                        uint bytesReturned;
                        bool result = DeviceIoControl(
                            handle,
                            IOCTL_SET_ACTIVE,
                            buffer,
                            1,
                            IntPtr.Zero,
                            0,
                            out bytesReturned,
                            IntPtr.Zero);

                        return result;
                    } finally {
                        Marshal.FreeHGlobal(buffer);
                    }
                } catch {
                    return false;
                } finally {
                    CloseHandle(handle);
                }
            }
        }

        /// <summary>
        /// Check if HidHide is currently active
        /// </summary>
        public static bool IsActive() {
            lock (_lockObject) {
                IntPtr handle = OpenDevice();
                if (handle == INVALID_HANDLE_VALUE) return false;

                try {
                    IntPtr buffer = Marshal.AllocHGlobal(1);
                    try {
                        uint bytesReturned;
                        bool result = DeviceIoControl(
                            handle,
                            IOCTL_GET_ACTIVE,
                            IntPtr.Zero,
                            0,
                            buffer,
                            1,
                            out bytesReturned,
                            IntPtr.Zero);

                        if (result && bytesReturned == 1) {
                            return Marshal.ReadByte(buffer) != 0;
                        }
                        return false;
                    } finally {
                        Marshal.FreeHGlobal(buffer);
                    }
                } catch {
                    return false;
                } finally {
                    CloseHandle(handle);
                }
            }
        }

        /// <summary>
        /// Add application to whitelist (allow it to see hidden devices)
        /// </summary>
        public static bool AddApplicationToWhitelist(string applicationPath) {
            lock (_lockObject) {
                try {
                    var currentList = GetWhitelistInternal();
                    if (currentList.Contains(applicationPath, StringComparer.OrdinalIgnoreCase)) {
                        return true; // Already in whitelist
                    }

                    currentList.Add(applicationPath);
                    return SetWhitelistInternal(currentList);
                } catch {
                    return false;
                }
            }
        }

        /// <summary>
        /// Remove application from whitelist
        /// </summary>
        public static bool RemoveApplicationFromWhitelist(string applicationPath) {
            lock (_lockObject) {
                try {
                    var currentList = GetWhitelistInternal();
                    currentList.RemoveAll(x => x.Equals(applicationPath, StringComparison.OrdinalIgnoreCase));
                    return SetWhitelistInternal(currentList);
                } catch {
                    return false;
                }
            }
        }

        /// <summary>
        /// Get current application whitelist
        /// </summary>
        public static List<string> GetWhitelist() {
            lock (_lockObject) {
                return GetWhitelistInternal();
            }
        }

        private static List<string> GetWhitelistInternal() {
            IntPtr handle = OpenDevice();
            if (handle == INVALID_HANDLE_VALUE) return new List<string>();

            try {
                // First call to get required buffer size
                uint bytesReturned;
                DeviceIoControl(handle, IOCTL_GET_WHITELIST, IntPtr.Zero, 0, IntPtr.Zero, 0, out bytesReturned, IntPtr.Zero);

                if (bytesReturned == 0) return new List<string>();

                IntPtr buffer = Marshal.AllocHGlobal((int)bytesReturned);
                try {
                    if (DeviceIoControl(handle, IOCTL_GET_WHITELIST, IntPtr.Zero, 0, buffer, bytesReturned, out bytesReturned, IntPtr.Zero)) {
                        return ParseMultiString(buffer, (int)bytesReturned);
                    }
                    return new List<string>();
                } finally {
                    Marshal.FreeHGlobal(buffer);
                }
            } catch {
                return new List<string>();
            } finally {
                CloseHandle(handle);
            }
        }

        /// <summary>
        /// Set application whitelist
        /// </summary>
        private static bool SetWhitelistInternal(List<string> applications) {
            IntPtr handle = OpenDevice();
            if (handle == INVALID_HANDLE_VALUE) return false;

            try {
                byte[] multiString = CreateMultiString(applications);
                IntPtr buffer = Marshal.AllocHGlobal(multiString.Length);
                try {
                    Marshal.Copy(multiString, 0, buffer, multiString.Length);

                    uint bytesReturned;
                    return DeviceIoControl(
                        handle,
                        IOCTL_SET_WHITELIST,
                        buffer,
                        (uint)multiString.Length,
                        IntPtr.Zero,
                        0,
                        out bytesReturned,
                        IntPtr.Zero);
                } finally {
                    Marshal.FreeHGlobal(buffer);
                }
            } catch {
                return false;
            } finally {
                CloseHandle(handle);
            }
        }

        /// <summary>
        /// Add device instance path to blacklist (hide this device)
        /// </summary>
        public static bool AddDeviceToBlacklist(string deviceInstancePath) {
            lock (_lockObject) {
                try {
                    var currentList = GetBlacklistInternal();
                    if (currentList.Contains(deviceInstancePath, StringComparer.OrdinalIgnoreCase)) {
                        return true; // Already hidden
                    }

                    currentList.Add(deviceInstancePath);
                    return SetBlacklistInternal(currentList);
                } catch {
                    return false;
                }
            }
        }

        /// <summary>
        /// Remove device from blacklist (unhide)
        /// </summary>
        public static bool RemoveDeviceFromBlacklist(string deviceInstancePath) {
            lock (_lockObject) {
                try {
                    var currentList = GetBlacklistInternal();
                    currentList.RemoveAll(x => string.Equals(x, deviceInstancePath, StringComparison.OrdinalIgnoreCase));
                    return SetBlacklistInternal(currentList);
                } catch {
                    return false;
                }
            }
        }

        /// <summary>
        /// Get current device blacklist
        /// </summary>
        public static List<string> GetBlacklist() {
            lock (_lockObject) {
                return GetBlacklistInternal();
            }
        }

        private static List<string> GetBlacklistInternal() {
            IntPtr handle = OpenDevice();
            if (handle == INVALID_HANDLE_VALUE) return new List<string>();

            try {
                uint bytesReturned;
                DeviceIoControl(handle, IOCTL_GET_BLACKLIST, IntPtr.Zero, 0, IntPtr.Zero, 0, out bytesReturned, IntPtr.Zero);

                if (bytesReturned == 0) return new List<string>();

                IntPtr buffer = Marshal.AllocHGlobal((int)bytesReturned);
                try {
                    if (DeviceIoControl(handle, IOCTL_GET_BLACKLIST, IntPtr.Zero, 0, buffer, bytesReturned, out bytesReturned, IntPtr.Zero)) {
                        return ParseMultiString(buffer, (int)bytesReturned);
                    }
                    return new List<string>();
                } finally {
                    Marshal.FreeHGlobal(buffer);
                }
            } catch {
                return new List<string>();
            } finally {
                CloseHandle(handle);
            }
        }

        /// <summary>
        /// Set device blacklist
        /// </summary>
        private static bool SetBlacklistInternal(List<string> devices) {
            IntPtr handle = OpenDevice();
            if (handle == INVALID_HANDLE_VALUE) return false;

            try {
                byte[] multiString = CreateMultiString(devices);
                IntPtr buffer = Marshal.AllocHGlobal(multiString.Length);
                try {
                    Marshal.Copy(multiString, 0, buffer, multiString.Length);

                    uint bytesReturned;
                    return DeviceIoControl(
                        handle,
                        IOCTL_SET_BLACKLIST,
                        buffer,
                        (uint)multiString.Length,
                        IntPtr.Zero,
                        0,
                        out bytesReturned,
                        IntPtr.Zero);
                } finally {
                    Marshal.FreeHGlobal(buffer);
                }
            } catch {
                return false;
            } finally {
                CloseHandle(handle);
            }
        }

        /// <summary>
        /// Clear all blacklisted devices
        /// </summary>
        public static bool ClearBlacklist() {
            lock (_lockObject) {
                try {
                    return SetBlacklistInternal(new List<string>());
                } catch {
                    return false;
                }
            }
        }

        /// <summary>
        /// Clear application whitelist
        /// </summary>
        public static bool ClearWhitelist() {
            lock (_lockObject) {
                try {
                    return SetWhitelistInternal(new List<string>());
                } catch {
                    return false;
                }
            }
        }

        /// <summary>
        /// Verify that a device is actually hidden in the blacklist
        /// </summary>
        public static bool IsDeviceHidden(string deviceInstancePath) {
            lock (_lockObject) {
                try {
                    var blacklist = GetBlacklistInternal();
                    return blacklist.Any(x => string.Equals(x, deviceInstancePath, StringComparison.OrdinalIgnoreCase));
                } catch {
                    return false;
                }
            }
        }

        /// <summary>
        /// Add device to blacklist with retry logic and verification
        /// </summary>
        public static bool AddDeviceToBlacklistWithRetry(string deviceInstancePath, int maxRetries = 3) {
            lock (_lockObject) {
                if (string.IsNullOrEmpty(deviceInstancePath)) return false;

                for (int attempt = 0; attempt < maxRetries; attempt++) {
                    try {
                        // Try to add to blacklist
                        if (!AddDeviceToBlacklist(deviceInstancePath)) {
                            Thread.Sleep(100); // Wait before retry
                            continue;
                        }

                        // Verify it was actually added
                        Thread.Sleep(50); // Small delay for driver to process
                        if (IsDeviceHidden(deviceInstancePath)) {
                            return true;
                        }

                        Thread.Sleep(100); // Wait before retry
                    } catch {
                        if (attempt == maxRetries - 1) return false;
                        Thread.Sleep(100);
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Remove device from blacklist with retry logic
        /// </summary>
        public static bool RemoveDeviceFromBlacklistWithRetry(string deviceInstancePath, int maxRetries = 3) {
            lock (_lockObject) {
                if (string.IsNullOrEmpty(deviceInstancePath)) return false;

                for (int attempt = 0; attempt < maxRetries; attempt++) {
                    try {
                        // Check if device is even in the blacklist
                        if (!IsDeviceHidden(deviceInstancePath)) {
                            return true; // Already not hidden
                        }

                        // Try to remove from blacklist
                        if (!RemoveDeviceFromBlacklist(deviceInstancePath)) {
                            Thread.Sleep(100);
                            continue;
                        }

                        // Verify it was actually removed
                        Thread.Sleep(50);
                        if (!IsDeviceHidden(deviceInstancePath)) {
                            return true;
                        }

                        Thread.Sleep(100);
                    } catch {
                        if (attempt == maxRetries - 1) return false;
                        Thread.Sleep(100);
                    }
                }
                return false;
            }
        }

        // Helper methods
        private static IntPtr OpenDevice() {
            return CreateFile(
                HidHideControlDevicePath,
                GENERIC_READ | GENERIC_WRITE,
                FILE_SHARE_READ | FILE_SHARE_WRITE,
                IntPtr.Zero,
                OPEN_EXISTING,
                0,
                IntPtr.Zero);
        }

        private static List<string> ParseMultiString(IntPtr buffer, int bufferSize) {
            var result = new List<string>();
            if (bufferSize < 2) return result;

            byte[] data = new byte[bufferSize];
            Marshal.Copy(buffer, data, 0, bufferSize);

            int start = 0;
            for (int i = 0; i < bufferSize - 1; i += 2) {
                if (data[i] == 0 && data[i + 1] == 0) {
                    if (i > start) {
                        string str = Encoding.Unicode.GetString(data, start, i - start);
                        if (!string.IsNullOrEmpty(str)) {
                            result.Add(str);
                        }
                    }
                    start = i + 2;

                    // Double null terminator = end of multi-string
                    if (i + 3 < bufferSize && data[i + 2] == 0 && data[i + 3] == 0) {
                        break;
                    }
                }
            }

            return result;
        }

        private static byte[] CreateMultiString(List<string> strings) {
            if (strings == null || strings.Count == 0) {
                return new byte[] { 0, 0, 0, 0 }; // Empty multi-string
            }

            var result = new List<byte>();
            foreach (var str in strings) {
                if (!string.IsNullOrEmpty(str)) {
                    result.AddRange(Encoding.Unicode.GetBytes(str));
                    result.Add(0);
                    result.Add(0); // Unicode null terminator
                }
            }

            // Add final double null terminator
            result.Add(0);
            result.Add(0);

            return result.ToArray();
        }

        /// <summary>
        /// Convert HID device path to device instance path for HidHide
        /// Example: "\\?\hid#vid_057e&pid_2009#6&2fd01ce5&0&0000#{4d1e55b2-f16f-11cf-88cb-001111000030}" 
        ///       -> "HID\VID_057E&PID_2009\6&2FD01CE5&0&0000"
        /// </summary>
        public static string ConvertHidPathToInstancePath(string hidPath) {
            if (string.IsNullOrEmpty(hidPath)) return null;

            try {
                // Remove the \\?\ prefix and convert to uppercase for consistency
                string path = hidPath.ToUpperInvariant().Replace("\\\\?\\", "");

                // Split by # to get parts
                // Format: hid#vid_057e&pid_2009#6&2fd01ce5&0&0000#{guid}
                string[] parts = path.Split('#');
                if (parts.Length < 3) return null;

                // parts[0] = "HID"
                // parts[1] = "VID_057E&PID_2009"
                // parts[2] = "6&2FD01CE5&0&0000"
                // parts[3] = "{guid}" (optional)

                // Reconstruct as HID\VID_057E&PID_2009\6&2FD01CE5&0&0000
                string instancePath = $"{parts[0]}\\{parts[1]}\\{parts[2]}";

                return instancePath;
            } catch {
                return null;
            }
        }
    }
}
