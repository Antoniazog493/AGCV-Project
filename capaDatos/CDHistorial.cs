using capaEntidad;
using Microsoft.Data.SqlClient;
using System.Data;

namespace capaDatos
{
    public class CDHistorial
    {
        private readonly string CadenaConexion = "Server=PORTABLE-HUB\\SQLEXPRESS;Database=DBVideojuegos;Trusted_Connection=True; Encrypt=True; TrustServerCertificate=True;";

        /// <summary>
        /// Registra una acción en el historial
        /// </summary>
        public bool RegistrarAccion(CEHistorial historial)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(CadenaConexion))
                {
                    cn.Open();
                    string query = @"INSERT INTO CEHistorial (IdUsuario, Accion, Detalles, FechaRegistro) 
                                    VALUES (@IdUsuario, @Accion, @Detalles, @FechaRegistro)";

                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = historial.IdUsuario;
                        cmd.Parameters.Add("@Accion", SqlDbType.NVarChar, 200).Value = historial.Accion;
                        cmd.Parameters.Add("@Detalles", SqlDbType.NVarChar, -1).Value = historial.Detalles ?? (object)DBNull.Value;
                        cmd.Parameters.Add("@FechaRegistro", SqlDbType.DateTime2).Value = historial.FechaRegistro;

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Obtiene el historial de un usuario con paginación
        /// </summary>
        public DataTable ObtenerHistorialPorUsuario(int idUsuario, int? limite = null)
        {
            using (SqlConnection cn = new SqlConnection(CadenaConexion))
            {
                cn.Open();
                string query = @"SELECT TOP (@Limite)
                                    IdHistorial,
                                    IdUsuario,
                                    Accion,
                                    Detalles,
                                    FechaRegistro
                                FROM CEHistorial WITH (NOLOCK)
                                WHERE IdUsuario = @IdUsuario
                                ORDER BY FechaRegistro DESC";

                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = idUsuario;
                    cmd.Parameters.Add("@Limite", SqlDbType.Int).Value = limite ?? 1000;

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        /// <summary>
        /// Obtiene todo el historial (solo administradores)
        /// </summary>
        public DataTable ObtenerTodoElHistorial(int? limite = null)
        {
            using (SqlConnection cn = new SqlConnection(CadenaConexion))
            {
                cn.Open();
                string query = @"SELECT TOP (@Limite)
                                    H.IdHistorial,
                                    H.IdUsuario,
                                    U.NombreUsuario,
                                    H.Accion,
                                    H.Detalles,
                                    H.FechaRegistro
                                FROM CEHistorial H WITH (NOLOCK)
                                INNER JOIN CEUsuario U WITH (NOLOCK) ON H.IdUsuario = U.IdUsuario
                                ORDER BY H.FechaRegistro DESC";

                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.Add("@Limite", SqlDbType.Int).Value = limite ?? 1000;

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        /// <summary>
        /// Obtiene estadísticas del historial de un usuario
        /// </summary>
        public Dictionary<string, int> ObtenerEstadisticas(int idUsuario)
        {
            var stats = new Dictionary<string, int>();

            using (SqlConnection cn = new SqlConnection(CadenaConexion))
            {
                cn.Open();
                string query = @"SELECT 
                                    COUNT(*) AS TotalRegistros,
                                    SUM(CASE WHEN Accion LIKE '%Conexión%' THEN 1 ELSE 0 END) AS TotalConexiones,
                                    SUM(CASE WHEN Accion LIKE '%Desconexión%' THEN 1 ELSE 0 END) AS TotalDesconexiones
                                FROM CEHistorial WITH (NOLOCK)
                                WHERE IdUsuario = @IdUsuario";

                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = idUsuario;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            stats["TotalRegistros"] = reader.GetInt32(0);
                            stats["TotalConexiones"] = reader.GetInt32(1);
                            stats["TotalDesconexiones"] = reader.GetInt32(2);
                        }
                    }
                }
            }

            return stats;
        }

        /// <summary>
        /// Limpia el historial de un usuario
        /// </summary>
        public bool LimpiarHistorialUsuario(int idUsuario)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(CadenaConexion))
                {
                    cn.Open();
                    string query = "DELETE FROM CEHistorial WHERE IdUsuario = @IdUsuario";

                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = idUsuario;
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Limpia historial antiguo (más de X días)
        /// </summary>
        public int LimpiarHistorialAntiguo(int idUsuario, int diasAntiguedad)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(CadenaConexion))
                {
                    cn.Open();
                    string query = @"DELETE FROM CEHistorial 
                                    WHERE IdUsuario = @IdUsuario 
                                    AND FechaRegistro < DATEADD(DAY, -@Dias, GETDATE())";

                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = idUsuario;
                        cmd.Parameters.Add("@Dias", SqlDbType.Int).Value = diasAntiguedad;
                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Cuenta los registros de historial de un usuario
        /// </summary>
        public int ContarRegistros(int idUsuario)
        {
            using (SqlConnection cn = new SqlConnection(CadenaConexion))
            {
                cn.Open();
                string query = "SELECT COUNT(*) FROM CEHistorial WITH (NOLOCK) WHERE IdUsuario = @IdUsuario";

                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = idUsuario;
                    return (int)cmd.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// Obtiene estadísticas globales del sistema
        /// </summary>
        public Dictionary<string, int> ObtenerEstadisticasGlobales()
        {
            var stats = new Dictionary<string, int>();

            using (SqlConnection cn = new SqlConnection(CadenaConexion))
            {
                cn.Open();
                string query = @"SELECT 
                                    COUNT(*) AS TotalRegistros,
                                    COUNT(DISTINCT IdUsuario) AS UsuariosConRegistros,
                                    SUM(CASE WHEN Accion LIKE '%Conexión%' THEN 1 ELSE 0 END) AS TotalConexiones,
                                    SUM(CASE WHEN Accion LIKE '%Desconexión%' THEN 1 ELSE 0 END) AS TotalDesconexiones,
                                    SUM(CASE WHEN Accion LIKE '%Error%' THEN 1 ELSE 0 END) AS TotalErrores
                                FROM CEHistorial WITH (NOLOCK)";

                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            stats["TotalRegistros"] = reader.GetInt32(0);
                            stats["UsuariosConRegistros"] = reader.GetInt32(1);
                            stats["TotalConexiones"] = reader.GetInt32(2);
                            stats["TotalDesconexiones"] = reader.GetInt32(3);
                            stats["TotalErrores"] = reader.GetInt32(4);
                        }
                    }
                }
            }

            return stats;
        }

        /// <summary>
        /// Limpia TODO el historial del sistema (solo administradores)
        /// </summary>
        public bool LimpiarTodoElHistorial()
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(CadenaConexion))
                {
                    cn.Open();
                    string query = "DELETE FROM CEHistorial";

                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Limpia registros antiguos de TODO el sistema (solo administradores)
        /// </summary>
        public int LimpiarHistorialGlobalAntiguo(int diasAntiguedad)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(CadenaConexion))
                {
                    cn.Open();
                    string query = @"DELETE FROM CEHistorial 
                                    WHERE FechaRegistro < DATEADD(DAY, -@Dias, GETDATE())";

                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        cmd.Parameters.Add("@Dias", SqlDbType.Int).Value = diasAntiguedad;
                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch
            {
                return 0;
            }
        }
    }
}
