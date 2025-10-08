USE AlquilerCanchas
GO

CREATE PROCEDURE SP_EliminarDetalleCompra
	@IdDetalle INT
AS
BEGIN
	DELETE FROM DetalleCompra
	WHERE IdDetalle = @IdDetalle;
END
