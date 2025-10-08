USE AlquilerCanchas
GO

CREATE PROCEDURE SP_EliminarCompra
	@IdCompra INT
AS
BEGIN
	DELETE FROM Compras
	WHERE IdCompra = @IdCompra;
END
