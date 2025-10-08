USE AlquilerCanchas
GO

CREATE PROCEDURE SP_ObtenerProducto
AS
BEGIN
	SELECT IdProducto, Nombre, Precio, Stock, Categoria
	FROM Producto
	ORDER BY Nombre;
END
