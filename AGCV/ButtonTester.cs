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
        private System.Windows.Forms.Timer scanTimer;
        private int controllerIndex = -1;
        
        // Estado por controlador para evitar mensajes duplicados
        private Dictionary<int, ushort> previousButtons = new Dictionary<int, ushort>();
        private Dictionary<int, byte> previousLeftTrigger = new Dictionary<int, byte>();
        private Dictionary<int, byte> previousRightTrigger = new Dictionary<int, byte>();
        private Dictionary<int, bool> wasConnected = new Dictionary<int, bool>();
        
        private Stopwatch globalStopwatch;
        private Dictionary<string, Stopwatch> buttonPressTimers;
        private int eventCounter = 0;
        private const int MAX_LOG_LINES = 1000;

        // Estados para evitar mensajes repetidos
        private bool hasLoggedNoController = false;
        private HashSet<int> availableControllers = new HashSet<int>();

        public ButtonTester()
        {
            InitializeComponent();
            globalStopwatch = Stopwatch.StartNew();
            buttonPressTimers = new Dictionary<string, Stopwatch>();
            
            // Inicializar estados para 4 controladores
            for (int i = 0; i < 4; i++)
            {
                previousButtons[i] = 0;
                previousLeftTrigger[i] = 0;
                previousRightTrigger[i] = 0;
                wasConnected[i] = false;
            }
            
            // Configurar ComboBox de controladores
            SetupControllerSelector();
            
            // Escanear controladores disponibles
            ScanAvailableControllers();
            
            // Auto-seleccionar primer controlador disponible
            if (availableControllers.Count > 0)
            {
                int firstAvailable = availableControllers.Min();
                controllerIndex = firstAvailable;
                cmbControllers.SelectedIndex = firstAvailable + 1; // +1 porque index 0 es "Auto-detect"
            }
            
            SetupTimers();
            LogMessage("AGCV Joy-Con Event Monitor iniciado", "SYSTEM", Color.Blue);
        }

        private void SetupControllerSelector()
        {
            cmbControllers.Items.Clear();
            cmbControllers.Items.Add("Auto-detect");
            cmbControllers.Items.Add("Controller 1");
            cmbControllers.Items.Add("Controller 2");
            cmbControllers.Items.Add("Controller 3");
            cmbControllers.Items.Add("Controller 4");
            cmbControllers.SelectedIndex = 0;
            cmbControllers.SelectedIndexChanged += CmbControllers_SelectedIndexChanged;
        }

        private void ScanAvailableControllers()
        {
            availableControllers.Clear();
            for (int i = 0; i < 4; i++)
            {
                XINPUT_STATE state = new XINPUT_STATE();
                if (XInputGetState(i, ref state) == 0)
                {
                    availableControllers.Add(i);
                }
            }
            
            // Actualizar ComboBox con indicadores de disponibilidad
            UpdateControllerList();
        }

        private void UpdateControllerList()
        {
            // Guardar selección actual
            int currentSelection = cmbControllers.SelectedIndex;
            
            cmbControllers.Items.Clear();
            cmbControllers.Items.Add("Auto-detect");
            
            for (int i = 0; i < 4; i++)
            {
                string status = availableControllers.Contains(i) ? " [Conectado]" : " [Desconectado]";
                cmbControllers.Items.Add($"Controller {i + 1}{status}");
            }
            
            // Restaurar selección si es válida
            if (currentSelection >= 0 && currentSelection < cmbControllers.Items.Count)
            {
                cmbControllers.SelectedIndex = currentSelection;
            }
            else
            {
                cmbControllers.SelectedIndex = 0;
            }
        }

        private void CmbControllers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbControllers.SelectedIndex == 0)
            {
                // Auto-detect - seleccionar primer controlador disponible
                controllerIndex = -1;
                LogMessage("Modo auto-detect activado", "SYSTEM", Color.Blue);
                FindController();
            }
            else
            {
                // Manual selection
                int newIndex = cmbControllers.SelectedIndex - 1;
                XINPUT_STATE state = new XINPUT_STATE();
                if (XInputGetState(newIndex, ref state) == 0)
                {
                    if (controllerIndex != newIndex)
                    {
                        controllerIndex = newIndex;
                        wasConnected[newIndex] = true;
                        LogMessage($"Cambiado manualmente a Controller {newIndex + 1}", "SYSTEM", Color.Green);
                    }
                }
                else
                {
                    LogMessage($"Controller {newIndex + 1} no esta conectado", "WARNING", Color.Orange);
                    // NO buscar automáticamente otro - dejar que el usuario elija
                }
            }
        }

        private void FindController()
        {
            // SOLO buscar automáticamente si estamos en modo auto-detect
            if (cmbControllers.SelectedIndex != 0)
            {
                // Modo manual - no hacer nada
                return;
            }

            // Si hay selección manual válida, no hacer auto-detect
            if (cmbControllers.SelectedIndex > 0 && controllerIndex >= 0)
            {
                XINPUT_STATE testState = new XINPUT_STATE();
                if (XInputGetState(controllerIndex, ref testState) == 0)
                {
                    return; // Controlador actual está bien
                }
            }

            // Buscar primer controlador disponible
            for (int i = 0; i < 4; i++)
            {
                XINPUT_STATE state = new XINPUT_STATE();
                if (XInputGetState(i, ref state) == 0)
                {
                    if (controllerIndex != i)
                    {
                        controllerIndex = i;
                        wasConnected[i] = true;
                        hasLoggedNoController = false;
                        LogMessage($"Controlador encontrado en slot {i + 1} (auto-detect)", "SYSTEM", Color.Green);
                        LogMessage("Esperando input del Joy-Con...", "SYSTEM", Color.Gray);
                    }
                    return;
                }
            }

            // No se encontró ningún controlador
            if (!hasLoggedNoController)
            {
                LogMessage("No se detecto ningun controlador. Conecta tu Joy-Con con AGCV.", "WARNING", Color.Orange);
                hasLoggedNoController = true;
            }
            controllerIndex = -1;
        }

        private void SetupTimers()
        {
            // Timer principal de actualización - más rápido para baja latencia
            updateTimer = new System.Windows.Forms.Timer();
            updateTimer.Interval = 5; // ~200 Hz para respuesta inmediata
            updateTimer.Tick += UpdateTimer_Tick;
            updateTimer.Start();
            
            // Timer de escaneo de controladores - solo cada 2 segundos
            scanTimer = new System.Windows.Forms.Timer();
            scanTimer.Interval = 2000;
            scanTimer.Tick += ScanTimer_Tick;
            scanTimer.Start();
        }

        private void ScanTimer_Tick(object sender, EventArgs e)
        {
            // Escanear controladores disponibles periódicamente
            ScanAvailableControllers();
            
            // FIX: Solo buscar otro controlador si el actual se desconectó Y estamos en modo auto-detect
            if (controllerIndex >= 0)
            {
                XINPUT_STATE state = new XINPUT_STATE();
                if (XInputGetState(controllerIndex, ref state) != 0)
                {
                    // El controlador actual se desconectó
                    LogMessage($"Controller {controllerIndex + 1} desconectado. Buscando otro...", "WARNING", Color.Orange);
                    
                    // Solo auto-cambiar si estamos en modo auto-detect
                    if (cmbControllers.SelectedIndex == 0)
                    {
                        FindController();
                    }
                }
            }
            else if (cmbControllers.SelectedIndex == 0)
            {
                // En modo auto-detect sin controlador, intentar encontrar uno
                FindController();
            }
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            if (controllerIndex < 0)
            {
                lblStatus.Text = "Joy-Con desconectado";
                lblStatus.ForeColor = Color.Red;
                return;
            }

            XINPUT_STATE state = new XINPUT_STATE();
            int result = XInputGetState(controllerIndex, ref state);

            if (result != 0)
            {
                lblStatus.Text = $"Controller {controllerIndex + 1} desconectado";
                lblStatus.ForeColor = Color.Red;

                // Solo loguear desconexion una vez
                if (wasConnected[controllerIndex])
                {
                    LogMessage($"Controller {controllerIndex + 1} desconectado", "ERROR", Color.Red);
                    wasConnected[controllerIndex] = false;
                }
                return;
            }

            // Controlador conectado
            if (!wasConnected[controllerIndex])
            {
                wasConnected[controllerIndex] = true;
                LogMessage($"Controller {controllerIndex + 1} reconectado", "SYSTEM", Color.Green);
            }

            lblStatus.Text = $"Joy-Con conectado (Controller {controllerIndex + 1})";
            lblStatus.ForeColor = Color.Green;

            var gamepad = state.Gamepad;
            long latency = globalStopwatch.ElapsedMilliseconds;

            // Detectar cambios en botones
            DetectButtonChanges(gamepad, latency);

            // Actualizar informacion de triggers (solo si cambio significativamente)
            if (Math.Abs(gamepad.bLeftTrigger - previousLeftTrigger[controllerIndex]) > 10)
            {
                LogTriggerEvent("LT", gamepad.bLeftTrigger, latency);
                previousLeftTrigger[controllerIndex] = gamepad.bLeftTrigger;
            }

            if (Math.Abs(gamepad.bRightTrigger - previousRightTrigger[controllerIndex]) > 10)
            {
                LogTriggerEvent("RT", gamepad.bRightTrigger, latency);
                previousRightTrigger[controllerIndex] = gamepad.bRightTrigger;
            }
        }

        private void DetectButtonChanges(XINPUT_GAMEPAD gamepad, long latency)
        {
            ushort currentButtons = gamepad.wButtons;
            ushort changed = (ushort)(currentButtons ^ previousButtons[controllerIndex]);

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
                { XINPUT_GAMEPAD_DPAD_UP, "D-PAD UP" },
                { XINPUT_GAMEPAD_DPAD_DOWN, "D-PAD DOWN" },
                { XINPUT_GAMEPAD_DPAD_LEFT, "D-PAD LEFT" },
                { XINPUT_GAMEPAD_DPAD_RIGHT, "D-PAD RIGHT" }
            };

            foreach (var button in buttonMap)
            {
                if ((changed & button.Key) != 0)
                {
                    bool isPressed = (currentButtons & button.Key) != 0;
                    LogButtonEvent(button.Value, isPressed, latency);
                }
            }

            previousButtons[controllerIndex] = currentButtons;
        }

        private void LogButtonEvent(string buttonName, bool isPressed, long latency)
        {
            string action = isPressed ? "PRESSED" : "RELEASED";
            Color color = isPressed ? Color.FromArgb(46, 204, 113) : Color.FromArgb(231, 76, 60);

            string timerKey = $"C{controllerIndex}_{buttonName}";
            long pressDuration = 0;
            
            if (isPressed)
            {
                if (!buttonPressTimers.ContainsKey(timerKey))
                {
                    buttonPressTimers[timerKey] = Stopwatch.StartNew();
                }
                else
                {
                    buttonPressTimers[timerKey].Restart();
                }
            }
            else
            {
                if (buttonPressTimers.ContainsKey(timerKey) && buttonPressTimers[timerKey].IsRunning)
                {
                    pressDuration = buttonPressTimers[timerKey].ElapsedMilliseconds;
                    buttonPressTimers[timerKey].Stop();
                }
            }

            string joyConType = DetermineJoyConType(buttonName);
            string durationInfo = pressDuration > 0 ? $" | Duration: {pressDuration}ms" : "";
            string controllerInfo = $"[Ctrl {controllerIndex + 1}]";

            LogMessage(
                $"{controllerInfo} [{joyConType}] {buttonName,-12} {action,-9} @ {latency}ms{durationInfo}",
                "INPUT",
                color
            );
        }

        private void LogTriggerEvent(string triggerName, byte value, long latency)
        {
            if (value > 128)
            {
                string joyConType = triggerName == "LT" ? "Joy-Con L" : "Joy-Con R";
                string controllerInfo = $"[Ctrl {controllerIndex + 1}]";
                
                LogMessage(
                    $"{controllerInfo} [{joyConType}] {triggerName,-12} VALUE: {value}/255 ({(value * 100 / 255)}%) @ {latency}ms",
                    "TRIGGER",
                    Color.FromArgb(52, 152, 219)
                );
            }
        }

        private string DetermineJoyConType(string buttonName)
        {
            // Botones del Joy-Con Left
            if (buttonName.Contains("UP") || buttonName.Contains("DOWN") ||
                buttonName.Contains("LEFT") || buttonName.Contains("RIGHT") ||
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
            if (InvokeRequired)
            {
                Invoke(new Action<string, string, Color>(LogMessage), message, category, color);
                return;
            }

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

            // Limitar lineas del log
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
            LogMessage("Log limpiado", "SYSTEM", Color.Blue);
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
                        LogMessage($"Log exportado a: {sfd.FileName}", "SYSTEM", Color.Green);

                        MessageBox.Show(
                            $"Log exportado exitosamente\n\n{sfd.FileName}",
                            "Exportacion Exitosa",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                LogMessage($"Error al exportar: {ex.Message}", "ERROR", Color.Red);
            }
        }

        private void btnGetInfo_Click(object sender, EventArgs e)
        {
            if (controllerIndex == -1)
            {
                LogMessage("No hay controlador conectado para obtener informacion", "WARNING", Color.Orange);
                return;
            }

            LogMessage("========================================", "INFO", Color.Blue);
            LogMessage($"Informacion del Controlador #{controllerIndex + 1}", "INFO", Color.Blue);
            LogMessage("========================================", "INFO", Color.Blue);

            // Obtener informacion de bateria
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
                0 => "Vacia",
                1 => "Baja (25%)",
                2 => "Media (50%)",
                3 => "Alta (75%)",
                _ => "Completa (100%)"
            };

            LogMessage($"Tipo de bateria: {batteryType}", "INFO", Color.DarkCyan);
            LogMessage($"Nivel de bateria: {batteryLevel}", "INFO", Color.DarkCyan);
            LogMessage($"Latencia promedio: ~{updateTimer.Interval}ms", "INFO", Color.DarkCyan);
            LogMessage($"Frecuencia de polling: ~{1000 / updateTimer.Interval}Hz", "INFO", Color.DarkCyan);
            LogMessage($"Tiempo activo: {globalStopwatch.Elapsed:hh\\:mm\\:ss}", "INFO", Color.DarkCyan);
            LogMessage($"Total de eventos: {eventCounter}", "INFO", Color.DarkCyan);
            
            // Información de controladores disponibles
            LogMessage($"Controladores disponibles: {availableControllers.Count}/4", "INFO", Color.DarkCyan);
            foreach (int idx in availableControllers.OrderBy(x => x))
            {
                LogMessage($"  - Controller {idx + 1}", "INFO", Color.DarkCyan);
            }
            
            LogMessage("========================================", "INFO", Color.Blue);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            updateTimer?.Stop();
            updateTimer?.Dispose();
            scanTimer?.Stop();
            scanTimer?.Dispose();
            globalStopwatch?.Stop();
            LogMessage("Joy-Con Event Monitor cerrado", "SYSTEM", Color.Blue);
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
