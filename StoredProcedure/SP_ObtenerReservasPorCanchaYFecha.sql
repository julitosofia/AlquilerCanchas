USE AlquilerCanchas
GO

CREATE PROCEDURE SP_ObtenerReservasPorCanchaYFecha
	@IdCancha INT,
	@Fecha DATE
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM Reserva
	WHERE IdCancha = @IdCancha AND CONVERT(DATE, Fecha) = @Fecha;
END
