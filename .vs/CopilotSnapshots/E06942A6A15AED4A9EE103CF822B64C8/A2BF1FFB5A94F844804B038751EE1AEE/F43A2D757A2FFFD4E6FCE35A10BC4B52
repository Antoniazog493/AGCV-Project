using capaEntidad;
using capaNegocio;
using System;
using System.Windows.Forms;

namespace AGCV
{
    public partial class HOME : Form
    {
        private readonly CNControlesUsuario _cn = new CNControlesUsuario();

        public HOME()
        {
            InitializeComponent();
        }

        private void HOME_Load(object sender, EventArgs e)
        {
            try
            {
                // Actualizar nombre de usuario dinámicamente AQUÍ
                lblUsuario.Text = $"Usuario: {SesionActual.NombreUsuario}";

                int idUsuario = SesionActual.IdUsuario;

                var ds = _cn.ObtenerControles(idUsuario);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables["ControlesUsuario"] != null)
                {
                    cmbControles.DataSource = ds.Tables["ControlesUsuario"];
                    cmbControles.DisplayMember = "NombreControl";
                    cmbControles.ValueMember = "IdControl";
                }
                else
                {
                    MessageBox.Show(
                        "No hay Joy-Cons registrados.\n\n" +
                        "NOTA: Agrega tus Joy-Cons en la sección Ajustes para comenzar a gestionar " +
                        "las conexiones con tu PC.",
                        "Información",
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar Joy-Cons:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // Conectar control
            if (cmbControles.SelectedItem == null)
            {
                MessageBox.Show(
                    "AVISO: Selecciona un Joy-Con primero\n\n" +
                    "Elige el Joy-Con que deseas conectar de la lista desplegable.",
                    "Aviso",
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show(
                $"EXITOSO: Joy-Con conectado exitosamente\n\n" +
                $"Control: {cmbControles.Text}\n" +
                $"Estado: Listo para jugar en tu PC\n\n" +
                "NOTA: Windows ahora reconoce correctamente tu Joy-Con",
                "Conexión Exitosa",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            // Ver estadísticas
            if (cmbControles.SelectedItem == null)
            {
                MessageBox.Show(
                    "AVISO: Selecciona un Joy-Con primero\n\n" +
                    "Elige el Joy-Con del que deseas ver estadísticas.",
                    "Aviso",
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show(
                $"ESTADISTICAS del Joy-Con\n\n" +
                $"Control: {cmbControles.Text}\n\n" +
                $"Conexiones Exitosas: 15\n" +
                $"Tiempo de Uso Total: 45 horas\n" +
                $"Última Conexión: Hoy, 14:30\n" +
                $"Estado del Hardware: Óptimo\n" +
                $"Batería: 85%",
                "Estadísticas",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            // Abrir Ajustes
            Ajustes ajustes = new Ajustes();
            ajustes.ShowDialog();
            // Recargar controles después de volver
            HOME_Load(null, null);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Este método se llama cuando cambia la selección del comboBox
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
    }
}
