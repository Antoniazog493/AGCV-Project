using System.Text.RegularExpressions;

namespace capaNegocio
{
    /// <summary>
    /// Servicio centralizado para validaciones de datos
    /// </summary>
    public static class ValidacionService
    {
        public const int LongitudMinimaContraseña = 6;
        private static readonly Regex RegexEmail = new Regex(
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Resultado de una validación
        /// </summary>
        public class ResultadoValidacion
        {
            public bool EsValido { get; set; }
            public string Mensaje { get; set; }
            public string Titulo { get; set; }

            public ResultadoValidacion(bool esValido, string mensaje = "", string titulo = "")
            {
                EsValido = esValido;
                Mensaje = mensaje;
                Titulo = titulo;
            }

            public static ResultadoValidacion Exitoso() => new ResultadoValidacion(true);
        }

        /// <summary>
        /// Valida que un campo no esté vacío
        /// </summary>
        public static ResultadoValidacion ValidarCampoRequerido(string valor, string nombreCampo)
        {
            if (string.IsNullOrWhiteSpace(valor))
            {
                return new ResultadoValidacion(
                    false,
                    $"El campo {nombreCampo} es obligatorio",
                    "Validación");
            }
            return ResultadoValidacion.Exitoso();
        }

        /// <summary>
        /// Valida el formato de un correo electrónico
        /// </summary>
        public static ResultadoValidacion ValidarEmail(string email)
        {
            var validacionRequerido = ValidarCampoRequerido(email, "correo");
            if (!validacionRequerido.EsValido)
            {
                return validacionRequerido;
            }

            if (!RegexEmail.IsMatch(email))
            {
                return new ResultadoValidacion(
                    false,
                    "Ingresa un correo válido (ejemplo@correo.com)",
                    "Validación");
            }

            return ResultadoValidacion.Exitoso();
        }

        /// <summary>
        /// Valida la longitud mínima de una contraseña
        /// </summary>
        public static ResultadoValidacion ValidarContraseña(string contraseña)
        {
            var validacionRequerido = ValidarCampoRequerido(contraseña, "contraseña");
            if (!validacionRequerido.EsValido)
            {
                return validacionRequerido;
            }

            if (contraseña.Length < LongitudMinimaContraseña)
            {
                return new ResultadoValidacion(
                    false,
                    $"La contraseña debe tener mínimo {LongitudMinimaContraseña} caracteres",
                    "Validación");
            }

            return ResultadoValidacion.Exitoso();
        }

        /// <summary>
        /// Valida el nombre de usuario
        /// </summary>
        public static ResultadoValidacion ValidarNombreUsuario(string nombreUsuario)
        {
            return ValidarCampoRequerido(nombreUsuario, "nombre de usuario");
        }

        /// <summary>
        /// Valida un rol de usuario
        /// </summary>
        public static ResultadoValidacion ValidarRol(string rol)
        {
            if (rol != "Usuario" && rol != "Administrador")
            {
                return new ResultadoValidacion(
                    false,
                    "Rol inválido. Solo se permite: Usuario o Administrador",
                    "Error");
            }
            return ResultadoValidacion.Exitoso();
        }

        /// <summary>
        /// Valida credenciales de login
        /// </summary>
        public static ResultadoValidacion ValidarCredencialesLogin(string usuario, string contraseña)
        {
            var validacionUsuario = ValidarCampoRequerido(usuario, "nombre de usuario");
            if (!validacionUsuario.EsValido)
            {
                return validacionUsuario;
            }

            var validacionContraseña = ValidarCampoRequerido(contraseña, "contraseña");
            if (!validacionContraseña.EsValido)
            {
                return validacionContraseña;
            }

            return ResultadoValidacion.Exitoso();
        }
    }
}
