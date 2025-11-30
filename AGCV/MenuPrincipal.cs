using capaEntidad;
using capaNegocio;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace AGCV
{
    public partial class HOME : Form
    {
        public HOME()
        {
            InitializeComponent();
        }

        private void HOME_Load(object sender, EventArgs e)
        {
            // Actualizar nombre de usuario dinámicamente
            lblUsuario.Text = $"Usuario: {SesionActual.NombreUsuario}";
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // Abrir AGCV para conectar Joy-Cons
            AbrirAGCV();
        }

        /// <summary>
        /// Abre AGCV para gestionar la conexión de Joy-Cons
        /// </summary>
        private void AbrirAGCV()
        {
            try
            {
                // Buscar BetterJoyForCemu.exe (motor de AGCV)
                string agcvPath = BuscarAGCV();

                if (string.IsNullOrEmpty(agcvPath))
                {
                    var resultado = MessageBox.Show(
                        "ERROR: No se encontró el motor de AGCV\n\n" +
                        "¿Deseas seleccionar manualmente la ubicación del motor AGCV?",
                        "Motor AGCV no encontrado",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Error);

                    if (resultado == DialogResult.Yes)
                    {
                        using (OpenFileDialog ofd = new OpenFileDialog())
                        {
                            ofd.Filter = "AGCV Motor (BetterJoyForCemu.exe)|BetterJoyForCemu.exe|Ejecutables (*.exe)|*.exe";
                            ofd.Title = "Selecciona el motor de AGCV";

                            if (ofd.ShowDialog() == DialogResult.OK)
                            {
                                agcvPath = ofd.FileName;
                                GuardarRutaAGCV(agcvPath);
                            }
                            else
                            {
                                return;
                            }
                        }
                    }
                    else
                    {
                        return;
                    }
                }

                // Verificar si AGCV ya está ejecutándose
                Process[] procesosAGCV = Process.GetProcessesByName("BetterJoyForCemu");
                if (procesosAGCV.Length > 0)
                {
                    MessageBox.Show(
                        "ℹ️ AGCV ya está en ejecución\n\n" +
                        "El motor de AGCV ya está activo.\n" +
                        "Si no ves la ventana, búscala en la barra de tareas.",
                        "Información",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return;
                }

                // Iniciar AGCV
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = agcvPath,
                    UseShellExecute = true,
                    WorkingDirectory = Path.GetDirectoryName(agcvPath)
                };

                Process.Start(startInfo);

                MessageBox.Show(
                    "✅ EXITOSO: AGCV iniciado correctamente\n\n" +
                    "INSTRUCCIONES PARA CONECTAR TU JOY-CON:\n\n" +
                    "1. Presiona los botones de sincronización en tu Joy-Con:\n" +
                    "   • Joy-Con L: Botón pequeño al lado del botón '-'\n" +
                    "   • Joy-Con R: Botón pequeño al lado del botón '+'\n\n" +
                    "2. El Joy-Con aparecerá en la ventana de AGCV\n\n" +
                    "3. Windows reconocerá automáticamente el Joy-Con\n" +
                    "   como un control Xbox compatible\n\n" +
                    "NOTA: Mantén AGCV abierto mientras usas el Joy-Con",
                    "AGCV Iniciado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"ERROR: No se pudo iniciar AGCV:\n\n{ex.Message}\n\n" +
                    "Verifica que el motor de AGCV esté instalado correctamente.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Busca el motor de AGCV (BetterJoyForCemu.exe) en varias ubicaciones comunes
        /// </summary>
        private string BuscarAGCV()
        {
            string[] posiblesRutas = new string[]
            {
                // 1. Ruta guardada previamente
                ObtenerRutaGuardada(),
                
                // 2. En el proyecto BetterJoyForCemu (compilado Debug)
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "BetterJoyForCemu", "bin", "x64", "Debug", "net8.0-windows", "BetterJoyForCemu.exe"),
                
                // 3. En el proyecto BetterJoyForCemu (compilado Release)
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "BetterJoyForCemu", "bin", "x64", "Release", "net8.0-windows", "BetterJoyForCemu.exe"),
                
                // 4. Junto a la aplicación principal
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AGCV", "BetterJoyForCemu.exe"),
                
                // 5. Ruta absoluta del proyecto
                @"C:\Users\Anton\Downloads\BetterJoy\AGCV-Project\BetterJoyForCemu\bin\x64\Debug\net8.0-windows\BetterJoyForCemu.exe"
            };

            foreach (string ruta in posiblesRutas)
            {
                if (!string.IsNullOrEmpty(ruta) && File.Exists(ruta))
                {
                    return Path.GetFullPath(ruta);
                }
            }

            return null;
        }

        /// <summary>
        /// Guarda la ruta del motor de AGCV para uso futuro
        /// </summary>
        private void GuardarRutaAGCV(string ruta)
        {
            try
            {
                string configFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "agcv_config.txt");
                File.WriteAllText(configFile, ruta);
            }
            catch
            {
                // Ignorar errores al guardar configuración
            }
        }

        /// <summary>
        /// Obtiene la ruta guardada del motor de AGCV
        /// </summary>
        private string ObtenerRutaGuardada()
        {
            try
            {
                string configFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "agcv_config.txt");
                if (File.Exists(configFile))
                {
                    return File.ReadAllText(configFile).Trim();
                }
                
                // Intentar leer configuración antigua de BetterJoy (compatibilidad)
                string oldConfigFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "betterjoy_config.txt");
                if (File.Exists(oldConfigFile))
                {
                    return File.ReadAllText(oldConfigFile).Trim();
                }
            }
            catch
            {
                // Ignorar errores al leer configuración
            }
            return null;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            // Abrir Monitor de Eventos
            ButtonTester tester = new ButtonTester();
            tester.ShowDialog();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            // Abrir Ajustes
            Ajustes ajustes = new Ajustes();
            ajustes.ShowDialog();
        }

        private void HOME_Click(object sender, EventArgs e)
        {
        }

        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            CerrarSesion();
        }

        private void CerrarSesion()
        {
            var resultado = MessageBox.Show(
                "¿Estás seguro de que deseas cerrar sesión?",
                "Confirmar Cierre de Sesión",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                // Limpiar sesión
                SesionActual.IdUsuario = 0;
                SesionActual.NombreUsuario = string.Empty;

                // Cerrar menú principal (volverá al LOGIN)
                this.Close();
            }
        }

        private void lblTitulo_Click(object sender, EventArgs e)
        {
        }

        private void lblTitulo_Click_1(object sender, EventArgs e)
        {

        }
    }
}
