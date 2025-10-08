CREATE PROCEDURE SP_InsertarReservas
    @IdCancha INT,
    @Cliente NVARCHAR(100),
    @Fecha DATETIME,
    @HoraInicio DATETIME,
    @HoraFin DATETIME,
    @Total DECIMAL(10,2),
    @Estado VARCHAR(20),
    @IdUsuario INT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Reserva (IdCancha, Cliente, Fecha, HoraInicio, HoraFin, Total, Estado, IdUsuario)
    VALUES (@IdCancha, @Cliente, @Fecha, @HoraInicio, @HoraFin, @Total, @Estado, @IdUsuario);

    SELECT SCOPE_IDENTITY() AS IdReserva;
END

