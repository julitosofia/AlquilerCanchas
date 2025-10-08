USE AlquilerCanchas;
GO

-- 1. Eliminar si ya existe el Stored Procedure para recrearlo
IF OBJECT_ID('dbo.SP_InsertaarReservas', 'P') IS NOT NULL
    DROP PROCEDURE dbo.SP_InsertaarReservas;
GO

-- 2. Creación del Stored Procedure
CREATE PROCEDURE dbo.SP_InsertaarReservas
    -- El nombre del parámetro debe coincidir con el código C# (ej. @IdCancha, @Cliente)
    @IdCancha INT,
    @Cliente NVARCHAR(200), -- Ajusta el tamaño de NVARCHAR si es necesario
    @Fecha DATETIME,
    @HoraInicio DATETIME,
    @HoraFin DATETIME,
    @Total DECIMAL(10, 2), -- Ajusta la precisión si es necesario
    @Estado VARCHAR(20),   -- Ajusta el tamaño de VARCHAR si es necesario
    @IdUsuario INT
AS
BEGIN
    -- Asegúrate de que el INSERT NO mencione la columna IdReserva, 
    -- ya que es IDENTITY y la DB la generará automáticamente.
    INSERT INTO dbo.Reserva (
        IdCancha,
        Cliente,
        Fecha,
        HoraInicio,
        HoraFin,
        Total,
        Estado,
        IdUsuario
    )
    VALUES (
        @IdCancha,
        @Cliente,
        @Fecha,
        @HoraInicio,
        @HoraFin,
        @Total,
        @Estado,
        @IdUsuario
    );

    -- Devolver el ID generado
    -- SCOPE_IDENTITY() devuelve el último ID generado en la sesión actual
    SELECT SCOPE_IDENTITY() AS IdReserva;
END
GO