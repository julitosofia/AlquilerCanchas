USE AlquilerCanchas
GO

CREATE PROCEDURE SP_EliminarVenta
	@IdVenta INT
AS
BEGIN
	DELETE FROM Venta
	WHERE IdVenta = @IdVenta;
END
