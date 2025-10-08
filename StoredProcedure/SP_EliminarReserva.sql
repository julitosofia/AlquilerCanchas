use AlquilerCanchas
GO

CREATE PROCEDURE SP_EliminarReserva
	@IdReserva INT
AS
BEGIN
	DELETE FROM Reserva
	WHERE IdReserva = @IdReserva;
END
