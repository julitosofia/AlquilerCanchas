USE AlquilerCanchas
GO

CREATE PROCEDURE SP_InsertarProductos
	@Nombre NVARCHAR(50),
	@Precio DECIMAL(10,2),
	@Stock INT,
	@Categoria NVARCHAR(50)
AS
BEGIN
	INSERT INTO Producto (Nombre, Precio, Stock, Categoria)
	VALUES (@Nombre, @Precio, @Stock, @Categoria);
END
