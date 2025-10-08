USE AlquilerCanchas
GO

CREATE PROCEDURE SP_CancelarReserva
	@IdReserva INT
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE Reserva
	SET Estado = 'Cancelada'
	WHERE IdReserva = @IdReserva;
END
