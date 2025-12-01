-- ============================================
-- SCRIPT DE ACTUALIZACIÓN DE BASE DE DATOS
-- Sistema: AGCV (Control de Joy-Cons)
-- Propósito: Actualizar base de datos DBVideojuegos con funcionalidad de historial
-- Autor: Sistema AGCV
-- Fecha: 2024
-- Versión: 2.0
-- ============================================

USE DBVideojuegos;
GO

PRINT '============================================';
PRINT 'INICIANDO ACTUALIZACIÓN DE BASE DE DATOS';
PRINT 'Sistema AGCV - Historial de Conexiones';
PRINT '============================================';
GO

-- ============================================
-- PASO 1: Verificar y actualizar tabla CEHistorial
-- ============================================

PRINT '';
PRINT 'PASO 1: Verificando tabla CEHistorial...';

-- Verificar si la tabla ya existe
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CEHistorial]') AND type in (N'U'))
BEGIN
    PRINT '   ? La tabla CEHistorial ya existe.';
    
    -- Verificar si tiene la estructura correcta
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[CEHistorial]') AND name = 'Detalles')
    BEGIN
        PRINT '   ? Actualizando estructura de CEHistorial...';
        
        -- Agregar columna Detalles si no existe
        ALTER TABLE CEHistorial 
        ADD Detalles NVARCHAR(MAX) NULL;
        
        PRINT '   ? Columna Detalles agregada.';
    END
    
    -- Actualizar el tamaño de la columna Accion si es necesario
    IF EXISTS (
        SELECT * FROM sys.columns c
        INNER JOIN sys.types t ON c.user_type_id = t.user_type_id
        WHERE c.object_id = OBJECT_ID(N'[dbo].[CEHistorial]') 
        AND c.name = 'Accion'
        AND c.max_length < 400  -- 200 * 2 (nvarchar usa 2 bytes por carácter)
    )
    BEGIN
        PRINT '   ? Expandiendo columna Accion a NVARCHAR(200)...';
        ALTER TABLE CEHistorial 
        ALTER COLUMN Accion NVARCHAR(200) NOT NULL;
        PRINT '   ? Columna Accion actualizada.';
    END
    
    -- Actualizar el tamaño de la columna Detalles si es necesario
    IF EXISTS (
        SELECT * FROM sys.columns c
        INNER JOIN sys.types t ON c.user_type_id = t.user_type_id
        WHERE c.object_id = OBJECT_ID(N'[dbo].[CEHistorial]') 
        AND c.name = 'Detalles'
        AND t.name = 'nvarchar'
        AND c.max_length > 0  -- Si no es MAX
    )
    BEGIN
        PRINT '   ? Expandiendo columna Detalles a NVARCHAR(MAX)...';
        ALTER TABLE CEHistorial 
        ALTER COLUMN Detalles NVARCHAR(MAX) NULL;
        PRINT '   ? Columna Detalles actualizada.';
    END
END
ELSE
BEGIN
    PRINT '   ? Creando tabla CEHistorial...';
    
    -- Crear tabla de historial
    CREATE TABLE [dbo].[CEHistorial](
        [IdHistorial] [int] IDENTITY(1,1) NOT NULL,
        [IdUsuario] [int] NOT NULL,
        [Accion] [nvarchar](200) NOT NULL,
        [Detalles] [nvarchar](MAX) NULL,
        [FechaRegistro] [datetime2](7) NOT NULL DEFAULT GETDATE(),
        CONSTRAINT [PK_CEHistorial] PRIMARY KEY CLUSTERED ([IdHistorial] ASC)
    );
    
    PRINT '   ? Tabla CEHistorial creada exitosamente.';
END
GO

-- ============================================
-- PASO 2: Verificar y crear Foreign Key
-- ============================================

PRINT '';
PRINT 'PASO 2: Verificando relaciones de tabla...';

-- Verificar si existe la Foreign Key
IF NOT EXISTS (
    SELECT * FROM sys.foreign_keys 
    WHERE object_id = OBJECT_ID(N'[dbo].[FK_CEHistorial_CEUsuario]')
)
BEGIN
    PRINT '   ? Creando Foreign Key con CEUsuario...';
    
    ALTER TABLE [dbo].[CEHistorial]  
    WITH CHECK ADD CONSTRAINT [FK_CEHistorial_CEUsuario] 
    FOREIGN KEY([IdUsuario])
    REFERENCES [dbo].[CEUsuario] ([IdUsuario])
    ON DELETE CASCADE;
    
    ALTER TABLE [dbo].[CEHistorial] 
    CHECK CONSTRAINT [FK_CEHistorial_CEUsuario];
    
    PRINT '   ? Foreign Key creada exitosamente.';
END
ELSE
BEGIN
    PRINT '   ? Foreign Key ya existe.';
END
GO

-- ============================================
-- PASO 3: Crear/Actualizar Índices
-- ============================================

PRINT '';
PRINT 'PASO 3: Verificando índices...';

-- Índice: IdUsuario + FechaRegistro (para consultas por usuario ordenadas por fecha)
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_CEHistorial_IdUsuario_FechaRegistro')
BEGIN
    PRINT '   ? Creando índice IX_CEHistorial_IdUsuario_FechaRegistro...';
    
    CREATE NONCLUSTERED INDEX [IX_CEHistorial_IdUsuario_FechaRegistro] 
    ON [dbo].[CEHistorial] ([IdUsuario] ASC, [FechaRegistro] DESC);
    
    PRINT '   ? Índice creado.';
END
ELSE
BEGIN
    PRINT '   ? Índice IX_CEHistorial_IdUsuario_FechaRegistro ya existe.';
END

-- Índice: Accion (para filtrar por tipo de acción)
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_CEHistorial_Accion')
BEGIN
    PRINT '   ? Creando índice IX_CEHistorial_Accion...';
    
    CREATE NONCLUSTERED INDEX [IX_CEHistorial_Accion] 
    ON [dbo].[CEHistorial] ([Accion] ASC);
    
    PRINT '   ? Índice creado.';
END
ELSE
BEGIN
    PRINT '   ? Índice IX_CEHistorial_Accion ya existe.';
END

-- Índice: FechaRegistro (ya existe en tu script original, verificar si está)
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_CEHistorial_FechaRegistro')
BEGIN
    -- Verificar si ya existe el índice con otro nombre
    IF NOT EXISTS (
        SELECT * FROM sys.indexes i
        INNER JOIN sys.index_columns ic ON i.object_id = ic.object_id AND i.index_id = ic.index_id
        INNER JOIN sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
        WHERE i.object_id = OBJECT_ID('CEHistorial')
        AND c.name = 'FechaRegistro'
        AND i.name != 'IX_CEHistorial_IdUsuario_FechaRegistro'
    )
    BEGIN
        PRINT '   ? Creando índice IX_CEHistorial_FechaRegistro...';
        
        CREATE NONCLUSTERED INDEX [IX_CEHistorial_FechaRegistro]
        ON [dbo].[CEHistorial] ([FechaRegistro] DESC);
        
        PRINT '   ? Índice creado.';
    END
    ELSE
    BEGIN
        PRINT '   ? Ya existe un índice sobre FechaRegistro.';
    END
END
ELSE
BEGIN
    PRINT '   ? Índice IX_CEHistorial_FechaRegistro ya existe.';
END
GO

-- ============================================
-- PASO 4: Actualizar/Crear Procedimientos Almacenados
-- ============================================

PRINT '';
PRINT 'PASO 4: Actualizando procedimientos almacenados...';

-- Procedimiento: Registrar Acción en Historial
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_RegistrarAccion]') AND type in (N'P'))
BEGIN
    PRINT '   ? Actualizando SP_RegistrarAccion...';
    DROP PROCEDURE [dbo].[SP_RegistrarAccion];
END
ELSE
BEGIN
    PRINT '   ? Creando SP_RegistrarAccion...';
END
GO

CREATE PROCEDURE [dbo].[SP_RegistrarAccion]
    @IdUsuario INT,
    @Accion NVARCHAR(200),
    @Detalles NVARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO CEHistorial (IdUsuario, Accion, Detalles, FechaRegistro)
    VALUES (@IdUsuario, @Accion, @Detalles, GETDATE());
    
    SELECT SCOPE_IDENTITY() AS IdHistorial;
END
GO

PRINT '   ? SP_RegistrarAccion listo.';
GO

-- Procedimiento: Historial Por Usuario
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_HistorialPorUsuario]') AND type in (N'P'))
BEGIN
    PRINT '   ? Actualizando SP_HistorialPorUsuario...';
    DROP PROCEDURE [dbo].[SP_HistorialPorUsuario];
END
ELSE
BEGIN
    PRINT '   ? Creando SP_HistorialPorUsuario...';
END
GO

CREATE PROCEDURE [dbo].[SP_HistorialPorUsuario]
    @IdUsuario INT,
    @Limite INT = 500
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT TOP (@Limite)
        H.IdHistorial,
        H.IdUsuario,
        U.NombreUsuario,
        H.Accion,
        H.Detalles,
        H.FechaRegistro
    FROM CEHistorial H WITH (NOLOCK)
    INNER JOIN CEUsuario U WITH (NOLOCK) ON H.IdUsuario = U.IdUsuario
    WHERE H.IdUsuario = @IdUsuario
    ORDER BY H.FechaRegistro DESC;
END
GO

PRINT '   ? SP_HistorialPorUsuario listo.';
GO

-- Procedimiento: Obtener Estadísticas del Historial
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_EstadisticasHistorial]') AND type in (N'P'))
BEGIN
    PRINT '   ? Actualizando SP_EstadisticasHistorial...';
    DROP PROCEDURE [dbo].[SP_EstadisticasHistorial];
END
ELSE
BEGIN
    PRINT '   ? Creando SP_EstadisticasHistorial...';
END
GO

CREATE PROCEDURE [dbo].[SP_EstadisticasHistorial]
    @IdUsuario INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        COUNT(*) AS TotalRegistros,
        SUM(CASE WHEN Accion LIKE '%Conexión%' OR Accion LIKE '%conexión%' THEN 1 ELSE 0 END) AS TotalConexiones,
        SUM(CASE WHEN Accion LIKE '%Desconexión%' OR Accion LIKE '%desconexión%' THEN 1 ELSE 0 END) AS TotalDesconexiones,
        SUM(CASE WHEN Accion LIKE '%Error%' OR Accion LIKE '%error%' THEN 1 ELSE 0 END) AS TotalErrores,
        MIN(FechaRegistro) AS PrimerRegistro,
        MAX(FechaRegistro) AS UltimoRegistro
    FROM CEHistorial WITH (NOLOCK)
    WHERE IdUsuario = @IdUsuario;
END
GO

PRINT '   ? SP_EstadisticasHistorial listo.';
GO

-- Procedimiento: Limpiar Historial de Usuario
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_LimpiarHistorial]') AND type in (N'P'))
BEGIN
    PRINT '   ? Actualizando SP_LimpiarHistorial...';
    DROP PROCEDURE [dbo].[SP_LimpiarHistorial];
END
ELSE
BEGIN
    PRINT '   ? Creando SP_LimpiarHistorial...';
END
GO

CREATE PROCEDURE [dbo].[SP_LimpiarHistorial]
    @IdUsuario INT,
    @DiasAntiguedad INT = NULL  -- NULL = limpiar todo, número = limpiar registros más antiguos que X días
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @RegistrosEliminados INT;
    
    IF @DiasAntiguedad IS NULL
    BEGIN
        -- Limpiar todo el historial del usuario
        DELETE FROM CEHistorial WHERE IdUsuario = @IdUsuario;
        SET @RegistrosEliminados = @@ROWCOUNT;
    END
    ELSE
    BEGIN
        -- Limpiar solo registros antiguos
        DELETE FROM CEHistorial 
        WHERE IdUsuario = @IdUsuario 
        AND FechaRegistro < DATEADD(DAY, -@DiasAntiguedad, GETDATE());
        SET @RegistrosEliminados = @@ROWCOUNT;
    END
    
    SELECT @RegistrosEliminados AS RegistrosEliminados;
END
GO

PRINT '   ? SP_LimpiarHistorial listo.';
GO

-- ============================================
-- PASO 5: Crear Vista de Historial Completo
-- ============================================

PRINT '';
PRINT 'PASO 5: Verificando vistas...';

-- Vista: Historial Completo (para administradores)
IF EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[VW_HistorialCompleto]'))
BEGIN
    PRINT '   ? Actualizando VW_HistorialCompleto...';
    DROP VIEW [dbo].[VW_HistorialCompleto];
END
ELSE
BEGIN
    PRINT '   ? Creando VW_HistorialCompleto...';
END
GO

CREATE VIEW [dbo].[VW_HistorialCompleto]
AS
SELECT 
    H.IdHistorial,
    H.IdUsuario,
    U.NombreUsuario,
    U.Rol AS RolUsuario,
    H.Accion,
    H.Detalles,
    H.FechaRegistro,
    CASE 
        WHEN H.Accion LIKE '%Error%' THEN 'Error'
        WHEN H.Accion LIKE '%Conexión%' THEN 'Conexión'
        WHEN H.Accion LIKE '%Desconexión%' THEN 'Desconexión'
        WHEN H.Accion LIKE '%Inicio%' THEN 'Inicio'
        WHEN H.Accion LIKE '%Cierre%' THEN 'Cierre'
        ELSE 'Otro'
    END AS TipoAccion
FROM CEHistorial H
INNER JOIN CEUsuario U ON H.IdUsuario = U.IdUsuario;
GO

PRINT '   ? VW_HistorialCompleto lista.';
GO

-- ============================================
-- PASO 6: Insertar Datos de Prueba (OPCIONAL)
-- ============================================

PRINT '';
PRINT 'PASO 6: Verificando datos de prueba...';

-- Verificar si ya existen registros de historial
DECLARE @TieneRegistros INT;
SELECT @TieneRegistros = COUNT(*) FROM CEHistorial;

IF @TieneRegistros = 0
BEGIN
    PRINT '   ? Insertando datos de prueba en el historial...';
    
    -- Obtener el ID del usuario administrador
    DECLARE @IdUsuarioAdmin INT;
    SELECT TOP 1 @IdUsuarioAdmin = IdUsuario FROM CEUsuario WHERE Rol = 'Administrador' AND Activo = 1;
    
    IF @IdUsuarioAdmin IS NOT NULL
    BEGIN
        -- Insertar registros de ejemplo
        INSERT INTO CEHistorial (IdUsuario, Accion, Detalles, FechaRegistro)
        VALUES 
            (@IdUsuarioAdmin, 'Inicio de sesión', 'Usuario: ' + (SELECT NombreUsuario FROM CEUsuario WHERE IdUsuario = @IdUsuarioAdmin) + ', Rol: Administrador', DATEADD(HOUR, -2, GETDATE())),
            (@IdUsuarioAdmin, 'Inicio de motor AGCV', 'Motor BetterJoy iniciado correctamente', DATEADD(HOUR, -1, GETDATE())),
            (@IdUsuarioAdmin, 'Cierre de motor AGCV', 'Motor BetterJoy cerrado', DATEADD(MINUTE, -30, GETDATE())),
            (@IdUsuarioAdmin, 'Cierre de sesión', 'Usuario: ' + (SELECT NombreUsuario FROM CEUsuario WHERE IdUsuario = @IdUsuarioAdmin), DATEADD(MINUTE, -15, GETDATE()));
        
        PRINT '   ? ' + CAST(@@ROWCOUNT AS NVARCHAR(10)) + ' registros de prueba insertados.';
    END
    ELSE
    BEGIN
        PRINT '   ? No se encontró un usuario administrador activo para insertar datos de prueba.';
    END
END
ELSE
BEGIN
    PRINT '   ? Ya existen ' + CAST(@TieneRegistros AS NVARCHAR(10)) + ' registros en el historial.';
END
GO

-- ============================================
-- PASO 7: Verificación Final
-- ============================================

PRINT '';
PRINT '============================================';
PRINT 'VERIFICACIÓN FINAL';
PRINT '============================================';

-- Verificar estructura de la tabla
SELECT 
    t.name AS 'Tabla',
    c.name AS 'Columna',
    ty.name AS 'Tipo de Dato',
    CASE 
        WHEN ty.name IN ('nvarchar', 'varchar', 'char', 'nchar') AND c.max_length = -1 THEN 'MAX'
        WHEN ty.name IN ('nvarchar', 'nchar') THEN CAST(c.max_length / 2 AS NVARCHAR(10))
        WHEN ty.name IN ('varchar', 'char') THEN CAST(c.max_length AS NVARCHAR(10))
        ELSE ''
    END AS 'Longitud',
    CASE WHEN c.is_nullable = 1 THEN 'Sí' ELSE 'No' END AS 'Permite NULL',
    CASE WHEN c.is_identity = 1 THEN 'Sí' ELSE 'No' END AS 'Es Identity'
FROM sys.tables t
INNER JOIN sys.columns c ON t.object_id = c.object_id
INNER JOIN sys.types ty ON c.user_type_id = ty.user_type_id
WHERE t.name = 'CEHistorial'
ORDER BY c.column_id;

-- Verificar índices
PRINT '';
PRINT 'Índices de la tabla CEHistorial:';
SELECT 
    i.name AS 'Nombre del Índice',
    i.type_desc AS 'Tipo',
    STUFF((
        SELECT ', ' + c.name
        FROM sys.index_columns ic
        INNER JOIN sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
        WHERE ic.object_id = i.object_id AND ic.index_id = i.index_id
        ORDER BY ic.key_ordinal
        FOR XML PATH('')
    ), 1, 2, '') AS 'Columnas'
FROM sys.indexes i
WHERE i.object_id = OBJECT_ID('CEHistorial')
AND i.name IS NOT NULL
ORDER BY i.name;

-- Verificar procedimientos almacenados
PRINT '';
PRINT 'Procedimientos almacenados del historial:';
SELECT 
    name AS 'Procedimiento',
    create_date AS 'Fecha de Creación',
    modify_date AS 'Última Modificación'
FROM sys.procedures
WHERE name LIKE 'SP_%Historial%' OR name LIKE 'SP_RegistrarAccion%' OR name LIKE 'SP_EstadisticasHistorial%' OR name LIKE 'SP_LimpiarHistorial%'
ORDER BY name;

-- Mostrar estadísticas de la tabla
PRINT '';
PRINT 'Estadísticas de la tabla CEHistorial:';
SELECT 
    'CEHistorial' AS Tabla,
    COUNT(*) AS 'Total de Registros',
    COUNT(DISTINCT IdUsuario) AS 'Usuarios con Registros',
    MIN(FechaRegistro) AS 'Registro Más Antiguo',
    MAX(FechaRegistro) AS 'Registro Más Reciente'
FROM CEHistorial;

PRINT '';
PRINT '============================================';
PRINT '? ACTUALIZACIÓN COMPLETADA EXITOSAMENTE';
PRINT '============================================';
PRINT '';
PRINT 'Resumen:';
PRINT '  ? Tabla CEHistorial verificada/actualizada';
PRINT '  ? Índices creados/verificados';
PRINT '  ? Procedimientos almacenados actualizados';
PRINT '  ? Vistas creadas';
PRINT '  ? Foreign Keys configuradas';
PRINT '';
PRINT 'El sistema de historial está listo para usar.';
PRINT 'Puedes verificar el funcionamiento en la aplicación AGCV.';
PRINT '============================================';
GO
