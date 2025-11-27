using capaDatos;
using capaEntidad;
using System.Data;

namespace capaNegocio
{
    public class CNHistorial
    {
        CDHistorial cdHistorial = new CDHistorial();

        public DataSet ObtenerHistorial(int idUsuario)
        {
            return cdHistorial.ObtenerHistorialPorUsuario(idUsuario);
        }

        public void RegistrarHistorial(CEHistorial h)
        {
            cdHistorial.RegistrarHistorial(h);
        }
    }
}
