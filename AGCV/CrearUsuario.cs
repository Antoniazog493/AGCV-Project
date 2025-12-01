using capaEntidad;
using capaNegocio;
using System;
using System.Windows.Forms;

namespace AGCV
{
    public partial class CrearUsuario : Form
    {
        private readonly CNUsuarios _cnUsuarios = new CNUsuarios();

        private const string MensajeConfirmarCancelacion = 
            "¿Estás seguro de que deseas cancelar?\n\nSe perderán todos los datos ingresados.";

        public CrearUsuario()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void textBox2_TextChanged(object sender, EventArgs e) { }
        private void textBox3_TextChanged(object sender, EventArgs e) { }

        private void button1_Click(object sender, EventArgs e)
        {
            var cEUsuario = new CEUsuario
            {
                NombreUsuario = txtNombre.Text.Trim(),
                Correo = txtCorreo.Text.Trim(),
                ClaveHash = txtContraseña.Text
            };

            var validacion = _cnUsuarios.ValidarDatos(cEUsuario);
            if (!validacion.EsValido)
            {
                MessageBox.Show(validacion.Mensaje, validacion.Titulo,
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                
                if (validacion.Mensaje.Contains("nombre"))
                {
                    txtNombre.Focus();
                }
                else if (validacion.Mensaje.Contains("correo"))
                {
                    txtCorreo.Focus();
                }
                else if (validacion.Mensaje.Contains("contraseña"))
                {
                    txtContraseña.Focus();
                }
                return;
            }

            try
            {
                _cnUsuarios.CrearCliente(cEUsuario);

                string mensajeExito = "EXITOSO: Cuenta creada exitosamente\n\n" +
                                     $"Usuario: {txtNombre.Text}\n" +
                                     $"Correo: {txtCorreo.Text}";

                MessageBox.Show(mensajeExito, "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

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
            var resultado = MessageBox.Show(MensajeConfirmarCancelacion,
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

        private void lblTituloText_Click(object sender, EventArgs e) { }
    }
}
