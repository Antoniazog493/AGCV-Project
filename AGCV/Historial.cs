using capaNegocio;
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace AGCV
{
    public partial class Historial : Form
    {
        private readonly CNHistorial _cnHistorial = new CNHistorial();
        private int _idUsuario;
        private bool _esAdministrador;

        public Historial()
        {
            InitializeComponent();
        }

        private void Historial_Load(object sender, EventArgs e)
        {
            _idUsuario = SesionActual.IdUsuario;
            _esAdministrador = SesionActual.EsAdministrador();
            
            // Mostrar mensaje diferente según el rol
            if (_esAdministrador)
            {
                lblBienvenida.Text = $"Administrador: {SesionActual.NombreUsuario} - Historial Global";
            }
            else
            {
                lblBienvenida.Text = $"Bienvenido, {SesionActual.NombreUsuario}";
            }
            
            CargarHistorial();
        }

        /// <summary>
        /// Carga el historial del usuario en el DataGridView
        /// </summary>
        private void CargarHistorial()
        {
            try
            {
                DataTable historial;
                
                // Si es administrador, obtener TODO el historial
                if (_esAdministrador)
                {
                    historial = _cnHistorial.ObtenerTodoElHistorial(_idUsuario, 1000); // Últimos 1000 registros globales
                }
                else
                {
                    historial = _cnHistorial.ObtenerHistorial(_idUsuario, 500); // Últimos 500 registros del usuario
                }
                
                if (historial != null && historial.Rows.Count > 0)
                {
                    dataGridView1.DataSource = historial;
                    
                    // Configurar las columnas
                    ConfigurarColumnas();
                    
                    // Actualizar contador
                    lblRegistros.Text = $"Registros: {historial.Rows.Count}";
                    
                    // Mostrar estadísticas
                    MostrarEstadisticas();
                }
                else
                {
                    lblRegistros.Text = "Registros: 0";
                    
                    string mensaje = _esAdministrador 
                        ? "📊 No hay registros de historial en el sistema\n\n" +
                          "El historial comenzará a registrarse automáticamente\n" +
                          "cuando los usuarios utilicen AGCV."
                        : "📊 No hay registros de historial\n\n" +
                          "El historial comenzará a registrarse automáticamente\n" +
                          "cuando conectes Joy-Cons con AGCV.";
                    
                    MessageBox.Show(mensaje, "Información",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error al cargar el historial:\n\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Configura las columnas del DataGridView
        /// </summary>
        private void ConfigurarColumnas()
        {
            // Ocultar columnas innecesarias
            if (dataGridView1.Columns.Contains("IdHistorial"))
                dataGridView1.Columns["IdHistorial"].Visible = false;
            
            if (dataGridView1.Columns.Contains("IdUsuario"))
                dataGridView1.Columns["IdUsuario"].Visible = false;

            // Mostrar columna NombreUsuario solo para administradores
            if (dataGridView1.Columns.Contains("NombreUsuario"))
            {
                if (_esAdministrador)
                {
                    dataGridView1.Columns["NombreUsuario"].HeaderText = "Usuario";
                    dataGridView1.Columns["NombreUsuario"].Width = 150;
                    dataGridView1.Columns["NombreUsuario"].DisplayIndex = 0; // Primera columna
                }
                else
                {
                    dataGridView1.Columns["NombreUsuario"].Visible = false;
                }
            }

            // Configurar encabezados y anchos
            if (dataGridView1.Columns.Contains("FechaRegistro"))
            {
                dataGridView1.Columns["FechaRegistro"].HeaderText = "Fecha y Hora";
                dataGridView1.Columns["FechaRegistro"].Width = 180;
                dataGridView1.Columns["FechaRegistro"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss";
                if (_esAdministrador)
                {
                    dataGridView1.Columns["FechaRegistro"].DisplayIndex = 1; // Segunda columna
                }
            }

            if (dataGridView1.Columns.Contains("Accion"))
            {
                dataGridView1.Columns["Accion"].HeaderText = "Acción";
                dataGridView1.Columns["Accion"].Width = 200;
                if (_esAdministrador)
                {
                    dataGridView1.Columns["Accion"].DisplayIndex = 2; // Tercera columna
                }
            }

            if (dataGridView1.Columns.Contains("Detalles"))
            {
                dataGridView1.Columns["Detalles"].HeaderText = "Detalles";
                dataGridView1.Columns["Detalles"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                if (_esAdministrador)
                {
                    dataGridView1.Columns["Detalles"].DisplayIndex = 3; // Cuarta columna
                }
            }

            // Alternar colores de filas para mejor legibilidad
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(245, 245, 245);
            
            // Color especial para administradores
            if (_esAdministrador)
            {
                dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(230, 0, 18);
                dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
                dataGridView1.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold);
                dataGridView1.EnableHeadersVisualStyles = false;
            }
        }

        /// <summary>
        /// Muestra estadísticas del historial
        /// </summary>
        private void MostrarEstadisticas()
        {
            try
            {
                Dictionary<string, int> stats;
                
                if (_esAdministrador)
                {
                    // Para administradores: estadísticas globales
                    stats = _cnHistorial.ObtenerEstadisticasGlobales();
                    
                    if (stats != null && stats.Count > 0)
                    {
                        string mensaje = $"📊 ESTADÍSTICAS GLOBALES\n\n" +
                            $"Total de registros: {stats.GetValueOrDefault("TotalRegistros", 0)}\n" +
                            $"Usuarios activos: {stats.GetValueOrDefault("UsuariosConRegistros", 0)}\n" +
                            $"Conexiones: {stats.GetValueOrDefault("TotalConexiones", 0)}\n" +
                            $"Desconexiones: {stats.GetValueOrDefault("TotalDesconexiones", 0)}\n" +
                            $"Errores: {stats.GetValueOrDefault("TotalErrores", 0)}";
                        
                        // Mostrar en tooltip del label de registros
                        ToolTip tooltip = new ToolTip();
                        tooltip.SetToolTip(lblRegistros, mensaje);
                    }
                }
                else
                {
                    // Para usuarios normales: estadísticas personales
                    stats = _cnHistorial.ObtenerEstadisticas(_idUsuario);
                    
                    if (stats != null && stats.Count > 0)
                    {
                        string mensaje = $"📊 ESTADÍSTICAS\n\n" +
                            $"Total de registros: {stats.GetValueOrDefault("TotalRegistros", 0)}\n" +
                            $"Conexiones: {stats.GetValueOrDefault("TotalConexiones", 0)}\n" +
                            $"Desconexiones: {stats.GetValueOrDefault("TotalDesconexiones", 0)}";
                        
                        ToolTip tooltip = new ToolTip();
                        tooltip.SetToolTip(lblRegistros, mensaje);
                    }
                }
            }
            catch
            {
                // Ignorar errores de estadísticas
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "Archivo CSV|*.csv|Todos los archivos|*.*";
                    sfd.Title = "Exportar Historial a CSV";
                    
                    string nombreArchivo = _esAdministrador 
                        ? $"Historial_Global_AGCV_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
                        : $"Historial_AGCV_{SesionActual.NombreUsuario}_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                    
                    sfd.FileName = nombreArchivo;

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        bool exitoso;
                        
                        if (_esAdministrador)
                        {
                            exitoso = _cnHistorial.ExportarHistorialGlobalACSV(_idUsuario, sfd.FileName);
                        }
                        else
                        {
                            exitoso = _cnHistorial.ExportarACSV(_idUsuario, sfd.FileName);
                        }

                        if (exitoso)
                        {
                            var result = MessageBox.Show(
                                $"✅ EXPORTACIÓN EXITOSA\n\n" +
                                $"El historial se ha exportado correctamente a:\n\n" +
                                $"{sfd.FileName}\n\n" +
                                $"¿Deseas abrir la ubicación del archivo?",
                                "Exportación Completada",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Information);

                            if (result == DialogResult.Yes)
                            {
                                System.Diagnostics.Process.Start("explorer.exe", $"/select,\"{sfd.FileName}\"");
                            }
                        }
                        else
                        {
                            MessageBox.Show(
                                "❌ ERROR\n\nNo se pudo exportar el historial.\n\nVerifica los permisos de escritura.",
                                "Error de Exportación",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error al exportar:\n\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            try
            {
                // Solo administradores pueden limpiar el historial global
                if (!_esAdministrador)
                {
                    MessageBox.Show(
                        "⚠️ ACCESO DENEGADO\n\n" +
                        "Solo los administradores pueden limpiar el historial.\n\n" +
                        "Contacta a un administrador si necesitas limpiar tu historial.",
                        "Permiso Requerido",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                // Mostrar opciones de limpieza
                using (var dialog = new Form())
                {
                    dialog.Text = "Limpiar Historial Global";
                    dialog.Width = 450;
                    dialog.Height = 250;
                    dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                    dialog.StartPosition = FormStartPosition.CenterParent;
                    dialog.MaximizeBox = false;
                    dialog.MinimizeBox = false;

                    var label = new Label
                    {
                        Text = "¿Qué deseas limpiar? (Historial Global)",
                        Left = 20,
                        Top = 20,
                        Width = 400,
                        Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold)
                    };

                    var btnTodo = new Button
                    {
                        Text = "🗑️ Limpiar TODO el historial del sistema",
                        Left = 20,
                        Top = 60,
                        Width = 400,
                        Height = 40,
                        BackColor = System.Drawing.Color.FromArgb(230, 0, 18),
                        ForeColor = System.Drawing.Color.White,
                        FlatStyle = FlatStyle.Flat
                    };
                    btnTodo.FlatAppearance.BorderSize = 0;
                    btnTodo.Click += (s, args) =>
                    {
                        dialog.DialogResult = DialogResult.Yes;
                        dialog.Tag = "todo";
                        dialog.Close();
                    };

                    var btnAntiguo = new Button
                    {
                        Text = "📅 Limpiar registros antiguos (> 30 días)",
                        Left = 20,
                        Top = 110,
                        Width = 400,
                        Height = 40,
                        BackColor = System.Drawing.Color.FromArgb(0, 150, 215),
                        ForeColor = System.Drawing.Color.White,
                        FlatStyle = FlatStyle.Flat
                    };
                    btnAntiguo.FlatAppearance.BorderSize = 0;
                    btnAntiguo.Click += (s, args) =>
                    {
                        dialog.DialogResult = DialogResult.Yes;
                        dialog.Tag = "antiguo";
                        dialog.Close();
                    };

                    var btnCancelar = new Button
                    {
                        Text = "Cancelar",
                        Left = 20,
                        Top = 160,
                        Width = 400,
                        Height = 35,
                        BackColor = System.Drawing.Color.FromArgb(127, 140, 141),
                        ForeColor = System.Drawing.Color.White,
                        FlatStyle = FlatStyle.Flat
                    };
                    btnCancelar.FlatAppearance.BorderSize = 0;
                    btnCancelar.Click += (s, args) =>
                    {
                        dialog.DialogResult = DialogResult.Cancel;
                        dialog.Close();
                    };

                    dialog.Controls.Add(label);
                    dialog.Controls.Add(btnTodo);
                    dialog.Controls.Add(btnAntiguo);
                    dialog.Controls.Add(btnCancelar);

                    if (dialog.ShowDialog() == DialogResult.Yes)
                    {
                        string opcion = dialog.Tag?.ToString();

                        if (opcion == "todo")
                        {
                            var confirmacion = MessageBox.Show(
                                "⚠️ ADVERTENCIA CRÍTICA\n\n" +
                                "¿Estás seguro de que deseas eliminar TODO el historial del sistema?\n\n" +
                                "Esto eliminará los registros de TODOS los usuarios.\n\n" +
                                "Esta acción NO se puede deshacer.",
                                "Confirmar Eliminación Global",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Warning);

                            if (confirmacion == DialogResult.Yes)
                            {
                                bool exitoso = _cnHistorial.LimpiarHistorialGlobal(_idUsuario);

                                if (exitoso)
                                {
                                    MessageBox.Show(
                                        "✅ Historial global limpiado correctamente",
                                        "Éxito",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);
                                    
                                    CargarHistorial(); // Recargar vista
                                }
                                else
                                {
                                    MessageBox.Show(
                                        "❌ Error al limpiar el historial global",
                                        "Error",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                                }
                            }
                        }
                        else if (opcion == "antiguo")
                        {
                            int eliminados = _cnHistorial.LimpiarHistorialGlobalAntiguo(_idUsuario, 30);

                            MessageBox.Show(
                                $"✅ Se eliminaron {eliminados} registros antiguos del sistema\n\n" +
                                "(Registros con más de 30 días de todos los usuarios)",
                                "Limpieza Completada",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

                            CargarHistorial(); // Recargar vista
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error al limpiar el historial:\n\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lblTituloText_Click(object sender, EventArgs e) { }
    }
}
