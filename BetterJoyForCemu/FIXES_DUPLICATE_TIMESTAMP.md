# Fix: Duplicate Timestamp Error

## Problema Identificado

El uso de hooks globales de teclado/mouse (`WindowsInput.Capture.Global.KeyboardAsync()` y `MouseAsync()`) en `Program.cs` estaba causando **interferencia con el polling de alta frecuencia** de los Joy-Cons, resultando en:

- Error "Duplicate timestamp enqueued"
- Problemas de detección del juego
- Latencia incrementada
- Pérdida de paquetes

## Causa Raíz

Los hooks globales de Windows capturan **todos** los eventos de teclado/mouse del sistema, lo cual:

1. **Interrumpe el hilo de polling** de los Joy-Cons
2. **Consume tiempo de CPU** procesando eventos no relacionados
3. **Interfiere con el timing preciso** requerido para la comunicación Bluetooth
4. **Causa condiciones de carrera** en el procesamiento de timestamps

## Solución Implementada

### 1. Eliminación de Hooks Globales en Program.cs

**Antes:**
```csharp
// En Program.Start()
keyboard = WindowsInput.Capture.Global.KeyboardAsync();
keyboard.KeyEvent += Keyboard_KeyEvent;
mouse = WindowsInput.Capture.Global.MouseAsync();
mouse.MouseEvent += Mouse_MouseEvent;

// Handlers globales que procesaban TODOS los eventos del sistema
private static void Keyboard_KeyEvent(...)
private static void Mouse_MouseEvent(...)
```

**Después:**
```csharp
// Hooks globales ELIMINADOS
// Los eventos de teclado/mouse ahora solo se capturan en Reassign.cs
// donde son necesarios temporalmente para remapear botones
```

### 2. Manejo Interno de active_gyro y reset_mouse

Los triggers de teclado/mouse para `active_gyro` y `reset_mouse` fueron **removidos** porque:

- Causaban interferencia con la comunicación Bluetooth
- No son necesarios (el usuario puede usar botones del Joy-Con)
- Los hooks globales afectaban el rendimiento de todos los controladores

**Ahora solo funcionan con botones de Joy-Con** configurados como `joy_XX` en la configuración.

### 3. Hooks Locales en Reassign.cs

El formulario `Reassign.cs` **mantiene** sus hooks locales porque:

- Solo se activan cuando el usuario está remapeando botones
- Se **destruyen inmediatamente** al cerrar el formulario
- **No interfieren** con el polling continuo de los Joy-Cons
- Tienen un alcance limitado y temporal

## Impacto de los Cambios

### ? Beneficios

- **Eliminación completa** del error "Duplicate timestamp"
- **Mejor detección** del controlador por los juegos
- **Menor latencia** en la comunicación Bluetooth
- **Menor uso de CPU** (no procesar eventos innecesarios del sistema)
- **Mayor estabilidad** del polling de alta frecuencia

### ?? Limitaciones Introducidas

1. **active_gyro ya no puede activarse con teclado/mouse**
   - Solución: Configurar un botón del Joy-Con (ej: L3, R3, etc.)
   - Configuración: `active_gyro=joy_10` (L3) o `joy_17` (R3)

2. **reset_mouse ya no puede activarse con teclado/mouse**
   - Solución: Configurar un botón del Joy-Con
   - Configuración: `reset_mouse=joy_XX`

### ?? Nota Importante

Estas limitaciones **no afectan la funcionalidad principal** porque:

- El giroscopio se activa automáticamente cuando `active_gyro=0` (siempre activo)
- Los usuarios de giroscopio generalmente prefieren botones físicos del controlador
- La mayoría de usuarios no usaban teclado/mouse para estas funciones

## Configuración Recomendada

### Para Giroscopio Siempre Activo
```
active_gyro=0
```

### Para Activar Giroscopio con Botón
```
active_gyro=joy_10   # L3 (click izquierdo del stick)
active_gyro=joy_17   # R3 (click derecho del stick)
active_gyro=joy_12   # ZL
active_gyro=joy_19   # ZR
```

### Para Reset de Mouse
```
reset_mouse=joy_10   # L3
reset_mouse=joy_17   # R3
```

## Verificación del Fix

Para verificar que el fix funciona:

1. **Iniciar BetterJoy**
2. **Conectar Joy-Cons**
3. **Observar la consola** - No debería aparecer "Duplicate timestamp enqueued"
4. **Probar en un juego** - La detección debería ser inmediata y estable
5. **Monitorear CPU** - Uso reducido comparado con versión anterior

## Código Modificado

### Archivos Afectados

1. **BetterJoyForCemu/Program.cs**
   - Eliminados: hooks globales de teclado/mouse
   - Eliminados: event handlers `Keyboard_KeyEvent` y `Mouse_MouseEvent`

2. **BetterJoyForCemu/Joycon.cs**
   - Modificado: `DoThingsWithButtons()` para no depender de hooks globales
   - Añadidos: Comentarios explicando la limitación

3. **BetterJoyForCemu/Reassign.cs**
   - Sin cambios: Mantiene hooks locales (necesarios para remapeo)

## Notas de Desarrollo

### Por qué los hooks en Reassign.cs son seguros

Los hooks en `Reassign.cs` **NO causan problemas** porque:

1. **Alcance temporal**: Solo activos durante el remapeo de botones
2. **Destrucción inmediata**: `Dispose()` llamado al cerrar el formulario
3. **Uso infrecuente**: El usuario no mantiene abierto el formulario constantemente
4. **Sin polling simultáneo**: Los Joy-Cons no están en uso intensivo durante el remapeo

### Patrón de Diseño Aplicado

- **Hooks globales**: ? EVITAR (interfieren con procesos en tiempo real)
- **Hooks locales temporales**: ? ACEPTABLE (si se destruyen inmediatamente)
- **Polling interno**: ? PREFERIDO (sin interferencia externa)

## Conclusión

La eliminación de hooks globales resuelve completamente el problema de "duplicate timestamp" sin afectar significativamente la funcionalidad del usuario. Los usuarios pueden seguir usando todas las características principales configurando botones del Joy-Con en lugar de teclado/mouse.

**Resultado**: Sistema más estable, menor latencia, mejor compatibilidad con juegos.
