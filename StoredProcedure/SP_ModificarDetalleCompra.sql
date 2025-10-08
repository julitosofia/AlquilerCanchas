USE AlquilerCanchas
GO

CREATE PROCEDURE SP_ModificarDetalleCompra
	@IdDetalle INT,
	@IdCompra INT,
	@NombreProducto NVARCHAR(100),
	@IdProducto INT,
	@Cantidad INT,
	@PrecioUnitario DECIMAL(10,2),
	@Categoria NVARCHAR(100)
AS
BEGIN
	UPDATE DetalleCompra
	SET IdCompra = @IdCompra,
		NombreProducto = @NombreProducto,
		IdProducto = @IdProducto,
		Cantidad = @Cantidad,
		PrecioUnitario = @PrecioUnitario,
		Categoria = @Categoria
	WHERE IdDetalle = @IdDetalle;
END
