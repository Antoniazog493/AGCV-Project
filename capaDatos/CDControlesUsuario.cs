using capaEntidad;
using Microsoft.Data.SqlClient;
using System.Data;

namespace capaDatos
{
    public class CDControlesUsuario
    {
        string cadena = "Server=PORTABLE-HUB\\SQLEXPRESS;Database=DBVideojuegos;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;";

        public DataSet ControlesPorUsuario(int idUsuario)
        {
            SqlConnection con = new SqlConnection(cadena);
            con.Open();

            SqlCommand cmd = new SqlCommand("SP_ControlesPorUsuario", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);

            SqlDataAdapter ad = new SqlDataAdapter(cmd);

            DataSet ds = new DataSet();
            ad.Fill(ds, "ControlesUsuario");

            con.Close();
            return ds;
        }
    }
}
