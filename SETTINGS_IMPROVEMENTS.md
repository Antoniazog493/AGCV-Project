# Mejoras en el Sistema de Configuración - v3

## Problemas Resueltos

### Problema 1: Restart muy rápido causaba "Instance Already Running"
**Antes**: Al reiniciar, la nueva instancia se abría antes de que la anterior terminara de cerrarse, causando el error "Instance Already Running".

**Causa**: `Process.Start()` inicia el proceso inmediatamente sin esperar a que la aplicación actual se cierre completamente.

**Solución**: 
- Usar `cmd.exe` con `timeout` para esperar 2 segundos antes de abrir la nueva instancia
- Esto da tiempo suficiente para que la aplicación actual se cierre completamente

```csharp
ProcessStartInfo startInfo = new ProcessStartInfo {
    FileName = "cmd.exe",
    Arguments = $"/c timeout /t 2 /nobreak > nul && start \"\" \"{exePath}\"",
    UseShellExecute = false,
    CreateNoWindow = true,
    WorkingDirectory = System.IO.Path.GetDirectoryName(exePath)
};
```

### Problema 2: Opciones se aplicaban automáticamente al hacer clic
**Antes**: Algunas opciones (como HomeLEDOn) se aplicaban inmediatamente al cambiar el checkbox, incluso sin presionar Apply.

**Solución**: 
- Removido completamente el código que aplicaba cambios automáticamente en `cbBox_Changed`
- Ahora **TODOS** los cambios requieren que el usuario presione el botón "Apply"
- Esto da al usuario control total sobre cuándo aplicar los cambios

### Problema 3: Nombres de opciones difíciles de leer
**Antes**: Los nombres de configuración estaban en CamelCase sin espacios (ej: "HomeLEDOn", "ShowAsXInput")

**Solución**:
- Agregado método `AddSpacesToCamelCase()` que convierte automáticamente nombres CamelCase a formato legible
- Los nombres ahora aparecen con espacios: "Home LED On", "Show As X Input"
- El nombre original se guarda en la propiedad `Tag` del Label para no perder la referencia

**Ejemplos de conversión**:
- `HomeLEDOn` ? `Home LED On`
- `ShowAsXInput` ? `Show As X Input`
- `GyroMouseSensitivityX` ? `Gyro Mouse Sensitivity X`
- `AHRS_beta` ? `AHRS_beta` (conserva guiones bajos)

## Solución Implementada (Resumen Completo)

Se ha separado la configuración en **dos categorías** y se corrigió el flujo de guardado y reinicio:

### 1. Settings que Requieren Reinicio (Marcados en Naranja)
Estos settings afectan componentes fundamentales que solo se pueden aplicar al iniciar la aplicación:
- `Show As X Input` - Activar emulación de controlador Xbox
- `Show As DS 4` - Activar emulación de controlador DualShock 4
- `Enable Rumble` - Habilitar vibración
- `IP` / `Port` - Configuración del servidor UDP
- `Use HID Hide` - Ocultar controladores del sistema
- `Auto Power Off` - Apagar controladores automáticamente
- `Do Not Rejoin Joycons` - No reunir Joy-Cons automáticamente
- `Allow Calibration` - Permitir calibración
- `Enable Shake Input`, `Shake Input Sensitivity`, `Shake Input Delay` - Configuración de shake
- Y otros relacionados con inicialización

### 2. Settings que se Aplican Instantáneamente
Estos settings se pueden cambiar sin reiniciar:
- `Home LED On` - Luz del botón HOME (se aplica al presionar Apply)
- `Progressive Scan` - Escaneo pasivo de controladores
- `Start In Tray` - Iniciar minimizado en bandeja
- Y otros que afectan comportamiento en tiempo real

## Nuevas Características

### Indicadores Visuales Mejorados
- **Etiquetas naranjas**: Los settings que requieren reinicio están marcados en color naranja
- **Nombres legibles**: Todos los nombres tienen espacios entre palabras para mejor legibilidad
- **Botón dinámico**: El botón "Apply" cambia su apariencia según el tipo de cambios pendientes:
  - `Apply` (gris) - Solo cambios instantáneos o sin cambios
  - `Apply & Restart` (rojo claro) - Hay cambios que requieren reinicio

### Flujo de Guardado Mejorado

**Cambio Importante**: Los settings ya **NO se guardan automáticamente** al hacer clic. **SIEMPRE** se requiere presionar "Apply".

Esto resuelve los problemas de:
- "No changes to apply" cuando sí había cambios
- Usuario puede cambiar de opinión antes de aplicar
- Control total sobre cuándo se guardan los cambios
- Experiencia consistente para todas las opciones

### Flujo Actual
1. **Cambiar un setting**:
   - El valor cambia en la UI
   - Si es un setting naranja, el botón cambia a "Apply & Restart" (rojo)
   - Los cambios **NO se guardan ni se aplican** hasta presionar Apply

2. **Sin cambios que requieran reinicio**: 
   - Al presionar "Apply", los cambios se guardan
   - Se refresca la configuración
   - Se aplican inmediatamente (ej: Home LED On enciende/apaga LEDs)
   - La aplicación continúa funcionando normalmente
   - Mensajes de diagnóstico en consola muestran el progreso

3. **Con cambios que requieren reinicio**:
   - Al presionar "Apply & Restart", los cambios se guardan
   - Aparece un diálogo de confirmación
   - El usuario puede elegir:
     - **Sí**: Usa `cmd.exe timeout` para esperar 2 segundos, luego inicia nueva instancia y cierra la actual
     - **No**: Los cambios están guardados pero continúa sin reiniciar (requiere reinicio manual posterior)

### Método de Reinicio Mejorado (v3)

**Nuevo método (más confiable y sin conflictos)**:
```csharp
1. Guardar configuración con AutoPowerOff=false
2. Obtener ruta del ejecutable
3. Crear ProcessStartInfo que usa cmd.exe con timeout de 2 segundos
4. Iniciar proceso cmd que esperará y luego abrirá BetterJoy
5. Cerrar aplicación actual con Application.Exit()
```

**Por qué es mejor**:
- ? Espera 2 segundos antes de abrir la nueva instancia
- ? Elimina el error "Instance Already Running"
- ? No depende de Application.Restart() que puede fallar
- ? El proceso cmd se ejecuta en background (CreateNoWindow = true)
- ? Maneja errores con mensajes claros al usuario

### Mejoras en la Experiencia
- ? Ya no hay error de "Instance Already Running"
- ? El usuario tiene control total sobre cuándo aplicar cambios
- ? Los cambios **NUNCA** se aplican automáticamente - siempre requieren Apply
- ? Nombres de opciones más fáciles de leer (con espacios)
- ? El usuario tiene control sobre cuándo reiniciar
- ? Feedback detallado en la consola sobre qué está pasando
- ? Previene pérdida de conexión innecesaria con los controladores
- ? El botón siempre muestra el estado correcto de cambios pendientes
- ? Reinicio funciona correctamente sin conflictos

## Mensajes de Consola Mejorados

La aplicación ahora muestra mensajes diagnósticos detallados:

### Para cambios instantáneos:
```
Settings saved successfully.
Settings applied successfully (no restart needed).
Applying instant settings...
Setting HomeLED to: True
HomeLED applied to controller 0
HomeLED applied to controller 1
Instant settings applied successfully.
```

### Para reinicio:
```
Settings saved successfully.
Restarting application...
Starting new instance: C:\...\BetterJoyForCemu.exe
[La aplicación se cierra y espera 2 segundos]
[Nueva instancia se abre]
```

### Si falla el reinicio:
```
Error restarting: [mensaje de error]
Stack trace: [detalles técnicos]
[Diálogo]: Could not restart automatically. Please restart BetterJoy manually.
```

## Ejemplo de Uso

### Caso 1: Cambiar solo Home LED On
1. Usuario cambia `Home LED On` de false a true
2. Botón permanece como "Apply" (gris)
3. **Nada sucede** hasta que presiona "Apply"
4. Presiona "Apply"
5. Mensajes en consola:
   - "Settings saved successfully."
   - "Applying instant settings..."
   - "Setting HomeLED to: true"
   - "HomeLED applied to controller 0"
   - "Instant settings applied successfully."
6. Los LEDs se encienden
7. La aplicación continúa funcionando

### Caso 2: Cambiar Show As X Input
1. Usuario cambia `Show As X Input` de false a true (etiqueta naranja)
2. El botón cambia a "Apply & Restart" (rojo claro)
3. **Nada sucede** hasta que presiona el botón
4. Usuario presiona el botón
5. Los cambios se guardan
6. Aparece: "¿Deseas reiniciar BetterJoy ahora?"
7. Si elige "Sí": 
   - Consola muestra: "Restarting application..."
   - Se inicia cmd.exe con timeout de 2 segundos
   - Se cierra la instancia actual
   - Después de 2 segundos, BetterJoy se abre con la nueva configuración
   - **NO** hay error de "Instance Already Running"
8. Si elige "No": El cambio está guardado pero requiere reinicio manual

### Caso 3: Cambios mixtos
1. Usuario cambia `Home LED On` (instantáneo) y `Show As DS 4` (requiere reinicio)
2. El botón cambia a "Apply & Restart" (rojo claro) porque hay un setting naranja
3. **Nada sucede** hasta presionar Apply
4. Al presionar Apply:
   - Ambos cambios se guardan
   - Se pregunta sobre el reinicio
   - Si elige No: `Home LED On` se aplica inmediatamente, pero `Show As DS 4` requiere reinicio manual

### Caso 4: Cambiar y revertir
1. Usuario cambia `Show As X Input` de false a true
2. Botón cambia a "Apply & Restart" (rojo)
3. Usuario cambia de opinión y lo vuelve a false
4. Botón vuelve a "Apply" (gris) - detecta que no hay cambios reales
5. Si presiona Apply: "No changes to apply."

## Solución de Problemas

### Si Home LED On no funciona:
1. Verifica la consola - debe mostrar:
   ```
   Applying instant settings...
   Setting HomeLED to: [true/false]
   HomeLED applied to controller [ID]
   ```
2. Si dice "No controllers connected" ? Conecta un controlador primero
3. Si muestra "Error setting HomeLED" ? Revisa el mensaje de error específico
4. **Asegúrate de presionar Apply** - ya no se aplica automáticamente

### Si el reinicio no funciona:
1. La consola mostrará el error exacto
2. Un diálogo te pedirá reiniciar manualmente
3. Los cambios YA están guardados - solo cierra y abre BetterJoy

### Si aparece "Instance Already Running":
1. Esto **ya no debería suceder** con el nuevo método de reinicio
2. Si aún sucede, espera 2-3 segundos y vuelve a abrir BetterJoy
3. Los cambios ya están guardados en el archivo de configuración

## Correcciones Adicionales

Se corrigió un bug en el método `CalcData` donde el tipo `KeyValuePair<string, float>` era incorrecto y debía ser `KeyValuePair<string, float[]>`.

## Notas Técnicas

- La lista de settings que requieren reinicio está definida en `RestartRequiredSettings`
- Se puede modificar fácilmente agregando/quitando settings de esta lista
- El sistema detecta automáticamente qué tipo de cambios se hicieron
- Se previene el apagado automático de Joy-Cons al reiniciar
- Los valores originales se rastrean en el diccionario `originalValues`
- `cbBox_Changed` **NO** guarda ni aplica nada - solo actualiza el estado del botón
- `settingsApply_Click` es el **ÚNICO** método que guarda y aplica cambios
- El reinicio usa `cmd.exe timeout` para evitar conflictos de "Instance Already Running"
- `ApplyInstantSettings()` llama a `ConfigurationManager.RefreshSection()` antes de leer valores
- Todos los métodos tienen try-catch para manejar errores gracefully
- Los mensajes de consola proporcionan diagnóstico detallado para troubleshooting
- Los nombres de configuración se convierten automáticamente a formato legible con espacios
- El nombre original se guarda en la propiedad `Tag` del Label para no perder la referencia

## Instrucciones para Usar

**IMPORTANTE**: 
1. **Cierra BetterJoy** antes de compilar si está corriendo
2. Los cambios en la configuración **SIEMPRE** requieren presionar "Apply"
3. Los settings naranjas requieren reinicio para tomar efecto
4. El reinicio ahora espera 2 segundos para evitar conflictos

### Cambios Visibles para el Usuario:
1. ? Nombres de opciones más fáciles de leer (con espacios)
2. ? **NINGUNA** opción se aplica automáticamente - siempre requiere Apply
3. ? El reinicio ya no causa "Instance Already Running"
4. ? Feedback claro sobre qué cambios requieren reinicio (color naranja)
