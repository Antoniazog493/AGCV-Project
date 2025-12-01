using capaNegocio;
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
        /// Instancia del servicio de historial
        /// </summary>
        private static CNHistorial _historial = new CNHistorial();

        /// <summary>
        /// Verifica si el usuario actual es administrador
        /// </summary>
        public static bool EsAdministrador()
        {
            return Rol == "Administrador";
        }

        /// <summary>
        /// Registra una acción en el historial
        /// </summary>
        public static void RegistrarAccion(string accion, string detalles = null)
        {
            try
            {
                if (IdUsuario > 0)
                {
                    _historial.RegistrarAccion(IdUsuario, accion, detalles);
                }
            }
            catch
            {
                // Ignorar errores de registro de historial
            }
        }

        /// <summary>
        /// Registra inicio de sesión
        /// </summary>
        public static void RegistrarInicioSesion()
        {
            RegistrarAccion(CNHistorial.AccionesComunes.InicioSesion, $"Usuario: {NombreUsuario}, Rol: {Rol}");
        }

        /// <summary>
        /// Registra cierre de sesión
        /// </summary>
        public static void RegistrarCierreSesion()
        {
            RegistrarAccion(CNHistorial.AccionesComunes.CierreSesion, $"Usuario: {NombreUsuario}");
        }

        /// <summary>
        /// Registra inicio del motor AGCV
        /// </summary>
        public static void RegistrarInicioAGCV()
        {
            RegistrarAccion(CNHistorial.AccionesComunes.InicioAGCV, "Motor BetterJoy iniciado correctamente");
        }

        /// <summary>
        /// Registra cierre del motor AGCV
        /// </summary>
        public static void RegistrarCierreAGCV()
        {
            RegistrarAccion(CNHistorial.AccionesComunes.CierreAGCV, "Motor BetterJoy cerrado");
        }

        /// <summary>
        /// Registra un error
        /// </summary>
        public static void RegistrarError(string mensaje)
        {
            RegistrarAccion(CNHistorial.AccionesComunes.ErrorConexion, CNHistorial.AccionesComunes.ObtenerDetallesError(mensaje));
        }

        /// <summary>
        /// Limpia la sesión actual y cierra el proceso BetterJoy si existe
        /// </summary>
        public static void Limpiar()
        {
            // Registrar cierre de sesión antes de limpiar
            RegistrarCierreSesion();
            
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
                    // Registrar cierre de AGCV
                    RegistrarCierreAGCV();
                    
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
