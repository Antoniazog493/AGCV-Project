namespace capaEntidad
{
    /// <summary>
    /// Entidad que representa un usuario del sistema AGCV
    /// </summary>
    public class CEUsuario
    {
        public int IdUsuario { get; set; }        // Identificador único
        public string NombreUsuario { get; set; } // Alias o username
        public string Correo { get; set; }        // Email de registro
        public string ClaveHash { get; set; }     // Contraseña encriptada
        public string Rol { get; set; }           // Rol: "Usuario" o "Administrador"
        public DateTime FechaCreacion { get; set; } // Fecha de creación de la cuenta
        public DateTime? UltimoAcceso { get; set; } // Último acceso al sistema
        public bool Activo { get; set; }          // Estado de la cuenta

        /// <summary>
        /// Verifica si el usuario es administrador
        /// </summary>
        public bool EsAdministrador() => Rol == "Administrador";

        /// <summary>
        /// Obtiene un texto descriptivo del rol
        /// </summary>
        public string ObtenerRolTexto()
        {
            return Rol == "Administrador" ? "⭐ Administrador" : "👤 Usuario";
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public CEUsuario()
        {
            Rol = "Usuario"; // Rol por defecto
            FechaCreacion = DateTime.Now;
            Activo = true;
        }
    }
    
    /// <summary>
    /// Entidad que representa el historial de acciones del usuario
    /// </summary>
    public class CEHistorial
    {
        public int IdHistorial { get; set; }
        public int IdUsuario { get; set; }
        public string Accion { get; set; }        // Ej: "Inicio de sesión", "Conexión de Joy-Con"
        public string Detalles { get; set; }      // Detalles adicionales de la acción
        public DateTime FechaRegistro { get; set; }
    }
}

