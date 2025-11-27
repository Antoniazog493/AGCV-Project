using capaDatos;
using System.Data;

namespace capaNegocio
{
    public class CNControlesUsuario
    {
        CDControlesUsuario cd = new CDControlesUsuario();

        public DataSet ObtenerControles(int idUsuario)
        {
            return cd.ControlesPorUsuario(idUsuario);
        }
    }
}
