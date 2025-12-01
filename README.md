<p align="center">
  <img src="title.png">
</p>

# AGCV - Administrador Gráfico de Controles Virtuales

## Información del Proyecto

**Proyecto Final - Herramientas de Programación Avanzada III**

**Universidad Tecnológica de Panamá**  
**Salón:** 1IL131  
**Año:** 2025

### Integrantes del Equipo

- Roberto Di Maso
- Hassan Elrada
- Nikechi Camarena
- Liam Thompson
- Samir Smith

## Descripción General

AGCV es un sistema integral de gestión y administración de controles virtuales para Nintendo Switch, diseñado específicamente para la compatibilidad con PC mediante Windows. El proyecto combina una interfaz gráfica de usuario moderna con un motor de controladores optimizado basado en BetterJoy, proporcionando una experiencia completa de gestión de Joy-Cons.

El sistema permite a los usuarios conectar sus Joy-Cons de Nintendo Switch a través de Bluetooth o USB, utilizándolos como controladores compatibles con XInput y DualShock4 para juegos de PC, emuladores y otras aplicaciones.

## Motivación del Proyecto

BetterJoy ha sido durante años una herramienta fundamental para usuarios que desean utilizar sus controles de Nintendo Switch en PC. Sin embargo, el proyecto original presenta varias limitaciones:

- Falta de soporte activo desde hace varios años
- Interfaz de usuario básica sin funciones de administración
- Problemas de latencia documentados, especialmente con el Joy-Con izquierdo
- Errores de "duplicate timestamp" que afectan la detección en juegos
- Ausencia de un sistema de gestión de usuarios y historial
- No incluye HidHide para ocultar dispositivos duplicados

Ante estas limitaciones, decidimos no solo actualizar el motor de BetterJoy, sino crear una aplicación completa que lo integre con funcionalidades modernas de administración y gestión.

## Arquitectura del Sistema

AGCV está construido siguiendo una arquitectura de capas que separa claramente las responsabilidades:

### Componentes Principales

**1. Motor BetterJoy (Actualizado)**
- Motor de bajo nivel para comunicación con Joy-Cons
- Implementación de emulación XInput y DualShock4 mediante ViGEmBus
- Servidor UDP para compatibilidad con emuladores (Cemu, Citra, Dolphin, Yuzu)
- Integración con HidHide para ocultar dispositivos duplicados
- Optimizaciones de latencia mediante sincronización de pares y predicción

**2. Aplicación de Gestión (AGCV)**
- **Capa de Presentación (capaPresentacion):** Interfaces gráficas Windows Forms
- **Capa de Negocio (capaNegocio):** Lógica de aplicación y validaciones
- **Capa de Datos (capaDatos):** Acceso a base de datos SQLite
- **Capa de Entidad (capaEntidad):** Modelos de datos y objetos de transferencia

### Base de Datos

Sistema de gestión basado en SQL con tres tablas principales:

- **Usuarios:** Gestión de cuentas con roles (Administrador/Usuario)
- **Historial:** Registro de acciones y eventos del sistema
- **Configuración:** Almacenamiento de preferencias y calibración

## Mejoras Implementadas sobre BetterJoy Original

### Correcciones Críticas

**Eliminación de Errores de "Duplicate Timestamp"**
- Problema: Los hooks globales de teclado/mouse interferían con el polling Bluetooth de alta frecuencia
- Solución: Eliminación de hooks globales en Program.cs, manteniendo solo hooks locales temporales en el formulario de remapeo
- Impacto: Eliminación completa del error, mejor detección por juegos, menor latencia

**Reducción de Latencia del Joy-Con Izquierdo**
- Implementación de JoyconPairSynchronizer para sincronización asimétrica
- Predicción de posiciones del stick para compensar diferencias de timing
- Prioridad de hilo elevada (ThreadPriority.AboveNormal) para el polling
- Polling no bloqueante con SpinWait para reducción de microsegundos

**Integración de HidHide**
- Detección automática del driver HidHide
- Ocultación selectiva de dispositivos para evitar entrada duplicada
- Gestión de whitelist automática para AGCV
- Cleanup completo al cerrar la aplicación

### Nuevas Funcionalidades

**Sistema de Gestión de Usuarios**
- Autenticación con roles (Administrador/Usuario)
- Administradores pueden gestionar usuarios, cambiar roles y contraseñas
- Protección contra eliminación del último administrador
- Historial completo de acciones por usuario

**Monitor de Eventos**
- Vista en tiempo real de eventos de botones y sticks
- Visualización de datos del giroscopio y acelerómetro
- Herramienta de diagnóstico para calibración

**Historial de Actividad**
- Registro automático de inicios de sesión
- Registro de conexión/desconexión de controles
- Registro de errores y eventos del sistema
- Filtrado por usuario y tipo de evento

**Mejoras de Interfaz**
- Diseño moderno y limpio siguiendo las guías de Windows
- Mensajes descriptivos en español
- Indicadores visuales de estado de batería
- Sistema de notificaciones en bandeja

## Requisitos del Sistema

### Hardware
- Adaptador Bluetooth 4.0 o superior (para conexión inalámbrica)
- Puerto USB (para conexión por cable)
- Joy-Cons de Nintendo Switch, Pro Controller, o controles compatibles

### Software
- Windows 10/11 (64 bits)
- .NET 8 Runtime
- ViGEmBus Driver (incluido en instalación)
- HidHide Driver (opcional, recomendado)

## Instalación

### Compilación desde Código Fuente

**Requisitos previos:**
- Visual Studio 2022 con .NET 8 SDK
- Git

### Funciones Principales

**Emparejar Joy-Cons**
- Haga clic en el icono de un Joy-Con conectado
- Se emparejará automáticamente con el Joy-Con opuesto si está disponible
- Una vez emparejados, funcionarán como un solo control

**Configuración de Botones**
- Acceda a "Reasignar Botones" desde el menú
- Haga clic izquierdo en un botón para detectar entrada
- Haga clic central para limpiar asignación
- Haga clic derecho para opciones avanzadas

**Giroscopio y Mouse**
- Configure active_gyro para activar el giroscopio
- Use reset_mouse para recentrar el cursor
- Ajuste sensibilidad en la configuración

## Configuración Avanzada

### Archivo de Configuración

La configuración se almacena en BetterJoyForCemu.exe.config. Algunas opciones importantes:

**Configuración de Entrada**
- ShowAsXInput: Mostrar como control Xbox 360 (true/false)
- ShowAsDS4: Mostrar como DualShock 4 (true/false)
- EnableRumble: Activar vibración (true/false)

**Configuración de Giroscopio**
- GyroToJoyOrMouse: Modo de giroscopio (joy_left/joy_right/mouse)
- GyroMouseSensitivityX: Sensibilidad horizontal del mouse
- GyroMouseSensitivityY: Sensibilidad vertical del mouse

**HidHide**
- UseHIDHide: Activar ocultación de dispositivos (true/false)
- PurgeAffectedDevices: Limpiar lista al cerrar (true/false)

**Nota:** Cambios en configuración marcados en naranja requieren reinicio de la aplicación.

## Roles y Permisos

### Usuario Regular
- Conectar y usar Joy-Cons
- Ver su propio historial
- Cambiar configuración personal
- Usar monitor de eventos

### Administrador
- Todas las funciones de Usuario
- Administrar otros usuarios
- Cambiar roles y contraseñas
- Ver historial global
- Gestionar configuración del sistema
- Acceso a herramientas de diagnóstico

**Nota:** El sistema siempre mantiene al menos un administrador activo.

## Compatibilidad

### Controles Soportados
- Joy-Con L/R (individuales o emparejados)
- Nintendo Switch Pro Controller
- SNES Controller (Nintendo Switch Online)
- N64 Controller (Nintendo Switch Online)
- Controles de terceros compatibles (configurable)

### Juegos y Emuladores
- Juegos de Steam con soporte XInput
- Cemu (emulador Wii U)
- Citra (emulador 3DS)
- Dolphin (emulador GameCube/Wii)
- Yuzu (emulador Switch)
- Cualquier juego compatible con XInput o DualShock4

## Solución de Problemas

### El Joy-Con no se conecta

**Bluetooth:**
- Verifique que el Bluetooth esté activado
- Mantenga presionado el botón de sincronización por 5 segundos
- Elimine emparejamientos previos de Windows
- Reinicie el adaptador Bluetooth

**USB:**
- Use un cable USB-C de calidad con soporte de datos
- Pruebe diferentes puertos USB
- Verifique que AGCV esté ejecutándose como Administrador

### Entrada duplicada en juegos

**Solución:**
- Instale HidHide desde: https://github.com/nefarius/HidHide/releases
- Active UseHIDHide en configuración
- Reinicie AGCV
- Los Joy-Cons se ocultarán automáticamente para otras aplicaciones

### Alta latencia o lag

**Optimizaciones:**
- Cierre otras aplicaciones que usen Bluetooth
- Asegúrese de que AGCV tenga prioridad alta de CPU
- Desactive el ahorro de energía del adaptador Bluetooth
- En modo emparejado, el sistema compensa automáticamente la latencia

### Error "Duplicate Timestamp"

Este error fue común en versiones antiguas de BetterJoy. En AGCV:
- Completamente eliminado mediante optimización de hooks
- Si persiste, verifique que no tenga software de captura de entrada instalado
- Desactive temporalmente software como AutoHotkey, EventGhost, etc.

## Comparación con BetterJoy Original

| Característica | BetterJoy Original | AGCV |
|----------------|-------------------|------|
| Soporte activo | No (desde 2020) | Sí (2025) |
| Sistema de usuarios | No | Sí, con roles |
| Historial de actividad | No | Sí, completo |
| Integración HidHide | No | Sí, automática |
| Error "Duplicate Timestamp" | Sí, frecuente | No, eliminado |
| Latencia Joy-Con izquierdo | Alta | Optimizada |
| Interfaz gráfica | Básica | Moderna, completa |
| Monitor de eventos | No | Sí, en tiempo real |
| Gestión de configuración | Manual | GUI integrada |
| Base de datos | No | Sí, SQLite |
| Documentación | Limitada | Completa |
| Versión .NET | Framework 4.7 | .NET 8 |

## Créditos y Agradecimientos

### Proyecto Original BetterJoy

Este proyecto está basado en y extiende el trabajo de:

**BetterJoy v7.0** por Davidobot  
Repositorio original: https://github.com/Davidobot/BetterJoy

Agradecimientos especiales a:
- **rajkosto** - ScpToolkit y servidor UDP
- **mfosse** - JoyCon-Driver
- **Looking-Glass** - JoyconLib
- **nefarius** - ViGEmBus
- **epigramx** - WiimoteHook
- **MTCKC** - ProconXInput
- **dekuNukem** - Documentación de ingeniería inversa de Nintendo Switch

### Librerías y Dependencias

- **ViGEmBus**: Driver de emulación de controles
- **HidHide**: Ocultación de dispositivos HID
- **hidapi**: Comunicación USB/Bluetooth de bajo nivel
- **WindowsInput**: Simulación de entrada de teclado/mouse
- **Nefarius.ViGEm.Client**: Cliente .NET para ViGEm

### Recursos Gráficos

Iconos modificados de The Noun Project:
- Nintendo Switch Pro Controller - Chad Remsing
- Joy-Con Left/Right - Chad Remsing
- SNES Controller - Mark Davis (modificado por Amy Alexander)
- N64 Controller - Mark Davis (modificado por Gino Moena)

## Licencia

Este proyecto mantiene compatibilidad con la licencia del proyecto original BetterJoy.

El código fuente está disponible bajo los mismos términos, permitiendo uso, modificación y distribución con la debida atribución a los autores originales y a este proyecto.

Para uso comercial o distribución modificada, consulte la licencia completa en el archivo LICENSE.

## Desarrollo Futuro

### Planes a Corto Plazo
- Soporte para más controles de terceros
- Perfiles de configuración por juego
- Calibración automática de giroscopio
- Modo de bajo consumo de energía
- Traducción a múltiples idiomas

### Planes a Largo Plazo
- Cliente multiplataforma (Linux, macOS)
- Sincronización en la nube de configuraciones
- API REST para integración con otras aplicaciones
- Soporte para otros controles (PlayStation, Xbox genéricos)
- Modo servidor para streaming local

## Contribuciones

Este proyecto es parte de un trabajo académico finalizado. Sin embargo, las contribuciones de la comunidad son bienvenidas:

1. Fork el repositorio
2. Cree una rama para su feature (git checkout -b feature/nueva-funcionalidad)
3. Commit sus cambios (git commit -m 'Añadir nueva funcionalidad')
4. Push a la rama (git push origin feature/nueva-funcionalidad)
5. Abra un Pull Request

Por favor, asegúrese de:
- Seguir las convenciones de código existentes
- Documentar nuevas funcionalidades
- Probar exhaustivamente los cambios
- Actualizar la documentación según sea necesario

## Soporte y Contacto

**Reportar Problemas:**  
Abra un issue en: https://github.com/Antoniazog493/AGCV-Project/issues

**Documentación Técnica:**  
Consulte la carpeta /docs para documentación detallada de desarrollo

**Wiki del Proyecto:**  
https://github.com/Antoniazog493/AGCV-Project/wiki

## Referencias

- Documentación de ViGEmBus: https://github.com/ViGEm/ViGEmBus
- Documentación de HidHide: https://github.com/nefarius/HidHide
- Nintendo Switch Reverse Engineering: https://github.com/dekuNukem/Nintendo_Switch_Reverse_Engineering
- Guía de Configuración de CemuHook: https://cemuhook.sshnuke.net/

---

**Proyecto desarrollado como requisito de la asignatura Herramientas de Programación Avanzada III**  
**Universidad Tecnológica de Panamá - 2025**

**Versión actual:** 1.0.0  
**Última actualización:** Enero 2025
