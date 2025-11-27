using capaEntidad;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;
namespace capaDatos
{


    public class CDUsuarios
    {
        String CadenaConexion = "Server=PORTABLE-HUB\\SQLEXPRESS;Database=DBVideojuegos;Trusted_Connection=True; Encrypt=True; TrustServerCertificate=True;";

        public void Crear(CEUsuario cE)
        {
            SqlConnection sqlConnection = new SqlConnection(CadenaConexion);
            {
                sqlConnection.Open();

                string query = "INSERT INTO CEUsuario " +
                               "(NombreUsuario, Correo, ClaveHash) " +
                               "VALUES (@NombreUsuario, @Correo, @ClaveHash)";

                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    // Parámetros seguros (para evitar SQL Injection)
                    
                    sqlCommand.Parameters.AddWithValue("@NombreUsuario", cE.NombreUsuario);
                    sqlCommand.Parameters.AddWithValue("@Correo", cE.Correo);
                    sqlCommand.Parameters.AddWithValue("@ClaveHash", cE.ClaveHash);

         


                    sqlCommand.ExecuteNonQuery();
                }
            }

            MessageBox.Show("✅ El registro ha sido creado en SQL Server");
        }
        public DataSet lista()
        {
            SqlConnection SqlConnection = new SqlConnection(CadenaConexion);
            SqlConnection.Open();

            string Query = "SELECT * FROM CEUsuario";
            SqlDataAdapter Adaptador;
            DataSet dataSet = new DataSet();

            Adaptador = new SqlDataAdapter(Query, SqlConnection);
            Adaptador.Fill(dataSet, "CEUsuario");

            return dataSet;
        }
        public CEUsuario Login(string usuario, string clave)
        {
            SqlConnection cn = new SqlConnection(CadenaConexion);
            cn.Open();

            string query = @"SELECT IdUsuario, NombreUsuario, Correo 
                     FROM CEUsuario 
                     WHERE NombreUsuario = @Usuario AND ClaveHash = @Clave";

            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@Usuario", usuario);
            cmd.Parameters.AddWithValue("@Clave", clave);

            SqlDataReader dr = cmd.ExecuteReader();

            CEUsuario u = null;

            if (dr.Read())
            {
                u = new CEUsuario()
                {
                    IdUsuario = dr.GetInt32(0),
                    NombreUsuario = dr.GetString(1),
                    Correo = dr.GetString(2)
                };
            }

            cn.Close();
            return u;
        }


    }
}