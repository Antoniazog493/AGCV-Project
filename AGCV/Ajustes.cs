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
                // Ajustar posición de los paneles
                pnlHistorial.Location = new System.Drawing.Point(70, 35);
                pnlAdministrarUsuarios.Location = new System.Drawing.Point(430, 35);
            }
            else
            {
                pnlAdministrarUsuarios.Visible = false;
                // Centrar el panel de historial
                pnlHistorial.Location = new System.Drawing.Point(230, 35);
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
