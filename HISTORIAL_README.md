# ?? HISTORIAL DE CONEXIONES - AGCV

## ? Implementación Completa

Se ha implementado exitosamente el **Sistema de Historial de Conexiones** para AGCV.

---

## ?? Características Implementadas

### 1. **Registro Automático de Eventos**
- ? Inicio/Cierre de sesión
- ? Inicio/Cierre del motor AGCV (BetterJoy)
- ? Conexión/Desconexión de Joy-Cons (próximamente)
- ? Errores de conexión
- ? Configuraciones modificadas (próximamente)

### 2. **Visualización del Historial**
- DataGridView con diseño moderno
- Columnas: Fecha/Hora, Acción, Detalles
- Contador de registros en tiempo real
- Colores alternados para mejor legibilidad

### 3. **Exportación a CSV**
- Exporta todo el historial del usuario
- Formato compatible con Excel
- Nombre de archivo automático con fecha/hora
- Opción para abrir el archivo después de exportar

### 4. **Limpieza de Historial**
- **Opción 1**: Limpiar TODO el historial (con confirmación)
- **Opción 2**: Limpiar registros antiguos (> 30 días)
- Interfaz moderna con botones diferenciados por color

### 5. **Estadísticas**
- Total de registros
- Total de conexiones
- Total de desconexiones

---

## ?? Archivos Creados/Modificados

### **Nuevos Archivos**
1. `capaDatos\CDHistorial.cs` - Capa de datos para operaciones SQL
2. `capaNegocio\CNHistorial.cs` - Lógica de negocio
3. `Database\01_Crear_Tabla_Historial.sql` - Script de base de datos

### **Archivos Modificados**
1. `AGCV\SesionActual.cs` - Métodos para registrar eventos
2. `AGCV\Historial.cs` - Funcionalidad completa del formulario
3. `AGCV\MenuPrincipal.cs` - Registro de inicio/cierre AGCV
4. `AGCV\InicioSesion.cs` - Registro de inicio de sesión

---

## ??? Configuración de Base de Datos

### **Paso 1: Ejecutar Script SQL**

1. Abre SQL Server Management Studio (SSMS)
2. Conéctate a tu servidor: `PORTABLE-HUB\SQLEXPRESS`
3. Abre el archivo: `Database\01_Crear_Tabla_Historial.sql`
4. Ejecuta el script (F5)

El script creará automáticamente:
- Tabla `CEHistorial`
- Índices para optimizar consultas
- Relación con tabla `CEUsuario`

### **Paso 2: Verificar Creación**

```sql
-- Verificar que la tabla existe
SELECT * FROM CEHistorial;

-- Ver estructura de la tabla
EXEC sp_help 'CEHistorial';
```

---

## ?? Cómo Usar el Historial

### **Acceder al Historial**

1. Inicia sesión en AGCV
2. Ve al menú principal (HOME)
3. Haz clic en **Ajustes** (icono de engranaje)
4. Haz clic en **"Ver Historial"**

### **Exportar Historial**

1. En el formulario de historial, haz clic en **"Exportar"**
2. Elige la ubicación para guardar el archivo CSV
3. El archivo se guarda con el nombre: `Historial_AGCV_[Usuario]_[FechaHora].csv`
4. Opcionalmente, ábrelo en Excel

### **Limpiar Historial**

1. Haz clic en **"Limpiar"**
2. Elige una opción:
   - **Limpiar TODO**: Elimina todos los registros (con confirmación)
   - **Limpiar antiguos**: Elimina registros con más de 30 días
3. Confirma la acción

---

## ?? Estructura de la Base de Datos

### **Tabla: CEHistorial**

| Columna | Tipo | Descripción |
|---------|------|-------------|
| `IdHistorial` | int (PK, Identity) | ID único del registro |
| `IdUsuario` | int (FK) | ID del usuario |
| `Accion` | nvarchar(200) | Tipo de acción realizada |
| `Detalles` | nvarchar(MAX) | Detalles adicionales (opcional) |
| `FechaRegistro` | datetime2 | Fecha y hora del evento |

### **Índices**
- `IX_CEHistorial_IdUsuario_FechaRegistro` - Para consultas rápidas por usuario
- `IX_CEHistorial_Accion` - Para filtrar por tipo de acción

---

## ?? Funciones Principales

### **SesionActual** (Métodos Estáticos)

```csharp
// Registrar eventos manualmente
SesionActual.RegistrarAccion("Mi acción", "Detalles opcionales");

// Métodos predefinidos
SesionActual.RegistrarInicioSesion();
SesionActual.RegistrarCierreSesion();
SesionActual.RegistrarInicioAGCV();
SesionActual.RegistrarCierreAGCV();
SesionActual.RegistrarError("Mensaje de error");
```

### **CNHistorial** (Clase de Negocio)

```csharp
var historial = new CNHistorial();

// Registrar acción
historial.RegistrarAccion(idUsuario, "Acción", "Detalles");

// Obtener historial
DataTable datos = historial.ObtenerHistorial(idUsuario, limite: 500);

// Exportar a CSV
bool exitoso = historial.ExportarACSV(idUsuario, "ruta/archivo.csv");

// Limpiar historial
bool limpiado = historial.LimpiarHistorial(idUsuario);

// Obtener estadísticas
Dictionary<string, int> stats = historial.ObtenerEstadisticas(idUsuario);
```

---

## ?? Acciones Predefinidas

El sistema incluye constantes para acciones comunes en `CNHistorial.AccionesComunes`:

- `InicioSesion` - "Inicio de sesión"
- `CierreSesion` - "Cierre de sesión"
- `ConexionJoyCon` - "Conexión de Joy-Con"
- `DesconexionJoyCon` - "Desconexión de Joy-Con"
- `InicioAGCV` - "Inicio de motor AGCV"
- `CierreAGCV` - "Cierre de motor AGCV"
- `ErrorConexion` - "Error de conexión"
- `ConfiguracionCambiada` - "Configuración modificada"

---

## ?? Próximos Pasos (Opcional)

Para completar la integración, se podría:

1. **Registrar conexiones de Joy-Con**
   - Modificar `BetterJoyForCemu\Program.cs` o `JoyconManager`
   - Llamar a `SesionActual.RegistrarAccion()` cuando se conecte/desconecte un Joy-Con

2. **Agregar más estadísticas**
   - Tiempo promedio de uso
   - Joy-Con más usado
   - Gráficos de uso por día/semana

3. **Notificaciones**
   - Alertas cuando hay muchos errores
   - Recordatorios para limpiar historial antiguo

---

## ? Checklist de Implementación

- [x] Capa de Datos (CDHistorial)
- [x] Capa de Negocio (CNHistorial)
- [x] Actualización de SesionActual
- [x] Formulario Historial funcional
- [x] Exportación a CSV
- [x] Limpieza de historial
- [x] Script de base de datos
- [x] Registro de inicio/cierre sesión
- [x] Registro de inicio/cierre AGCV
- [ ] Registro de conexión/desconexión Joy-Con (próximamente)
- [ ] Estadísticas avanzadas (próximamente)

---

## ?? Troubleshooting

### **Error: "Tabla CEHistorial no existe"**
**Solución**: Ejecuta el script `Database\01_Crear_Tabla_Historial.sql`

### **Error: "No se puede conectar a la base de datos"**
**Solución**: Verifica la cadena de conexión en `CDHistorial.cs`:
```csharp
private readonly string CadenaConexion = "Server=PORTABLE-HUB\\SQLEXPRESS;Database=DBVideojuegos;Trusted_Connection=True; Encrypt=True; TrustServerCertificate=True;";
```

### **El historial está vacío**
**Solución**: Los eventos se registran automáticamente. Inicia/cierra sesión varias veces para generar registros.

---

## ?? Soporte

Si encuentras algún problema, verifica:
1. La base de datos está corriendo
2. El script SQL se ejecutó correctamente
3. La cadena de conexión es correcta
4. El usuario de la sesión es válido

---

**¡El sistema de historial está completamente funcional y listo para usar!** ??
