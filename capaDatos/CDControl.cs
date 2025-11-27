using capaEntidad;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace capaDatos
{
    public class CDControl
    {
        String CadenaConexion = "Server=DESKTOP-ME6KG8O\\SQLEXPRESS;Database=DBVideojuegos;Trusted_Connection=True; Encrypt=True; TrustServerCertificate=True;";

        public void Crear(CEControl cE)
        {
            SqlConnection sqlConnection = new SqlConnection(CadenaConexion);
            {
                sqlConnection.Open();

                string query = "INSERT INTO CEControl " +
                               "(NombreControl, Marca, Modelo) " +
                               "VALUES (@NombreControl, @Marca, @Modelo)";

                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    // Parámetros seguros (para evitar SQL Injection)
                    
                    sqlCommand.Parameters.AddWithValue("@NombreControl", cE.NombreControl);
                    sqlCommand.Parameters.AddWithValue("@Marca", cE.Marca);
                    sqlCommand.Parameters.AddWithValue("@Modelo", cE.Modelo);




                    sqlCommand.ExecuteNonQuery();
                }
            }

            MessageBox.Show("✅ El registro ha sido creado en SQL Server");
        }
        public DataSet lista1()
        {
            SqlConnection SqlConnection = new SqlConnection(CadenaConexion);
            SqlConnection.Open();

            string Query = "SELECT * FROM CEControl";
            SqlDataAdapter Adaptador;
            DataSet dataSet = new DataSet();

            Adaptador = new SqlDataAdapter(Query, SqlConnection);
            Adaptador.Fill(dataSet, "CEControl");

            return dataSet;
        }

    }
}

