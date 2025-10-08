USE AlquilerCanchas
GO

CREATE PROCEDURE SP_VerificarDisponibilidadReserva
	@IdCancha INT,
	@Fecha DATETIME,
	@HoraInicio DATETIME,
	@HoraFin DATETIME
AS
BEGIN
	SET NOCOUNT ON;
	IF EXISTS (
		SELECT 1 FROM Reserva
		WHERE IdCancha = @IdCancha
		  AND Fecha = @Fecha
		  AND (
		      (@HoraInicio BETWEEN HoraInicio AND HoraFin) OR
			  (@HoraFin BETWEEN HoraInicio AND HoraFin) OR
			  (HoraInicio BETWEEN @HoraInicio AND @HoraFin)
		  )
		  AND Estado = 'Pendiente'
	)
		SELECT 0 AS Disponible;
	ELSE
		SELECT 1 AS Disponible;
END
