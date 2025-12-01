using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace AGCV
{
    public partial class ButtonTester : Form
    {
        // XInput API declarations
        [DllImport("xinput1_4.dll")]
        private static extern int XInputGetState(int dwUserIndex, ref XINPUT_STATE pState);

        [DllImport("xinput1_4.dll")]
        private static extern int XInputGetBatteryInformation(int dwUserIndex, byte devType, ref XINPUT_BATTERY_INFORMATION pBatteryInformation);

        [StructLayout(LayoutKind.Sequential)]
        private struct XINPUT_STATE
        {
            public uint dwPacketNumber;
            public XINPUT_GAMEPAD Gamepad;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct XINPUT_GAMEPAD
        {
            public ushort wButtons;
            public byte bLeftTrigger;
            public byte bRightTrigger;
            public short sThumbLX;
            public short sThumbLY;
            public short sThumbRX;
            public short sThumbRY;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct XINPUT_BATTERY_INFORMATION
        {
            public byte BatteryType;
            public byte BatteryLevel;
        }

        // Button flags
        private const ushort XINPUT_GAMEPAD_DPAD_UP = 0x0001;
        private const ushort XINPUT_GAMEPAD_DPAD_DOWN = 0x0002;
        private const ushort XINPUT_GAMEPAD_DPAD_LEFT = 0x0004;
        private const ushort XINPUT_GAMEPAD_DPAD_RIGHT = 0x0008;
        private const ushort XINPUT_GAMEPAD_START = 0x0010;
        private const ushort XINPUT_GAMEPAD_BACK = 0x0020;
        private const ushort XINPUT_GAMEPAD_LEFT_THUMB = 0x0040;
        private const ushort XINPUT_GAMEPAD_RIGHT_THUMB = 0x0080;
        private const ushort XINPUT_GAMEPAD_LEFT_SHOULDER = 0x0100;
        private const ushort XINPUT_GAMEPAD_RIGHT_SHOULDER = 0x0200;
        private const ushort XINPUT_GAMEPAD_A = 0x1000;
        private const ushort XINPUT_GAMEPAD_B = 0x2000;
        private const ushort XINPUT_GAMEPAD_X = 0x4000;
        private const ushort XINPUT_GAMEPAD_Y = 0x8000;

        private System.Windows.Forms.Timer updateTimer;
        private int controllerIndex = -1;
        private ushort previousButtons = 0;
        private byte previousLeftTrigger = 0;
        private byte previousRightTrigger = 0;
        private Stopwatch globalStopwatch;
        private Dictionary<string, Stopwatch> buttonPressTimers;
        private int eventCounter = 0;
        private const int MAX_LOG_LINES = 1000;

        // Estados para evitar mensajes repetidos
        private bool wasConnected = false;
        private bool hasLoggedDisconnection = false;
        private bool hasLoggedNoController = false;

        public ButtonTester()
        {
            InitializeComponent();
            globalStopwatch = Stopwatch.StartNew();
            buttonPressTimers = new Dictionary<string, Stopwatch>();
            FindController();
            SetupUpdateTimer();
            LogMessage("?? AGCV Joy-Con Event Monitor iniciado", "SYSTEM", Color.Blue);
        }

        private void FindController()
        {
            for (int i = 0; i < 4; i++)
            {
                XINPUT_STATE state = new XINPUT_STATE();
                if (XInputGetState(i, ref state) == 0)
                {
                    controllerIndex = i;
                    wasConnected = true;
                    hasLoggedDisconnection = false;
                    hasLoggedNoController = false;
                    LogMessage($"? Controlador encontrado en slot {i + 1}", "SYSTEM", Color.Green);
                    LogMessage("Esperando input del Joy-Con...", "SYSTEM", Color.Gray);
                    return;
                }
            }

            // Solo loguear si es la primera vez que no se encuentra
            if (!hasLoggedNoController)
            {
                LogMessage("? No se detectó ningún controlador. Conecta tu Joy-Con con AGCV.", "WARNING", Color.Orange);
                hasLoggedNoController = true;
            }
        }

        private void SetupUpdateTimer()
        {
            updateTimer = new System.Windows.Forms.Timer();
            updateTimer.Interval = 8; // ~120 Hz para mejor precisión
            updateTimer.Tick += UpdateTimer_Tick;
            updateTimer.Start();
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            XINPUT_STATE state = new XINPUT_STATE();
            int result = XInputGetState(controllerIndex, ref state);

            if (result != 0)
            {
                lblStatus.Text = "? Joy-Con desconectado";
                lblStatus.ForeColor = Color.Red;

                // Solo loguear desconexión una vez
                if (wasConnected && !hasLoggedDisconnection)
                {
                    LogMessage("? Conexión perdida. Reconectando...", "ERROR", Color.Red);
                    hasLoggedDisconnection = true;
                    wasConnected = false;
                }

                // Intentar reconectar silenciosamente
                if (controllerIndex != -1)
                {
                    controllerIndex = -1;
                }

                FindController();
                return;
            }

            // Controlador conectado
            if (!wasConnected)
            {
                wasConnected = true;
                hasLoggedDisconnection = false;
            }

            lblStatus.Text = $"? Joy-Con conectado (Controller {controllerIndex + 1})";
            lblStatus.ForeColor = Color.Green;

            var gamepad = state.Gamepad;
            long latency = globalStopwatch.ElapsedMilliseconds;

            // Detectar cambios en botones
            DetectButtonChanges(gamepad, latency);

            // Actualizar información de triggers (solo si cambió significativamente)
            if (Math.Abs(gamepad.bLeftTrigger - previousLeftTrigger) > 10)
            {
                LogTriggerEvent("LT", gamepad.bLeftTrigger, latency);
                previousLeftTrigger = gamepad.bLeftTrigger;
            }

            if (Math.Abs(gamepad.bRightTrigger - previousRightTrigger) > 10)
            {
                LogTriggerEvent("RT", gamepad.bRightTrigger, latency);
                previousRightTrigger = gamepad.bRightTrigger;
            }
        }

        private void DetectButtonChanges(XINPUT_GAMEPAD gamepad, long latency)
        {
            ushort currentButtons = gamepad.wButtons;
            ushort changed = (ushort)(currentButtons ^ previousButtons);

            if (changed == 0) return;

            // Mapeo de botones
            var buttonMap = new Dictionary<ushort, string>
            {
                { XINPUT_GAMEPAD_A, "A" },
                { XINPUT_GAMEPAD_B, "B" },
                { XINPUT_GAMEPAD_X, "X" },
                { XINPUT_GAMEPAD_Y, "Y" },
                { XINPUT_GAMEPAD_LEFT_SHOULDER, "LB" },
                { XINPUT_GAMEPAD_RIGHT_SHOULDER, "RB" },
                { XINPUT_GAMEPAD_START, "START (+)" },
                { XINPUT_GAMEPAD_BACK, "BACK (-)" },
                { XINPUT_GAMEPAD_LEFT_THUMB, "L-STICK" },
                { XINPUT_GAMEPAD_RIGHT_THUMB, "R-STICK" },
                { XINPUT_GAMEPAD_DPAD_UP, "D-PAD ?" },
                { XINPUT_GAMEPAD_DPAD_DOWN, "D-PAD ?" },
                { XINPUT_GAMEPAD_DPAD_LEFT, "D-PAD ?" },
                { XINPUT_GAMEPAD_DPAD_RIGHT, "D-PAD ?" }
            };

            foreach (var button in buttonMap)
            {
                if ((changed & button.Key) != 0)
                {
                    bool isPressed = (currentButtons & button.Key) != 0;
                    LogButtonEvent(button.Value, isPressed, latency);
                }
            }

            previousButtons = currentButtons;
        }

        private void LogButtonEvent(string buttonName, bool isPressed, long latency)
        {
            string action = isPressed ? "PRESSED" : "RELEASED";
            Color color = isPressed ? Color.FromArgb(46, 204, 113) : Color.FromArgb(231, 76, 60);

            long pressDuration = 0;
            if (isPressed)
            {
                if (!buttonPressTimers.ContainsKey(buttonName))
                {
                    buttonPressTimers[buttonName] = Stopwatch.StartNew();
                }
                else
                {
                    buttonPressTimers[buttonName].Restart();
                }
            }
            else
            {
                if (buttonPressTimers.ContainsKey(buttonName) && buttonPressTimers[buttonName].IsRunning)
                {
                    pressDuration = buttonPressTimers[buttonName].ElapsedMilliseconds;
                    buttonPressTimers[buttonName].Stop();
                }
            }

            string joyConType = DetermineJoyConType(buttonName);
            string durationInfo = pressDuration > 0 ? $" | Duration: {pressDuration}ms" : "";

            LogMessage(
                $"[{joyConType}] {buttonName,-12} {action,-9} @ {latency}ms{durationInfo}",
                "INPUT",
                color
            );
        }

        private void LogTriggerEvent(string triggerName, byte value, long latency)
        {
            if (value > 128)
            {
                string joyConType = triggerName == "LT" ? "Joy-Con L" : "Joy-Con R";
                LogMessage(
                    $"[{joyConType}] {triggerName,-12} VALUE: {value}/255 ({(value * 100 / 255)}%) @ {latency}ms",
                    "TRIGGER",
                    Color.FromArgb(52, 152, 219)
                );
            }
        }

        private string DetermineJoyConType(string buttonName)
        {
            // Botones del Joy-Con Left
            if (buttonName.Contains("?") || buttonName.Contains("?") ||
                buttonName.Contains("?") || buttonName.Contains("?") ||
                buttonName == "LB" || buttonName == "L-STICK" ||
                buttonName == "BACK (-)")
            {
                return "Joy-Con L";
            }
            // Botones del Joy-Con Right
            else if (buttonName == "A" || buttonName == "B" ||
                     buttonName == "X" || buttonName == "Y" ||
                     buttonName == "RB" || buttonName == "R-STICK" ||
                     buttonName == "START (+)")
            {
                return "Joy-Con R";
            }

            return "Controller";
        }

        private void LogMessage(string message, string category, Color color)
        {
            eventCounter++;
            string timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
            string formattedMessage = $"[{timestamp}] [{category}] {message}";

            txtLog.SelectionStart = txtLog.TextLength;
            txtLog.SelectionLength = 0;
            txtLog.SelectionColor = color;
            txtLog.AppendText(formattedMessage + Environment.NewLine);
            txtLog.SelectionColor = txtLog.ForeColor;

            // Auto-scroll
            txtLog.SelectionStart = txtLog.Text.Length;
            txtLog.ScrollToCaret();

            // Actualizar contador
            lblEventCount.Text = $"Events: {eventCounter}";

            // Limitar líneas del log
            if (txtLog.Lines.Length > MAX_LOG_LINES)
            {
                var lines = txtLog.Lines.Skip(txtLog.Lines.Length - MAX_LOG_LINES).ToArray();
                txtLog.Lines = lines;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtLog.Clear();
            eventCounter = 0;
            lblEventCount.Text = "Events: 0";
            LogMessage("??? Log limpiado", "SYSTEM", Color.Blue);
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "Text Files (*.txt)|*.txt|Log Files (*.log)|*.log";
                    sfd.FileName = $"JoyCon_Log_{DateTime.Now:yyyyMMdd_HHmmss}";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        System.IO.File.WriteAllText(sfd.FileName, txtLog.Text);
                        LogMessage($"?? Log exportado a: {sfd.FileName}", "SYSTEM", Color.Green);

                        MessageBox.Show(
                            $"? Log exportado exitosamente\n\n{sfd.FileName}",
                            "Exportación Exitosa",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                LogMessage($"? Error al exportar: {ex.Message}", "ERROR", Color.Red);
            }
        }

        private void btnGetInfo_Click(object sender, EventArgs e)
        {
            if (controllerIndex == -1)
            {
                LogMessage("? No hay controlador conectado para obtener información", "WARNING", Color.Orange);
                return;
            }

            LogMessage("???????????????????????????????????????????????????", "INFO", Color.Blue);
            LogMessage($"?? Información del Controlador #{controllerIndex + 1}", "INFO", Color.Blue);
            LogMessage("???????????????????????????????????????????????????", "INFO", Color.Blue);

            // Obtener información de batería
            XINPUT_BATTERY_INFORMATION batteryInfo = new XINPUT_BATTERY_INFORMATION();
            XInputGetBatteryInformation(controllerIndex, 0, ref batteryInfo);

            string batteryType = batteryInfo.BatteryType switch
            {
                0 => "Desconectado",
                1 => "Alcalina",
                2 => "Ni-MH",
                3 => "Desconocido",
                _ => "N/A"
            };

            string batteryLevel = batteryInfo.BatteryLevel switch
            {
                0 => "Vacía",
                1 => "Baja (25%)",
                2 => "Media (50%)",
                3 => "Alta (75%)",
                _ => "Completa (100%)"
            };

            LogMessage($"?? Tipo de batería: {batteryType}", "INFO", Color.DarkCyan);
            LogMessage($"?? Nivel de batería: {batteryLevel}", "INFO", Color.DarkCyan);
            LogMessage($"?? Latencia promedio: ~{updateTimer.Interval}ms", "INFO", Color.DarkCyan);
            LogMessage($"?? Frecuencia de polling: ~{1000 / updateTimer.Interval}Hz", "INFO", Color.DarkCyan);
            LogMessage($"? Tiempo activo: {globalStopwatch.Elapsed:hh\\:mm\\:ss}", "INFO", Color.DarkCyan);
            LogMessage($"?? Total de eventos: {eventCounter}", "INFO", Color.DarkCyan);
            LogMessage("???????????????????????????????????????????????????", "INFO", Color.Blue);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            updateTimer?.Stop();
            updateTimer?.Dispose();
            globalStopwatch?.Stop();
            LogMessage("?? Joy-Con Event Monitor cerrado", "SYSTEM", Color.Blue);
            base.OnFormClosing(e);
        }

        private void lblTitulo_Click(object sender, EventArgs e)
        {

        }

        private void panelHeader_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
