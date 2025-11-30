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

        private void button2_Click(object sender, EventArgs e)
        {
            // Abrir formulario de Historial
            using (var historial = new Historial())
            {
                historial.ShowDialog();
            }
        }

        private void lblHistorialTitulo_Click(object sender, EventArgs e)
        {
        }
    }
}
