using capaNegocio;
using System;
using System.Data;
using System.Windows.Forms;

namespace AGCV
{
    public partial class AdministrarUsuarios : Form
    {
        private readonly CNUsuarios _cnUsuarios = new CNUsuarios();

        public AdministrarUsuarios()
        {
            InitializeComponent();
        }

        private void AdministrarUsuarios_Load(object sender, EventArgs e)
        {
            // Verificar que el usuario actual es administrador
            if (!SesionActual.EsAdministrador())
            {
                MessageBox.Show(
                    "ERROR: No tienes permisos para acceder a esta funcionalidad.\n\n" +
                    "Solo los administradores pueden administrar usuarios.",
                    "Acceso Denegado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                this.Close();
                return;
            }

            CargarUsuarios();
            ConfigurarDataGridView();
        }

        private void ConfigurarDataGridView()
        {
            dgvUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsuarios.MultiSelect = false;
            dgvUsuarios.ReadOnly = true;
            dgvUsuarios.AllowUserToAddRows = false;
            dgvUsuarios.AllowUserToDeleteRows = false;
        }

        private void CargarUsuarios()
        {
            try
            {
                DataTable usuarios = _cnUsuarios.ObtenerTodosLosUsuarios(SesionActual.IdUsuario);
                
                if (usuarios != null)
                {
                    dgvUsuarios.DataSource = usuarios;
                    
                    // Ocultar columnas innecesarias
                    if (dgvUsuarios.Columns.Contains("IdUsuario"))
                        dgvUsuarios.Columns["IdUsuario"].Visible = false;
                    if (dgvUsuarios.Columns.Contains("Activo"))
                        dgvUsuarios.Columns["Activo"].Visible = false;
                    if (dgvUsuarios.Columns.Contains("Rol"))
                        dgvUsuarios.Columns["Rol"].Visible = false;
                    
                    // Configurar encabezados
                    if (dgvUsuarios.Columns.Contains("NombreUsuario"))
                        dgvUsuarios.Columns["NombreUsuario"].HeaderText = "Nombre de Usuario";
                    if (dgvUsuarios.Columns.Contains("Correo"))
                        dgvUsuarios.Columns["Correo"].HeaderText = "Correo Electrónico";
                    if (dgvUsuarios.Columns.Contains("RolTexto"))
                        dgvUsuarios.Columns["RolTexto"].HeaderText = "Rol";
                    if (dgvUsuarios.Columns.Contains("FechaCreacion"))
                        dgvUsuarios.Columns["FechaCreacion"].HeaderText = "Fecha de Creación";
                    if (dgvUsuarios.Columns.Contains("UltimoAcceso"))
                        dgvUsuarios.Columns["UltimoAcceso"].HeaderText = "Último Acceso";
                    if (dgvUsuarios.Columns.Contains("Estado"))
                        dgvUsuarios.Columns["Estado"].HeaderText = "Estado";

                    lblTotalUsuarios.Text = $"Total de usuarios: {usuarios.Rows.Count}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"ERROR: No se pudieron cargar los usuarios:\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnCambiarContraseña_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Selecciona un usuario para cambiar su contraseña.",
                    "Información",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            int idUsuario = Convert.ToInt32(dgvUsuarios.SelectedRows[0].Cells["IdUsuario"].Value);
            string nombreUsuario = dgvUsuarios.SelectedRows[0].Cells["NombreUsuario"].Value.ToString();

            using (var formCambiarContraseña = new CambiarContraseñaAdmin(idUsuario, nombreUsuario))
            {
                if (formCambiarContraseña.ShowDialog() == DialogResult.OK)
                {
                    CargarUsuarios();
                }
            }
        }

        private void btnCambiarRol_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Selecciona un usuario para cambiar su rol.",
                    "Información",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            int idUsuario = Convert.ToInt32(dgvUsuarios.SelectedRows[0].Cells["IdUsuario"].Value);
            string nombreUsuario = dgvUsuarios.SelectedRows[0].Cells["NombreUsuario"].Value.ToString();
            string rolActual = dgvUsuarios.SelectedRows[0].Cells["Rol"].Value.ToString();

            if (idUsuario == SesionActual.IdUsuario)
            {
                MessageBox.Show(
                    "No puedes cambiar tu propio rol.",
                    "Acción no permitida",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            string nuevoRol = rolActual == "Administrador" ? "Usuario" : "Administrador";
            string mensaje = $"¿Estás seguro de cambiar el rol de '{nombreUsuario}' a '{nuevoRol}'?";

            var resultado = MessageBox.Show(mensaje, "Confirmar Cambio de Rol",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                bool exito = _cnUsuarios.CambiarRol(SesionActual.IdUsuario, idUsuario, nuevoRol);

                if (exito)
                {
                    MessageBox.Show(
                        $"EXITOSO: El rol de '{nombreUsuario}' se cambió a '{nuevoRol}'.",
                        "Rol Actualizado",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    CargarUsuarios();
                }
                else
                {
                    MessageBox.Show(
                        "ERROR: No se pudo cambiar el rol del usuario.\n\n" +
                        "Posibles razones:\n" +
                        "- Es el último administrador activo del sistema\n" +
                        "- No tienes permisos suficientes",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private void btnActivarDesactivar_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Selecciona un usuario para cambiar su estado.",
                    "Información",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            int idUsuario = Convert.ToInt32(dgvUsuarios.SelectedRows[0].Cells["IdUsuario"].Value);
            string nombreUsuario = dgvUsuarios.SelectedRows[0].Cells["NombreUsuario"].Value.ToString();
            bool estadoActual = Convert.ToBoolean(dgvUsuarios.SelectedRows[0].Cells["Activo"].Value);

            if (idUsuario == SesionActual.IdUsuario)
            {
                MessageBox.Show(
                    "No puedes desactivar tu propia cuenta.",
                    "Acción no permitida",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            string nuevoEstadoTexto = estadoActual ? "desactivar" : "activar";
            string mensaje = $"¿Estás seguro de {nuevoEstadoTexto} la cuenta de '{nombreUsuario}'?";

            var resultado = MessageBox.Show(mensaje, "Confirmar Cambio de Estado",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                bool exito = _cnUsuarios.CambiarEstadoUsuario(SesionActual.IdUsuario, idUsuario, !estadoActual);

                if (exito)
                {
                    MessageBox.Show(
                        $"EXITOSO: La cuenta de '{nombreUsuario}' fue {nuevoEstadoTexto}da correctamente.",
                        "Estado Actualizado",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    CargarUsuarios();
                }
                else
                {
                    MessageBox.Show(
                        "ERROR: No se pudo cambiar el estado del usuario.\n\n" +
                        "Posibles razones:\n" +
                        "- Es el último administrador activo del sistema\n" +
                        "- No tienes permisos suficientes",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Selecciona un usuario para eliminar.",
                    "Información",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            int idUsuario = Convert.ToInt32(dgvUsuarios.SelectedRows[0].Cells["IdUsuario"].Value);
            string nombreUsuario = dgvUsuarios.SelectedRows[0].Cells["NombreUsuario"].Value.ToString();

            if (idUsuario == SesionActual.IdUsuario)
            {
                MessageBox.Show(
                    "No puedes eliminar tu propia cuenta.",
                    "Acción no permitida",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            var resultado = MessageBox.Show(
                $"?? ADVERTENCIA: ¿Estás seguro de eliminar al usuario '{nombreUsuario}'?\n\n" +
                "Esta acción NO se puede deshacer.\n" +
                "Se eliminarán todos los datos asociados al usuario.",
                "Confirmar Eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (resultado == DialogResult.Yes)
            {
                bool exito = _cnUsuarios.EliminarUsuario(SesionActual.IdUsuario, idUsuario);

                if (exito)
                {
                    MessageBox.Show(
                        $"EXITOSO: El usuario '{nombreUsuario}' fue eliminado correctamente.",
                        "Usuario Eliminado",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    CargarUsuarios();
                }
                else
                {
                    MessageBox.Show(
                        "ERROR: No se pudo eliminar el usuario.\n\n" +
                        "Posibles razones:\n" +
                        "- Es el último administrador del sistema\n" +
                        "- No tienes permisos suficientes",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private void btnRefrescar_Click(object sender, EventArgs e)
        {
            CargarUsuarios();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
