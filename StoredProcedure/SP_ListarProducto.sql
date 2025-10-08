USE AlquilerCanchas
GO

CREATE PROCEDURE SP_ListarProducto
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		IdProducto,
		Nombre,
		Precio,
		Stock,
		Categoria
	FROM Producto
	ORDER BY Nombre;
END
