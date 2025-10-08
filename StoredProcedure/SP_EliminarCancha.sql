USE AlquilerCanchas
GO

CREATE PROCEDURE SP_EliminarCancha
	@IdCancha INT
AS
BEGIN
	DELETE FROM Cancha
	WHERE IdCancha = @IdCancha;
END
