USE AlquilerCanchas
GO

CREATE PROCEDURE SP_InsertarDetalleCompra
	@IdCompra INT,
	@NombreProducto NVARCHAR(100),
	@IdProducto INT,
	@Cantidad INT,
	@PrecioUnitario DECIMAL(10,2),
	@Categoria NVARCHAR(100)
AS
BEGIN
	INSERT INTO DetalleCompra (IdCompra, NombreProducto, IdProducto, Cantidad, PrecioUnitario, Categoria)
	VALUES (@IdCompra, @NombreProducto,@IdProducto, @Cantidad, @PrecioUnitario, @Categoria);
END
