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

        public Historial()
        {
            InitializeComponent();
        }

        private void Historial_Load(object sender, EventArgs e)
        {
            _idUsuario = SesionActual.IdUsuario;
            lblBienvenida.Text = $"Bienvenido, {SesionActual.NombreUsuario}";
            
            CargarHistorial();
        }

        /// <summary>
        /// Carga el historial del usuario en el DataGridView
        /// </summary>
        private void CargarHistorial()
        {
            try
            {
                DataTable historial = _cnHistorial.ObtenerHistorial(_idUsuario, 500); // Últimos 500 registros
                
                if (historial != null && historial.Rows.Count > 0)
                {
                    dataGridView1.DataSource = historial;
                    
                    // Configurar las columnas
                    ConfigurarColumnas();
                    
                    // Actualizar contador
                    int totalRegistros = _cnHistorial.ContarRegistros(_idUsuario);
                    lblRegistros.Text = $"Registros: {totalRegistros}";
                    
                    // Mostrar estadísticas
                    MostrarEstadisticas();
                }
                else
                {
                    lblRegistros.Text = "Registros: 0";
                    MessageBox.Show(
                        "📊 No hay registros de historial\n\n" +
                        "El historial comenzará a registrarse automáticamente\n" +
                        "cuando conectes Joy-Cons con AGCV.",
                        "Información",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
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

            // Configurar encabezados y anchos
            if (dataGridView1.Columns.Contains("FechaRegistro"))
            {
                dataGridView1.Columns["FechaRegistro"].HeaderText = "Fecha y Hora";
                dataGridView1.Columns["FechaRegistro"].Width = 180;
                dataGridView1.Columns["FechaRegistro"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss";
            }

            if (dataGridView1.Columns.Contains("Accion"))
            {
                dataGridView1.Columns["Accion"].HeaderText = "Acción";
                dataGridView1.Columns["Accion"].Width = 200;
            }

            if (dataGridView1.Columns.Contains("Detalles"))
            {
                dataGridView1.Columns["Detalles"].HeaderText = "Detalles";
                dataGridView1.Columns["Detalles"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            // Alternar colores de filas para mejor legibilidad
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(245, 245, 245);
        }

        /// <summary>
        /// Muestra estadísticas del historial en la consola
        /// </summary>
        private void MostrarEstadisticas()
        {
            try
            {
                var stats = _cnHistorial.ObtenerEstadisticas(_idUsuario);
                
                if (stats != null && stats.Count > 0)
                {
                    string mensaje = $"📊 ESTADÍSTICAS\n\n" +
                        $"Total de registros: {stats.GetValueOrDefault("TotalRegistros", 0)}\n" +
                        $"Conexiones: {stats.GetValueOrDefault("TotalConexiones", 0)}\n" +
                        $"Desconexiones: {stats.GetValueOrDefault("TotalDesconexiones", 0)}";
                    
                    // Se podría mostrar en un tooltip o en un label adicional
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
                    sfd.FileName = $"Historial_AGCV_{SesionActual.NombreUsuario}_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        bool exitoso = _cnHistorial.ExportarACSV(_idUsuario, sfd.FileName);

                        if (exitoso)
                        {
                            MessageBox.Show(
                                $"✅ EXPORTACIÓN EXITOSA\n\n" +
                                $"El historial se ha exportado correctamente a:\n\n" +
                                $"{sfd.FileName}\n\n" +
                                $"¿Deseas abrir el archivo ahora?",
                                "Exportación Completada",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Information);

                            if (MessageBox.Show("", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
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
                // Mostrar opciones de limpieza
                using (var dialog = new Form())
                {
                    dialog.Text = "Limpiar Historial";
                    dialog.Width = 450;
                    dialog.Height = 250;
                    dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                    dialog.StartPosition = FormStartPosition.CenterParent;
                    dialog.MaximizeBox = false;
                    dialog.MinimizeBox = false;

                    var label = new Label
                    {
                        Text = "¿Qué deseas limpiar?",
                        Left = 20,
                        Top = 20,
                        Width = 400,
                        Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold)
                    };

                    var btnTodo = new Button
                    {
                        Text = "🗑️ Limpiar TODO el historial",
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
                                "⚠️ ADVERTENCIA\n\n" +
                                "¿Estás seguro de que deseas eliminar TODO el historial?\n\n" +
                                "Esta acción NO se puede deshacer.",
                                "Confirmar Eliminación",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Warning);

                            if (confirmacion == DialogResult.Yes)
                            {
                                bool exitoso = _cnHistorial.LimpiarHistorial(_idUsuario);

                                if (exitoso)
                                {
                                    MessageBox.Show(
                                        "✅ Historial limpiado correctamente",
                                        "Éxito",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);
                                    
                                    CargarHistorial(); // Recargar vista
                                }
                                else
                                {
                                    MessageBox.Show(
                                        "❌ Error al limpiar el historial",
                                        "Error",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                                }
                            }
                        }
                        else if (opcion == "antiguo")
                        {
                            int eliminados = _cnHistorial.LimpiarHistorialAntiguo(_idUsuario, 30);

                            MessageBox.Show(
                                $"✅ Se eliminaron {eliminados} registros antiguos\n\n" +
                                "(Registros con más de 30 días)",
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
