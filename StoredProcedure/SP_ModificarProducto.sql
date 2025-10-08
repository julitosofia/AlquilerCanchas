USE AlquilerCanchas
GO

CREATE PROCEDURE SP_ModificarProducto
	@IdProducto int,
	@Nombre NVARCHAR(50),
	@Precio DECIMAL,
	@Stock int,
	@Categoria NVARCHAR(50)
AS
BEGIN
	UPDATE Producto
	SET Nombre = @Nombre,
		Precio = @Precio,
		Stock = @Stock,
		Categoria = @Categoria
	WHERE IdProducto = @IdProducto;
END
