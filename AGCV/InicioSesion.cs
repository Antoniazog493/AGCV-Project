using capaNegocio;
using System;
using System.Windows.Forms;

namespace AGCV
{
    public partial class LogIn : Form
    {
        private readonly CNUsuarios _cnUsuarios = new CNUsuarios();

        private const string MensajeCredencialesIncorrectas = "ERROR: Usuario o contraseña incorrectos";
        private const string MensajeRecuperarContraseña =
            "🔒 Recuperación de Contraseña\n\n" +
            "Para cambiar tu contraseña, debes contactar a un administrador del sistema.\n\n" +
            "Los administradores pueden cambiar contraseñas desde:\n" +
            "Ajustes → Administrar Usuarios\n\n";

        public LogIn()
        {
            InitializeComponent();
        }

        private void LogIn_Load(object sender, EventArgs e)
        {
            SesionActual.Limpiar();
            txtUsuario.Clear();
            txtContraseña.Clear();
            txtUsuario.Focus();
        }

        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void textBox2_TextChanged(object sender, EventArgs e) { }

        private void button1_Click(object sender, EventArgs e)
        {
            IntentoLogin();
        }

        private void IntentoLogin()
        {
            string usuario = txtUsuario.Text.Trim();
            string clave = txtContraseña.Text.Trim();

            var validacion = ValidacionService.ValidarCredencialesLogin(usuario, clave);
            if (!validacion.EsValido)
            {
                MessageBox.Show(validacion.Mensaje, validacion.Titulo,
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                if (validacion.Mensaje.Contains("nombre"))
                {
                    txtUsuario.Focus();
                }
                else
                {
                    txtContraseña.Focus();
                }
                return;
            }

            try
            {
                var datosUsuario = _cnUsuarios.Login(usuario, clave);

                if (datosUsuario == null)
                {
                    MessageBox.Show(MensajeCredencialesIncorrectas, "Error de Autenticación",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtContraseña.Clear();
                    txtContraseña.Focus();
                    return;
                }

                SesionActual.IdUsuario = datosUsuario.IdUsuario;
                SesionActual.NombreUsuario = datosUsuario.NombreUsuario;
                SesionActual.Rol = datosUsuario.Rol;

                // Registrar inicio de sesión en el historial
                SesionActual.RegistrarInicioSesion();

                string rolTexto = datosUsuario.EsAdministrador() ? "⭐ Administrador" : "👤 Usuario";
                string mensajeBienvenida = $"EXITOSO: ¡Bienvenido a AGCV, {datosUsuario.NombreUsuario}!\n\n" +
                                          $"Rol: {rolTexto}\n" +
                                          "Sistema de gestión para Joy-Cons de Nintendo Switch";

                MessageBox.Show(mensajeBienvenida, "Sesión Iniciada",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                HOME menuPrincipal = new HOME();
                menuPrincipal.FormClosed += (s, e) =>
                {
                    LimpiarFormulario();
                    this.Show();
                };
                menuPrincipal.Show();

                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"ERROR: Error al iniciar sesión:\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (var crearUsuario = new CrearUsuario())
            {
                crearUsuario.ShowDialog();
            }
        }

        private void lblOlvidaste_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show(MensajeRecuperarContraseña, "Recuperar Contraseña",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void LimpiarFormulario()
        {
            txtUsuario.Clear();
            txtContraseña.Clear();
            txtUsuario.Focus();
        }

        private void lblDescripcion_Click(object sender, EventArgs e) { }
        private void lblTitulo_Click(object sender, EventArgs e) { }

        private void panelIzquierdo_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
