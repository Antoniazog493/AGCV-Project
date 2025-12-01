using capaDatos;
using capaEntidad;
using System.Data;

namespace capaNegocio
{
    public class CNHistorial
    {
        private readonly CDHistorial _cdHistorial = new CDHistorial();
        private readonly CDUsuarios _cdUsuarios = new CDUsuarios();

        /// <summary>
        /// Registra una acción en el historial
        /// </summary>
        public bool RegistrarAccion(int idUsuario, string accion, string detalles = null)
        {
            try
            {
                var historial = new CEHistorial
                {
                    IdUsuario = idUsuario,
                    Accion = accion,
                    Detalles = detalles,
                    FechaRegistro = DateTime.Now
                };

                return _cdHistorial.RegistrarAccion(historial);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Obtiene el historial de un usuario
        /// </summary>
        public DataTable ObtenerHistorial(int idUsuario, int? limite = null)
        {
            return _cdHistorial.ObtenerHistorialPorUsuario(idUsuario, limite);
        }

        /// <summary>
        /// Obtiene todo el historial (solo para administradores)
        /// </summary>
        public DataTable ObtenerTodoElHistorial(int idUsuarioSolicitante, int? limite = null)
        {
            // Verificar que sea administrador
            if (!_cdUsuarios.EsAdministrador(idUsuarioSolicitante))
            {
                return null;
            }

            return _cdHistorial.ObtenerTodoElHistorial(limite);
        }

        /// <summary>
        /// Obtiene estadísticas del historial de un usuario
        /// </summary>
        public Dictionary<string, int> ObtenerEstadisticas(int idUsuario)
        {
            return _cdHistorial.ObtenerEstadisticas(idUsuario);
        }

        /// <summary>
        /// Obtiene estadísticas globales del sistema (solo administradores)
        /// </summary>
        public Dictionary<string, int> ObtenerEstadisticasGlobales()
        {
            try
            {
                return _cdHistorial.ObtenerEstadisticasGlobales();
            }
            catch
            {
                return new Dictionary<string, int>();
            }
        }

        /// <summary>
        /// Limpia el historial de un usuario
        /// </summary>
        public bool LimpiarHistorial(int idUsuario)
        {
            return _cdHistorial.LimpiarHistorialUsuario(idUsuario);
        }

        /// <summary>
        /// Limpia TODO el historial del sistema (solo administradores)
        /// </summary>
        public bool LimpiarHistorialGlobal(int idUsuarioSolicitante)
        {
            // Verificar que sea administrador
            if (!_cdUsuarios.EsAdministrador(idUsuarioSolicitante))
            {
                return false;
            }

            return _cdHistorial.LimpiarTodoElHistorial();
        }

        /// <summary>
        /// Limpia registros antiguos de un usuario
        /// </summary>
        public int LimpiarHistorialAntiguo(int idUsuario, int diasAntiguedad)
        {
            return _cdHistorial.LimpiarHistorialAntiguo(idUsuario, diasAntiguedad);
        }

        /// <summary>
        /// Limpia registros antiguos de TODO el sistema (solo administradores)
        /// </summary>
        public int LimpiarHistorialGlobalAntiguo(int idUsuarioSolicitante, int diasAntiguedad)
        {
            // Verificar que sea administrador
            if (!_cdUsuarios.EsAdministrador(idUsuarioSolicitante))
            {
                return 0;
            }

            return _cdHistorial.LimpiarHistorialGlobalAntiguo(diasAntiguedad);
        }

        /// <summary>
        /// Cuenta los registros de un usuario
        /// </summary>
        public int ContarRegistros(int idUsuario)
        {
            return _cdHistorial.ContarRegistros(idUsuario);
        }

        /// <summary>
        /// Exporta el historial de un usuario a CSV
        /// </summary>
        public bool ExportarACSV(int idUsuario, string rutaArchivo)
        {
            try
            {
                var historial = ObtenerHistorial(idUsuario);
                
                using (var escritor = new System.IO.StreamWriter(rutaArchivo, false, System.Text.Encoding.UTF8))
                {
                    // Escribir encabezados
                    escritor.WriteLine("Fecha,Acción,Detalles");

                    // Escribir datos
                    foreach (DataRow fila in historial.Rows)
                    {
                        var fecha = ((DateTime)fila["FechaRegistro"]).ToString("yyyy-MM-dd HH:mm:ss");
                        var accion = fila["Accion"].ToString().Replace(",", ";").Replace("\n", " ").Replace("\r", "");
                        var detalles = (fila["Detalles"] != DBNull.Value ? fila["Detalles"].ToString() : "")
                            .Replace(",", ";").Replace("\n", " ").Replace("\r", "");

                        escritor.WriteLine($"{fecha},{accion},{detalles}");
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Exporta TODO el historial del sistema a CSV (solo administradores)
        /// </summary>
        public bool ExportarHistorialGlobalACSV(int idUsuarioSolicitante, string rutaArchivo)
        {
            try
            {
                // Verificar que sea administrador
                if (!_cdUsuarios.EsAdministrador(idUsuarioSolicitante))
                {
                    return false;
                }

                var historial = _cdHistorial.ObtenerTodoElHistorial(null); // Sin límite para exportación
                
                using (var escritor = new System.IO.StreamWriter(rutaArchivo, false, System.Text.Encoding.UTF8))
                {
                    // Escribir encabezados (incluye usuario)
                    escritor.WriteLine("Usuario,Fecha,Acción,Detalles");

                    // Escribir datos
                    foreach (DataRow fila in historial.Rows)
                    {
                        var usuario = fila["NombreUsuario"].ToString().Replace(",", ";");
                        var fecha = ((DateTime)fila["FechaRegistro"]).ToString("yyyy-MM-dd HH:mm:ss");
                        var accion = fila["Accion"].ToString().Replace(",", ";").Replace("\n", " ").Replace("\r", "");
                        var detalles = (fila["Detalles"] != DBNull.Value ? fila["Detalles"].ToString() : "")
                            .Replace(",", ";").Replace("\n", " ").Replace("\r", "");

                        escritor.WriteLine($"{usuario},{fecha},{accion},{detalles}");
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Métodos helper para acciones comunes
        /// </summary>
        public static class AccionesComunes
        {
            public const string InicioSesion = "Inicio de sesión";
            public const string CierreSesion = "Cierre de sesión";
            public const string ConexionJoyCon = "Conexión de Joy-Con";
            public const string DesconexionJoyCon = "Desconexión de Joy-Con";
            public const string InicioAGCV = "Inicio de motor AGCV";
            public const string CierreAGCV = "Cierre de motor AGCV";
            public const string ErrorConexion = "Error de conexión";
            public const string ConfiguracionCambiada = "Configuración modificada";

            public static string ObtenerDetallesJoyCon(string tipo, string serial, int bateria)
            {
                return $"Tipo: {tipo}, Serial: {serial}, Batería: {bateria}%";
            }

            public static string ObtenerDetallesError(string mensaje)
            {
                return $"Error: {mensaje}";
            }
        }
    }
}
