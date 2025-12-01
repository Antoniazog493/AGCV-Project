using capaNegocio;
using System;
using System.Windows.Forms;

namespace AGCV
{
    public partial class CambiarContraseñaAdmin : Form
    {
        private readonly CNUsuarios _cnUsuarios = new CNUsuarios();
        private readonly int _idUsuario;
        private readonly string _nombreUsuario;

        public CambiarContraseñaAdmin(int idUsuario, string nombreUsuario)
        {
            InitializeComponent();
            _idUsuario = idUsuario;
            _nombreUsuario = nombreUsuario;
        }

        private void CambiarContraseñaAdmin_Load(object sender, EventArgs e)
        {
            lblUsuario.Text = $"Cambiar contraseña de: {_nombreUsuario}";
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string nuevaContraseña = txtNuevaContraseña.Text;
            string confirmarContraseña = txtConfirmarContraseña.Text;

            var validacion = ValidacionService.ValidarContraseña(nuevaContraseña);
            if (!validacion.EsValido)
            {
                MessageBox.Show(validacion.Mensaje, validacion.Titulo,
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNuevaContraseña.Focus();
                return;
            }

            if (nuevaContraseña != confirmarContraseña)
            {
                MessageBox.Show(
                    "Las contraseñas no coinciden.\n\nPor favor, verifica que ambas contraseñas sean iguales.",
                    "Contraseñas no coinciden",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txtConfirmarContraseña.Clear();
                txtConfirmarContraseña.Focus();
                return;
            }

            try
            {
                bool exito = _cnUsuarios.CambiarContraseña(SesionActual.IdUsuario, _idUsuario, nuevaContraseña);

                if (exito)
                {
                    MessageBox.Show(
                        $"EXITOSO: La contraseña de '{_nombreUsuario}' fue cambiada correctamente.",
                        "Contraseña Actualizada",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(
                        "ERROR: No se pudo cambiar la contraseña.\n\n" +
                        "Verifica que tengas los permisos necesarios.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"ERROR: No se pudo cambiar la contraseña:\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void chkMostrarContraseña_CheckedChanged(object sender, EventArgs e)
        {
            txtNuevaContraseña.UseSystemPasswordChar = !chkMostrarContraseña.Checked;
            txtConfirmarContraseña.UseSystemPasswordChar = !chkMostrarContraseña.Checked;
        }
    }
}
