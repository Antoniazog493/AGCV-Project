using capaDatos;
using capaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace capaNegocio
{
    public class CNControl
    {
        CDControl cDControl = new CDControl();

        public bool validarDatos1(CEControl control)
        {
            bool validado = true;
            
            if (string.IsNullOrWhiteSpace(control.NombreControl))
            {
                MessageBox.Show("El campo nombre es obligatorio");
                validado = false;
            }
            if (string.IsNullOrWhiteSpace(control.Marca))
            {
                MessageBox.Show("El campo marca es obligatorio");
                validado = false;
            }
            if (string.IsNullOrWhiteSpace(control.Modelo))
            {
                MessageBox.Show("El campo modelo es obligatorio");
                validado = false;
            }
            return validado;
        }
        
        public void CrearControl(CEControl cE)
        {
            cDControl.Crear(cE);
        }
        
        public DataSet obtenerDatos()
        {
            return cDControl.lista1();
        }
    }
}
