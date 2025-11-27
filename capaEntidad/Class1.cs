namespace capaEntidad
{
    public class Class1
    {
    }
    
    /// <summary>
    /// Entidad que representa un Joy-Con de Nintendo Switch
    /// </summary>
    public class CEControl
    {
        public int IdControl { get; set; }
        public string NombreControl { get; set; }  // Ej: "Joy-Con Izquierdo Rojo"
        public string Marca { get; set; }          // Versión de Switch: "Switch v1" o "Switch v2"
        public string Modelo { get; set; }         // Tipo: "Joy-Con L (Izquierdo)" o "Joy-Con R (Derecho)"
    }

    /// <summary>
    /// Entidad que representa una consola Nintendo Switch
    /// </summary>
    public class CEConsola
    {
        public int IdConsola { get; set; }
        public string Nombre { get; set; }       // Ej: Switch v1, Switch v2, Switch OLED
        public string Fabricante { get; set; }   // Nintendo
    }

    /// <summary>
    /// Entidad que representa una conexión entre un Joy-Con y la PC
    /// </summary>
    public class CEConexion
    {
        public int IdConexion { get; set; }
        public int IdControl { get; set; }
        public int IdConsola { get; set; }
        public DateTime FechaConexion { get; set; }
        public string Estado { get; set; }       // Ej: Conectado, Desconectado, Error de reconocimiento
    }
    
    /// <summary>
    /// Entidad que representa un usuario del sistema AGCV
    /// </summary>
    public class CEUsuario
    {
        public int IdUsuario { get; set; }        // Identificador único
        public string NombreUsuario { get; set; } // Alias o username
        public string Correo { get; set; }        // Email de registro
        public string ClaveHash { get; set; }     // Contraseña encriptada
    }
    
    /// <summary>
    /// Entidad que representa el historial de conexiones de Joy-Cons
    /// </summary>
    public class CEHistorial
    {
        public int IdHistorial { get; set; }
        public int IdUsuario { get; set; }
        public int IdConexion { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}

