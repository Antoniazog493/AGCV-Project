using capaEntidad;
using capaNegocio;
using System;
using System.Windows.Forms;

namespace AGCV
{
    public partial class NuevoControl : Form
    {
        private readonly CNControl _cnControl = new CNControl();
        private readonly Form _ventanaAnterior;

        public NuevoControl(Form anterior)
        {
            InitializeComponent();
            _ventanaAnterior = anterior;
        }

        public NuevoControl()
        {
            InitializeComponent();
        }

        private void NuevoControl_Load(object sender, EventArgs e)
        {
            // Establecer valores por defecto
            cmbModelo.SelectedIndex = -1;
            cmbVersion.SelectedIndex = -1;
            txtNombreControl.Focus();
        }

        private void Modelo_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void Consola_TextChanged(object sender, EventArgs e)
        {
        }

        private void NombreControl_TextChanged(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GuardarControl();
        }

        private void GuardarControl()
        {
            // Validar que todos los campos estén completos
            if (string.IsNullOrWhiteSpace(txtNombreControl.Text))
            {
                MessageBox.Show(
                    "AVISO: El nombre del Joy-Con es obligatorio\n\n" +
                    "Ejemplo: 'Joy-Con Izquierdo Rojo', 'Joy-Con Derecho Azul'",
                    "Validación",
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Warning);
                txtNombreControl.Focus();
                return;
            }

            if (cmbModelo.SelectedIndex == -1)
            {
                MessageBox.Show(
                    "AVISO: Debes seleccionar el tipo de Joy-Con\n\n" +
                    "Indica si es Joy-Con Izquierdo (L) o Derecho (R)",
                    "Validación",
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Warning);
                cmbModelo.Focus();
                return;
            }

            if (cmbVersion.SelectedIndex == -1)
            {
                MessageBox.Show(
                    "AVISO: Debes seleccionar la versión de Nintendo Switch\n\n" +
                    "Indica si es para Switch v1 o Switch v2\n" +
                    "NOTA: Esto ayuda a identificar correctamente el Joy-Con en Windows",
                    "Validación",
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Warning);
                cmbVersion.Focus();
                return;
            }

            // Crear objeto control
            var cEControl = new CEControl
            {
                NombreControl = txtNombreControl.Text.Trim(),
                Marca = cmbVersion.Text.Trim(),
                Modelo = cmbModelo.Text.Trim()
            };

            // Validar datos
            bool validado = _cnControl.validarDatos1(cEControl);
            if (!validado)
            {
                return;
            }

            try
            {
                // Guardar control
                _cnControl.CrearControl(cEControl);

                // Mostrar mensaje de éxito
                MessageBox.Show(
                    "EXITOSO: Joy-Con registrado exitosamente en AGCV\n\n" +
                    $"Nombre: {txtNombreControl.Text}\n" +
                    $"Tipo: {cmbModelo.Text}\n" +
                    $"Versión Switch: {cmbVersion.Text}\n\n" +
                    "NOTA: Ahora puedes gestionar la conexión de este Joy-Con con tu PC",
                    "Éxito",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                // Limpiar formulario
                LimpiarFormulario();

                // Cerrar diálogo
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"ERROR: Error al guardar el control:\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CancelarFormulario();
        }

        private void CancelarFormulario()
        {
            var resultado = MessageBox.Show(
                "¿Estás seguro de que deseas cancelar?\n\nSe perderán todos los datos ingresados.",
                "Confirmar cancelación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                LimpiarFormulario();
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        private void LimpiarFormulario()
        {
            txtNombreControl.Clear();
            cmbModelo.SelectedIndex = -1;
            cmbVersion.SelectedIndex = -1;
            txtNombreControl.Focus();
        }

        private void lblTituloText_Click(object sender, EventArgs e)
        {
        }

        private void lblTituloText_Click_1(object sender, EventArgs e)
        {
        }
    }
}
