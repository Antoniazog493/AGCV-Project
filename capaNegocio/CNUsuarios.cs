using capaDatos;
using capaEntidad;
using System.Data;

namespace capaNegocio
{
    public class CNUsuarios
    {
        private readonly CDUsuarios cDUsuario = new CDUsuarios();

        /// <summary>
        /// Valida los datos de un usuario
        /// </summary>
        public ValidacionService.ResultadoValidacion ValidarDatos(CEUsuario usuario)
        {
            var validacionNombre = ValidacionService.ValidarNombreUsuario(usuario.NombreUsuario);
            if (!validacionNombre.EsValido)
            {
                return validacionNombre;
            }

            var validacionCorreo = ValidacionService.ValidarEmail(usuario.Correo);
            if (!validacionCorreo.EsValido)
            {
                return validacionCorreo;
            }

            var validacionClave = ValidacionService.ValidarContraseña(usuario.ClaveHash);
            if (!validacionClave.EsValido)
            {
                return validacionClave;
            }

            return ValidacionService.ResultadoValidacion.Exitoso();
        }
        
        public void CrearCliente(CEUsuario cE)
        {
            cDUsuario.Crear(cE);
        }
        
        public DataSet obtenerDatos()
        {
            return cDUsuario.lista();
        }
        
        public CEUsuario Login(string usuario, string clave)
        {
            return cDUsuario.Login(usuario, clave);
        }

        public DataTable ObtenerTodosLosUsuarios(int idUsuarioSolicitante)
        {
            if (!cDUsuario.EsAdministrador(idUsuarioSolicitante))
            {
                return null;
            }

            return cDUsuario.ObtenerTodosLosUsuarios();
        }

        public bool CambiarRol(int idUsuarioSolicitante, int idUsuarioObjetivo, string nuevoRol)
        {
            if (!cDUsuario.EsAdministrador(idUsuarioSolicitante))
            {
                return false;
            }

            var validacionRol = ValidacionService.ValidarRol(nuevoRol);
            if (!validacionRol.EsValido)
            {
                return false;
            }

            if (nuevoRol == "Usuario")
            {
                CEUsuario usuarioObjetivo = cDUsuario.ObtenerPorId(idUsuarioObjetivo);
                if (usuarioObjetivo != null && usuarioObjetivo.Rol == "Administrador")
                {
                    int cantidadAdmins = cDUsuario.ContarAdministradoresActivos();
                    if (cantidadAdmins <= 1)
                    {
                        return false;
                    }
                }
            }

            return cDUsuario.ActualizarRol(idUsuarioObjetivo, nuevoRol);
        }

        public bool CambiarEstadoUsuario(int idUsuarioSolicitante, int idUsuarioObjetivo, bool nuevoEstado)
        {
            if (!cDUsuario.EsAdministrador(idUsuarioSolicitante))
            {
                return false;
            }

            if (!nuevoEstado)
            {
                CEUsuario usuarioObjetivo = cDUsuario.ObtenerPorId(idUsuarioObjetivo);
                if (usuarioObjetivo != null && usuarioObjetivo.Rol == "Administrador" && usuarioObjetivo.Activo)
                {
                    int cantidadAdmins = cDUsuario.ContarAdministradoresActivos();
                    if (cantidadAdmins <= 1)
                    {
                        return false;
                    }
                }
            }

            return cDUsuario.CambiarEstadoUsuario(idUsuarioObjetivo, nuevoEstado);
        }

        public bool EsAdministrador(int idUsuario)
        {
            return cDUsuario.EsAdministrador(idUsuario);
        }

        public bool CambiarContraseña(int idUsuarioSolicitante, int idUsuarioObjetivo, string nuevaContraseña)
        {
            // Verificar que el solicitante es administrador
            if (!cDUsuario.EsAdministrador(idUsuarioSolicitante))
            {
                return false;
            }

            // Validar la nueva contraseña
            var validacion = ValidacionService.ValidarContraseña(nuevaContraseña);
            if (!validacion.EsValido)
            {
                return false;
            }

            // Cambiar la contraseña (en un sistema real aquí se debería hashear)
            return cDUsuario.CambiarContraseña(idUsuarioObjetivo, nuevaContraseña);
        }

        public bool EliminarUsuario(int idUsuarioSolicitante, int idUsuarioObjetivo)
        {
            // Verificar que el solicitante es administrador
            if (!cDUsuario.EsAdministrador(idUsuarioSolicitante))
            {
                return false;
            }

            // No permitir eliminar su propia cuenta
            if (idUsuarioSolicitante == idUsuarioObjetivo)
            {
                return false;
            }

            // Verificar si es el último administrador
            CEUsuario usuarioObjetivo = cDUsuario.ObtenerPorId(idUsuarioObjetivo);
            if (usuarioObjetivo != null && usuarioObjetivo.Rol == "Administrador")
            {
                int cantidadAdmins = cDUsuario.ContarAdministradoresActivos();
                if (cantidadAdmins <= 1)
                {
                    // No permitir eliminar el último administrador
                    return false;
                }
            }

            // Eliminar el usuario
            return cDUsuario.EliminarUsuario(idUsuarioObjetivo);
        }
    }
}