using System;
using System.Windows.Forms;

namespace AGCV
{
    public partial class Historial : Form
    {
        private const string MensajeHistorialInfo = 
            "📊 HISTORIAL DE CONEXIONES\n\n" +
            "Esta funcionalidad mostrará el historial de\n" +
            "conexiones de Joy-Cons una vez implementada\n" +
            "con AGCV.\n\n" +
            "Próximamente:\n" +
            "• Registro de conexiones\n" +
            "• Tiempo de uso por sesión\n" +
            "• Estadísticas de batería\n" +
            "• Errores de conexión";

        private const string MensajeExportarInfo = 
            "ℹ️ EXPORTAR HISTORIAL\n\n" +
            "La función de exportar estará disponible\n" +
            "una vez se implemente el historial completo.\n\n" +
            "Podrás exportar a:\n" +
            "• CSV\n" +
            "• Excel\n" +
            "• PDF";

        private const string MensajeLimpiarInfo = 
            "ℹ️ LIMPIAR HISTORIAL\n\n" +
            "La función de limpiar historial estará\n" +
            "disponible una vez se implemente el\n" +
            "registro de conexiones.";

        public Historial()
        {
            InitializeComponent();
        }

        private void Historial_Load(object sender, EventArgs e)
        {
            lblBienvenida.Text = $"Bienvenido, {SesionActual.NombreUsuario}";
            lblRegistros.Text = "Registros: 0";
            
            MessageBox.Show(MensajeHistorialInfo, "Información",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            MessageBox.Show(MensajeExportarInfo, "Próximamente",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            MessageBox.Show(MensajeLimpiarInfo, "Próximamente",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lblTituloText_Click(object sender, EventArgs e) { }
    }
}
