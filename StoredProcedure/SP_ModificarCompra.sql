USE AlquilerCanchas
GO

CREATE PROCEDURE SP_ModificarCompra
	@IdCompra INT,
	@Cliente NVARCHAR(100),
	@Fecha DATETIME
AS
BEGIN
	UPDATE Compras
	SET Cliente = @Cliente,
		Fecha = @Fecha
	WHERE IdCompra = @IdCompra;
END
