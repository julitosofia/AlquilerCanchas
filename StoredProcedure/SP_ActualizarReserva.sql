USE AlquilerCanchas
GO

CREATE PROCEDURE SP_ActualizarReserva
	@IdReserva INT,
	@Fecha DATETIME,
	@HoraInicio DATETIME,
	@HoraFin DATETIME,
	@Total DECIMAL(10,2),
	@Estado NVARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE Reserva
	SET Fecha = @Fecha,
		HoraInicio = @HoraInicio,
		HoraFin = @HoraFin,
		Total = @Total,
		Estado = @Estado
	WHERE IdReserva = @IdReserva;
END
