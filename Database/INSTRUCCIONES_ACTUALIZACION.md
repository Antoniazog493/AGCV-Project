# ??? INSTRUCCIONES DE ACTUALIZACIÓN DE BASE DE DATOS

## Para: DBVideojuegos (AGCV)

---

## ?? PASOS PARA ACTUALIZAR LA BASE DE DATOS

### **PASO 1: Abrir SQL Server Management Studio (SSMS)**

1. Abre **SQL Server Management Studio**
2. Conéctate a tu servidor: `PORTABLE-HUB\SQLEXPRESS`
3. Credenciales: Usa **Windows Authentication**

---

### **PASO 2: Ejecutar el Script de Actualización**

1. En SSMS, haz clic en **File ? Open ? File** (o presiona `Ctrl+O`)
2. Navega hasta tu proyecto y abre el archivo:
   ```
   C:\Users\Anton\Downloads\AGCV-Project\Database\01_Crear_Tabla_Historial.sql
   ```
3. Asegúrate de que esté seleccionada la base de datos **DBVideojuegos** en el dropdown superior
4. Presiona **F5** o haz clic en **Execute**

---

### **PASO 3: Verificar la Ejecución**

El script mostrará mensajes de progreso. Deberías ver algo como esto:

```
============================================
INICIANDO ACTUALIZACIÓN DE BASE DE DATOS
Sistema AGCV - Historial de Conexiones
============================================

PASO 1: Verificando tabla CEHistorial...
   ? Tabla CEHistorial creada exitosamente.

PASO 2: Verificando relaciones de tabla...
   ? Foreign Key creada exitosamente.

PASO 3: Verificando índices...
   ? Índice IX_CEHistorial_IdUsuario_FechaRegistro creado.
   ? Índice IX_CEHistorial_Accion creado.

PASO 4: Actualizando procedimientos almacenados...
   ? SP_RegistrarAccion listo.
   ? SP_HistorialPorUsuario listo.
   ? SP_EstadisticasHistorial listo.
   ? SP_LimpiarHistorial listo.

PASO 5: Verificando vistas...
   ? VW_HistorialCompleto lista.

PASO 6: Verificando datos de prueba...
   ? 4 registros de prueba insertados.

============================================
? ACTUALIZACIÓN COMPLETADA EXITOSAMENTE
============================================
```

---

## ? VERIFICACIÓN POST-INSTALACIÓN

### **Opción 1: Verificar desde SSMS**

Ejecuta estas consultas para verificar que todo está correcto:

```sql
-- Verificar que la tabla existe
USE DBVideojuegos;
GO

SELECT * FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_NAME = 'CEHistorial';

-- Ver la estructura de la tabla
EXEC sp_help 'CEHistorial';

-- Ver los registros de prueba
SELECT * FROM CEHistorial;

-- Ver las estadísticas
SELECT 
    COUNT(*) AS TotalRegistros,
    COUNT(DISTINCT IdUsuario) AS UsuariosConRegistros
FROM CEHistorial;
```

### **Opción 2: Verificar desde la Aplicación AGCV**

1. Abre la aplicación **AGCV**
2. Inicia sesión con el usuario **Roberto** (contraseña: `123456`)
3. Ve a **Ajustes ? Ver Historial**
4. Deberías ver los registros de prueba insertados

---

## ?? QUÉ HACE EL SCRIPT

### **Modificaciones Realizadas**

1. ? **Crea/Actualiza tabla CEHistorial**
   - Columnas optimizadas (Accion: 200 caracteres, Detalles: MAX)
   - Compatible con tu estructura existente

2. ? **Crea Foreign Key** con CEUsuario
   - Relación: `IdUsuario` ? `CEUsuario.IdUsuario`
   - Configurada con `ON DELETE CASCADE` (si eliminas un usuario, se elimina su historial)

3. ? **Crea Índices** para rendimiento
   - `IX_CEHistorial_IdUsuario_FechaRegistro` - Consultas por usuario ordenadas
   - `IX_CEHistorial_Accion` - Filtros por tipo de acción
   - `IX_CEHistorial_FechaRegistro` - Ordenamiento por fecha

4. ? **Actualiza Procedimientos Almacenados**
   - `SP_RegistrarAccion` - Mejorado con más campos
   - `SP_HistorialPorUsuario` - Ahora acepta límite de registros
   - `SP_EstadisticasHistorial` - NUEVO (estadísticas del usuario)
   - `SP_LimpiarHistorial` - NUEVO (limpiar historial)

5. ? **Crea Vista** VW_HistorialCompleto
   - Para administradores ver todo el historial

6. ? **Inserta Datos de Prueba** (opcional)
   - 4 registros de ejemplo para el usuario administrador

---

## ??? SEGURIDAD DEL SCRIPT

### **El script es IDEMPOTENTE** ?

Esto significa que:
- ? Puedes ejecutarlo **múltiples veces** sin problemas
- ? **NO perderás datos** existentes
- ? Solo actualiza lo necesario
- ? Verifica antes de crear/modificar

### **Protecciones Incluidas**

```sql
-- Ejemplo: Solo crea si NO existe
IF NOT EXISTS (SELECT * FROM sys.objects WHERE name = 'CEHistorial')
BEGIN
    CREATE TABLE CEHistorial ...
END

-- Ejemplo: Solo inserta datos de prueba si la tabla está vacía
IF (SELECT COUNT(*) FROM CEHistorial) = 0
BEGIN
    INSERT INTO CEHistorial ...
END
```

---

## ?? ESTRUCTURA FINAL DE CEHistorial

| Columna | Tipo | Permite NULL | Descripción |
|---------|------|--------------|-------------|
| `IdHistorial` | INT (IDENTITY) | No | ID único del registro |
| `IdUsuario` | INT | No | ID del usuario (FK) |
| `Accion` | NVARCHAR(200) | No | Tipo de acción |
| `Detalles` | NVARCHAR(MAX) | Sí | Detalles adicionales |
| `FechaRegistro` | DATETIME2 | No | Fecha/hora del evento |

---

## ?? PROCEDIMIENTOS ALMACENADOS DISPONIBLES

### **SP_RegistrarAccion**
```sql
EXEC SP_RegistrarAccion 
    @IdUsuario = 1, 
    @Accion = 'Inicio de sesión', 
    @Detalles = 'Usuario: Roberto, Rol: Administrador';
```

### **SP_HistorialPorUsuario**
```sql
-- Obtener últimos 100 registros
EXEC SP_HistorialPorUsuario @IdUsuario = 1, @Limite = 100;

-- Obtener todos los registros (hasta 500)
EXEC SP_HistorialPorUsuario @IdUsuario = 1;
```

### **SP_EstadisticasHistorial**
```sql
EXEC SP_EstadisticasHistorial @IdUsuario = 1;
```

### **SP_LimpiarHistorial**
```sql
-- Limpiar TODO el historial del usuario
EXEC SP_LimpiarHistorial @IdUsuario = 1;

-- Limpiar solo registros mayores a 30 días
EXEC SP_LimpiarHistorial @IdUsuario = 1, @DiasAntiguedad = 30;
```

---

## ?? TROUBLESHOOTING

### **Error: "Database 'DBVideojuegos' does not exist"**
**Solución**: Asegúrate de que la base de datos DBVideojuegos existe. Si no, ejecuta primero tu script original de creación de base de datos.

### **Error: "Cannot drop the table 'CEHistorial' because it is being referenced by a FOREIGN KEY constraint"**
**Solución**: El script maneja esto automáticamente. Si ves este error, es porque algo está intentando eliminar la tabla manualmente.

### **Warning: "The object 'CEHistorial' already exists"**
**Solución**: Esto es normal y esperado. El script verifica y actualiza la tabla existente.

### **Error: "Invalid column name 'Detalles'"**
**Solución**: Ejecuta nuevamente el script completo. Asegúrate de ejecutar TODO el script, no solo partes.

---

## ?? CONTACTO Y SOPORTE

Si tienes problemas:

1. Verifica que SQL Server esté corriendo
2. Asegúrate de tener permisos de administrador en la base de datos
3. Revisa los mensajes de error en SSMS (panel de Messages)
4. Ejecuta el script completo de nuevo (es seguro hacerlo)

---

## ?? ¡LISTO PARA USAR!

Una vez ejecutado el script exitosamente:

1. ? La tabla CEHistorial está creada/actualizada
2. ? Los índices están optimizados
3. ? Los procedimientos almacenados están listos
4. ? La aplicación AGCV puede registrar eventos
5. ? El formulario de Historial funcionará correctamente

---

**Fecha de última actualización**: 2024
**Versión del script**: 2.0
**Compatible con**: SQL Server 2019+ / SQL Server Express
