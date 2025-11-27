using capaEntidad;
using capaNegocio;
using System;
using System.Windows.Forms;

namespace AGCV
{
    public partial class CrearUsuario : Form
    {
        private readonly CNUsuarios _cnUsuarios = new CNUsuarios();

        public CrearUsuario()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // Validación en tiempo real (opcional)
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            // Validación en tiempo real (opcional)
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            // Validación en tiempo real (opcional)
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Validaciones previas
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El nombre de usuario es obligatorio", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtCorreo.Text))
            {
                MessageBox.Show("El correo es obligatorio", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCorreo.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtContraseña.Text))
            {
                MessageBox.Show("La contraseña es obligatoria", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtContraseña.Focus();
                return;
            }

            // Validar formato de correo básico
            if (!txtCorreo.Text.Contains("@") || !txtCorreo.Text.Contains("."))
            {
                MessageBox.Show("Ingresa un correo válido (ejemplo@correo.com)", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCorreo.Focus();
                return;
            }

            // Validar longitud de contraseña
            if (txtContraseña.Text.Length < 6)
            {
                MessageBox.Show("La contraseña debe tener mínimo 6 caracteres", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtContraseña.Focus();
                return;
            }

            // Crear objeto usuario
            var cEUsuario = new CEUsuario
            {
                NombreUsuario = txtNombre.Text.Trim(),
                Correo = txtCorreo.Text.Trim(),
                ClaveHash = txtContraseña.Text
            };

            // Validar con la lógica existente
            bool validado = _cnUsuarios.validarDatos(cEUsuario);
            if (!validado)
            {
                return;
            }

            try
            {
                // Guardar usuario
                _cnUsuarios.CrearCliente(cEUsuario);

                // Mostrar mensaje de éxito
                MessageBox.Show(
                    "EXITOSO: Cuenta creada exitosamente\n\n" +
                    $"Usuario: {txtNombre.Text}\n" +
                    $"Correo: {txtCorreo.Text}",
                    "Éxito",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                // Limpiar y cerrar
                LimpiarFormulario();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"ERROR: Error al crear la cuenta:\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            var resultado = MessageBox.Show(
                "¿Estás seguro de que deseas cancelar?\n\nSe perderán todos los datos ingresados.",
                "Confirmar cancelación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                LimpiarFormulario();
                this.Close();
            }
        }

        private void LimpiarFormulario()
        {
            txtNombre.Clear();
            txtCorreo.Clear();
            txtContraseña.Clear();
            txtNombre.Focus();
        }

        private void lblTituloText_Click(object sender, EventArgs e)
        {

        }
    }
}
