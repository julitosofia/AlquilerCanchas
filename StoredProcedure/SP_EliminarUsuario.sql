USE AlquilerCanchas
GO

CREATE PROCEDURE SP_EliminarUsuario
	@IdUsuario INT
AS
BEGIN
	DELETE FROM Usuario
	WHERE IdUsuario = @IdUsuario;
END
