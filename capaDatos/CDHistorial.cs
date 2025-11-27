using capaEntidad;
using Microsoft.Data.SqlClient;
using System.Data;

namespace capaDatos
{
    public class CDHistorial
    {
        String CadenaConexion =
            "Server=DESKTOP-ME6KG8O\\SQLEXPRESS;Database=DBVideojuegos;Trusted_Connection=True; Encrypt=True; TrustServerCertificate=True;";

        // ➤ Método para obtener historial según el SP
        public DataSet ObtenerHistorialPorUsuario(int idUsuario)
        {
            SqlConnection conexion = new SqlConnection(CadenaConexion);
            conexion.Open();

            SqlCommand cmd = new SqlCommand("SP_HistorialPorUsuario", conexion);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);

            SqlDataAdapter adaptador = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adaptador.Fill(ds, "Historial");

            conexion.Close();
            return ds;
        }

        // ➤ Insertar registro de historial (cuando el usuario hace una conexión)
        public void RegistrarHistorial(CEHistorial h)
        {
            SqlConnection conexion = new SqlConnection(CadenaConexion);
            conexion.Open();

            string query = @"INSERT INTO CEHistorial (IdUsuario, IdConexion, FechaRegistro)
                             VALUES (@IdUsuario, @IdConexion, GETDATE())";

            SqlCommand cmd = new SqlCommand(query, conexion);
            cmd.Parameters.AddWithValue("@IdUsuario", h.IdUsuario);
            cmd.Parameters.AddWithValue("@IdConexion", h.IdConexion);

            cmd.ExecuteNonQuery();
            conexion.Close();
        }
    }
}
