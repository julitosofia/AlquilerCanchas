use AlquilerCanchas
GO

CREATE PROCEDURE SP_InsertarReserva
	@IdCancha INT,
	@Cliente NVARCHAR(50),
	@Fecha DATETIME,
	@HoraIncio DATETIME,
	@HoraFin DATETIME,
	@Total DECIMAL(10,2),
	@Estado VARCHAR(20),
	@IdUsuario INT
AS
BEGIN
	INSERT INTO Reserva(IdCancha, Cliente, Fecha, HoraInicio, HoraFin, Total, Estado, IdUsuario)
	VALUES (@IdCancha, @Cliente, @Fecha, @HoraIncio, @HoraFin, @Total, @Estado, @IdUsuario);
END
