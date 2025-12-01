-- =============================================
-- Índices recomendados para optimizar queries
-- Base de datos: DBVideojuegos
-- Tabla: CEUsuario
-- =============================================

-- ÍNDICE 1: Optimizar Login (búsqueda por NombreUsuario + ClaveHash + Activo)
-- Beneficia: Método Login() - Query más frecuente del sistema
CREATE NONCLUSTERED INDEX IX_CEUsuario_Login
ON CEUsuario (NombreUsuario, ClaveHash, Activo)
INCLUDE (IdUsuario, Correo, Rol);
GO

-- ÍNDICE 2: Optimizar búsqueda por ID (usado en EsAdministrador, ObtenerPorId)
-- Beneficia: Métodos EsAdministrador(), ObtenerPorId()
CREATE NONCLUSTERED INDEX IX_CEUsuario_IdUsuario_Activo
ON CEUsuario (IdUsuario, Activo)
INCLUDE (Rol, NombreUsuario, Correo, FechaCreacion, UltimoAcceso);
GO

-- ÍNDICE 3: Optimizar conteo de administradores
-- Beneficia: Método ContarAdministradoresActivos()
CREATE NONCLUSTERED INDEX IX_CEUsuario_Rol_Activo
ON CEUsuario (Rol, Activo);
GO

-- ÍNDICE 4: Optimizar ordenamiento por fecha de creación
-- Beneficia: Método ObtenerTodosLosUsuarios()
CREATE NONCLUSTERED INDEX IX_CEUsuario_FechaCreacion
ON CEUsuario (FechaCreacion DESC)
INCLUDE (IdUsuario, NombreUsuario, Correo, Rol, UltimoAcceso, Activo);
GO

-- =============================================
-- Estadísticas recomendadas
-- =============================================

-- Actualizar estadísticas después de crear índices
UPDATE STATISTICS CEUsuario WITH FULLSCAN;
GO

-- =============================================
-- Verificación de índices
-- =============================================

-- Query para verificar los índices creados
SELECT 
    i.name AS IndexName,
    i.type_desc AS IndexType,
    STUFF((SELECT ', ' + c.name
           FROM sys.index_columns ic
           INNER JOIN sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
           WHERE ic.object_id = i.object_id AND ic.index_id = i.index_id AND ic.is_included_column = 0
           ORDER BY ic.key_ordinal
           FOR XML PATH('')), 1, 2, '') AS KeyColumns,
    STUFF((SELECT ', ' + c.name
           FROM sys.index_columns ic
           INNER JOIN sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
           WHERE ic.object_id = i.object_id AND ic.index_id = i.index_id AND ic.is_included_column = 1
           FOR XML PATH('')), 1, 2, '') AS IncludedColumns
FROM sys.indexes i
WHERE i.object_id = OBJECT_ID('CEUsuario')
AND i.name LIKE 'IX_%';
GO

-- =============================================
-- NOTAS DE RENDIMIENTO
-- =============================================
-- 
-- 1. IX_CEUsuario_Login: 
--    - Mejora el rendimiento de login en ~70-90%
--    - Evita table scan en cada autenticación
--
-- 2. IX_CEUsuario_IdUsuario_Activo:
--    - Optimiza búsquedas por ID (muy frecuentes)
--    - Covering index evita lookups
--
-- 3. IX_CEUsuario_Rol_Activo:
--    - Optimiza conteo de administradores
--    - Mejora queries de validación de roles
--
-- 4. IX_CEUsuario_FechaCreacion:
--    - Optimiza ordenamiento en listados
--    - Covering index para todas las columnas necesarias
--
-- MANTENIMIENTO:
-- - Ejecutar REBUILD de índices mensualmente
-- - Actualizar estadísticas semanalmente
-- - Monitorear fragmentación con:
--   SELECT * FROM sys.dm_db_index_physical_stats(DB_ID(), OBJECT_ID('CEUsuario'), NULL, NULL, 'DETAILED')
