USE AlquilerCanchas
GO

CREATE PROCEDURE SP_EliminarProducto
	@IdProducto INT
AS
BEGIN
	DELETE FROM Producto
	WHERE IdProducto = @IdProducto;
END
