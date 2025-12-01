using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGCV
{
    /// <summary>
    /// Clase estática que mantiene la información de la sesión activa del usuario
    /// </summary>
    public static class SesionActual
    {
        public static int IdUsuario { get; set; }
        public static string NombreUsuario { get; set; }
        public static string Rol { get; set; }
        
        /// <summary>
        /// Proceso de BetterJoy iniciado por la sesión actual
        /// </summary>
        public static Process ProcesoBetterJoy { get; set; }

        /// <summary>
        /// Verifica si el usuario actual es administrador
        /// </summary>
        public static bool EsAdministrador()
        {
            return Rol == "Administrador";
        }

        /// <summary>
        /// Limpia la sesión actual y cierra el proceso BetterJoy si existe
        /// </summary>
        public static void Limpiar()
        {
            IdUsuario = 0;
            NombreUsuario = string.Empty;
            Rol = string.Empty;
            
            // Cerrar BetterJoy si fue iniciado por esta sesión
            CerrarBetterJoy();
        }

        /// <summary>
        /// Cierra el proceso BetterJoy si está activo y fue iniciado por esta sesión
        /// </summary>
        public static void CerrarBetterJoy()
        {
            try
            {
                if (ProcesoBetterJoy != null && !ProcesoBetterJoy.HasExited)
                {
                    ProcesoBetterJoy.Kill();
                    ProcesoBetterJoy.WaitForExit(3000); // Esperar hasta 3 segundos
                    ProcesoBetterJoy.Dispose();
                }
            }
            catch
            {
                // Ignorar errores al cerrar el proceso
            }
            finally
            {
                ProcesoBetterJoy = null;
            }
        }
    }
}
