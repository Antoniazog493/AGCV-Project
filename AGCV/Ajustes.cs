using System;
using System.Windows.Forms;

namespace AGCV
{
    public partial class Ajustes : Form
    {
        public Ajustes()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
            // Mostrar panel de administrar usuarios solo si es administrador
            if (SesionActual.EsAdministrador())
            {
                pnlAdministrarUsuarios.Visible = true;
                // Ambos paneles visibles lado a lado
                pnlHistorial.Location = new System.Drawing.Point(40, 50);
                pnlAdministrarUsuarios.Location = new System.Drawing.Point(420, 50);
            }
            else
            {
                pnlAdministrarUsuarios.Visible = false;
                // Centrar el panel de historial cuando está solo
                int centerX = (this.ClientSize.Width - pnlHistorial.Width) / 2;
                pnlHistorial.Location = new System.Drawing.Point(centerX, 50);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (var historial = new Historial())
            {
                historial.ShowDialog();
            }
        }

        private void btnAdministrarUsuarios_Click(object sender, EventArgs e)
        {
            using (var administrarUsuarios = new AdministrarUsuarios())
            {
                administrarUsuarios.ShowDialog();
            }
        }

        private void lblHistorialTitulo_Click(object sender, EventArgs e) { }
    }
}
