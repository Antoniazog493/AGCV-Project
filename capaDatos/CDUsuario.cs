using capaEntidad;
using Microsoft.Data.SqlClient;
using System.Data;

namespace capaDatos
{
    public class CDUsuarios
    {
        private readonly string CadenaConexion = "Server=PORTABLE-HUB\\SQLEXPRESS;Database=DBVideojuegos;Trusted_Connection=True; Encrypt=True; TrustServerCertificate=True;";

        public void Crear(CEUsuario cE)
        {
            using (SqlConnection sqlConnection = new SqlConnection(CadenaConexion))
            {
                sqlConnection.Open();

                string query = "INSERT INTO CEUsuario " +
                               "(NombreUsuario, Correo, ClaveHash, Rol, FechaCreacion, Activo) " +
                               "VALUES (@NombreUsuario, @Correo, @ClaveHash, @Rol, @FechaCreacion, @Activo)";

                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.Add("@NombreUsuario", SqlDbType.NVarChar, 100).Value = cE.NombreUsuario;
                    sqlCommand.Parameters.Add("@Correo", SqlDbType.NVarChar, 150).Value = cE.Correo;
                    sqlCommand.Parameters.Add("@ClaveHash", SqlDbType.NVarChar, 256).Value = cE.ClaveHash;
                    sqlCommand.Parameters.Add("@Rol", SqlDbType.NVarChar, 50).Value = string.IsNullOrEmpty(cE.Rol) ? "Usuario" : cE.Rol;
                    sqlCommand.Parameters.Add("@FechaCreacion", SqlDbType.DateTime2).Value = cE.FechaCreacion;
                    sqlCommand.Parameters.Add("@Activo", SqlDbType.Bit).Value = cE.Activo;

                    sqlCommand.ExecuteNonQuery();
                }
            }
        }

        public DataSet lista()
        {
            using (SqlConnection sqlConnection = new SqlConnection(CadenaConexion))
            {
                sqlConnection.Open();

                string query = "SELECT IdUsuario, NombreUsuario, Correo, Rol, FechaCreacion, UltimoAcceso, Activo FROM CEUsuario";
                
                using (SqlDataAdapter adaptador = new SqlDataAdapter(query, sqlConnection))
                {
                    DataSet dataSet = new DataSet();
                    adaptador.Fill(dataSet, "CEUsuario");
                    return dataSet;
                }
            }
        }

        public CEUsuario Login(string usuario, string clave)
        {
            using (SqlConnection cn = new SqlConnection(CadenaConexion))
            {
                cn.Open();

                using (SqlTransaction transaction = cn.BeginTransaction())
                {
                    try
                    {
                        string queryLogin = @"SELECT IdUsuario, NombreUsuario, Correo, Rol, Activo 
                                            FROM CEUsuario WITH (NOLOCK)
                                            WHERE NombreUsuario = @Usuario 
                                            AND ClaveHash = @Clave 
                                            AND Activo = 1";

                        using (SqlCommand cmd = new SqlCommand(queryLogin, cn, transaction))
                        {
                            cmd.Parameters.Add("@Usuario", SqlDbType.NVarChar, 100).Value = usuario;
                            cmd.Parameters.Add("@Clave", SqlDbType.NVarChar, 256).Value = clave;

                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                if (dr.Read())
                                {
                                    CEUsuario u = new CEUsuario()
                                    {
                                        IdUsuario = dr.GetInt32(0),
                                        NombreUsuario = dr.GetString(1),
                                        Correo = dr.GetString(2),
                                        Rol = dr.GetString(3),
                                        Activo = dr.GetBoolean(4)
                                    };

                                    dr.Close();

                                    string queryUpdate = "UPDATE CEUsuario SET UltimoAcceso = GETDATE() WHERE IdUsuario = @IdUsuario";
                                    using (SqlCommand cmdUpdate = new SqlCommand(queryUpdate, cn, transaction))
                                    {
                                        cmdUpdate.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = u.IdUsuario;
                                        cmdUpdate.ExecuteNonQuery();
                                    }

                                    transaction.Commit();
                                    return u;
                                }
                            }
                        }

                        transaction.Rollback();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            
            return null;
        }

        public DataTable ObtenerTodosLosUsuarios()
        {
            using (SqlConnection cn = new SqlConnection(CadenaConexion))
            {
                cn.Open();
                string query = @"SELECT 
                                    IdUsuario,
                                    NombreUsuario,
                                    Correo,
                                    Rol,
                                    FechaCreacion,
                                    UltimoAcceso,
                                    Activo
                                FROM CEUsuario WITH (NOLOCK)
                                ORDER BY FechaCreacion DESC";

                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        
                        dt.Columns.Add("Estado", typeof(string));
                        dt.Columns.Add("RolTexto", typeof(string));
                        
                        foreach (DataRow row in dt.Rows)
                        {
                            row["Estado"] = (bool)row["Activo"] ? "Activo" : "Inactivo";
                            row["RolTexto"] = row["Rol"].ToString() == "Administrador" ? "⭐ Administrador" : "👤 Usuario";
                        }
                        
                        return dt;
                    }
                }
            }
        }

        public bool ActualizarRol(int idUsuario, string nuevoRol)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(CadenaConexion))
                {
                    cn.Open();
                    string query = "UPDATE CEUsuario SET Rol = @Rol WHERE IdUsuario = @IdUsuario";

                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        cmd.Parameters.Add("@Rol", SqlDbType.NVarChar, 50).Value = nuevoRol;
                        cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = idUsuario;

                        int affected = cmd.ExecuteNonQuery();
                        return affected > 0;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public bool CambiarEstadoUsuario(int idUsuario, bool nuevoEstado)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(CadenaConexion))
                {
                    cn.Open();
                    string query = "UPDATE CEUsuario SET Activo = @Activo WHERE IdUsuario = @IdUsuario";

                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        cmd.Parameters.Add("@Activo", SqlDbType.Bit).Value = nuevoEstado;
                        cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = idUsuario;

                        int affected = cmd.ExecuteNonQuery();
                        return affected > 0;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public bool EsAdministrador(int idUsuario)
        {
            using (SqlConnection cn = new SqlConnection(CadenaConexion))
            {
                cn.Open();
                string query = "SELECT Rol FROM CEUsuario WITH (NOLOCK) WHERE IdUsuario = @IdUsuario AND Activo = 1";

                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = idUsuario;
                    object result = cmd.ExecuteScalar();

                    return result != null && result.ToString() == "Administrador";
                }
            }
        }

        public int ContarAdministradoresActivos()
        {
            using (SqlConnection cn = new SqlConnection(CadenaConexion))
            {
                cn.Open();
                string query = "SELECT COUNT(*) FROM CEUsuario WITH (NOLOCK) WHERE Rol = @Rol AND Activo = 1";

                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.Add("@Rol", SqlDbType.NVarChar, 50).Value = "Administrador";
                    return (int)cmd.ExecuteScalar();
                }
            }
        }

        public CEUsuario ObtenerPorId(int idUsuario)
        {
            using (SqlConnection cn = new SqlConnection(CadenaConexion))
            {
                cn.Open();
                string query = @"SELECT IdUsuario, NombreUsuario, Correo, Rol, FechaCreacion, UltimoAcceso, Activo 
                               FROM CEUsuario WITH (NOLOCK)
                               WHERE IdUsuario = @IdUsuario";

                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = idUsuario;
                    
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            return new CEUsuario
                            {
                                IdUsuario = dr.GetInt32(0),
                                NombreUsuario = dr.GetString(1),
                                Correo = dr.GetString(2),
                                Rol = dr.GetString(3),
                                FechaCreacion = dr.GetDateTime(4),
                                UltimoAcceso = dr.IsDBNull(5) ? null : dr.GetDateTime(5),
                                Activo = dr.GetBoolean(6)
                            };
                        }
                    }
                }
            }
            return null;
        }

        public bool CambiarContraseña(int idUsuario, string nuevaClaveHash)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(CadenaConexion))
                {
                    cn.Open();
                    string query = "UPDATE CEUsuario SET ClaveHash = @ClaveHash WHERE IdUsuario = @IdUsuario";

                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        cmd.Parameters.Add("@ClaveHash", SqlDbType.NVarChar, 256).Value = nuevaClaveHash;
                        cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = idUsuario;

                        int affected = cmd.ExecuteNonQuery();
                        return affected > 0;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public bool EliminarUsuario(int idUsuario)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(CadenaConexion))
                {
                    cn.Open();
                    
                    using (SqlTransaction transaction = cn.BeginTransaction())
                    {
                        try
                        {
                            // Primero eliminar registros relacionados en el historial
                            string queryHistorial = "DELETE FROM CEHistorial WHERE IdUsuario = @IdUsuario";
                            using (SqlCommand cmdHistorial = new SqlCommand(queryHistorial, cn, transaction))
                            {
                                cmdHistorial.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = idUsuario;
                                cmdHistorial.ExecuteNonQuery();
                            }
                            
                            // Luego eliminar el usuario
                            string queryUsuario = "DELETE FROM CEUsuario WHERE IdUsuario = @IdUsuario";
                            using (SqlCommand cmdUsuario = new SqlCommand(queryUsuario, cn, transaction))
                            {
                                cmdUsuario.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = idUsuario;
                                int affected = cmdUsuario.ExecuteNonQuery();
                                
                                if (affected > 0)
                                {
                                    transaction.Commit();
                                    return true;
                                }
                                else
                                {
                                    transaction.Rollback();
                                    return false;
                                }
                            }
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch
            {
                return false;
            }
        }
    }
}