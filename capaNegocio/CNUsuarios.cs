using capaDatos;
using capaEntidad;
using System.Data;
using System.Windows.Forms;

namespace capaNegocio
{
    public class CNUsuarios
    {
        CDUsuarios cDUsuario = new CDUsuarios();

        public bool validarDatos(CEUsuario usuario)
        {
            bool validado = true;
            
            if (string.IsNullOrWhiteSpace(usuario.NombreUsuario))
            {
                MessageBox.Show("El campo nombre es obligatorio");
                validado = false;
            }
            if (string.IsNullOrWhiteSpace(usuario.Correo))
            {
                MessageBox.Show("El campo correo es obligatorio");
                validado = false;
            }
            if (string.IsNullOrWhiteSpace(usuario.ClaveHash))
            {
                MessageBox.Show("La clave es obligatoria");
                validado = false;
            }
            return validado;
        }
        
        public void CrearCliente(CEUsuario cE)
        {
            cDUsuario.Crear(cE);
        }
        
        public DataSet obtenerDatos()
        {
            return cDUsuario.lista();
        }
        
        public CEUsuario Login(string usuario, string clave)
        {
            return cDUsuario.Login(usuario, clave);
        }
    }
}