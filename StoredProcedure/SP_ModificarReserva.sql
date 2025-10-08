USE AlquilerCanchas
GO

CREATE PROCEDURE SP_ModificarReserva
	@IdReserva INT,
	@IdCancha INT,
	@Cliente NVARCHAR(50),
	@Fecha DATETIME,
	@HoraInicio DATETIME,
	@HoraFin DATETIME,
	@Total DECIMAL(10,2),
	@Estado VARCHAR(20),
	@IdUsuario INT
AS
BEGIN
	UPDATE Reserva
	SET IdCancha = @IdCancha,
		Cliente = @Cliente,
		Fecha = @Fecha,
		HoraInicio = @HoraInicio,
		HoraFin = @HoraFin,
		Total = @Total,
		Estado = @Estado,
		IdUsuario = @IdUsuario
	WHERE IdReserva = @IdReserva;
END
