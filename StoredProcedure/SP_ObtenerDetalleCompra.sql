use AlquilerCanchas
GO

CREATE PROCEDURE SP_ObtenerDetalleCompra
AS
BEGIN
	SELECT E.IdDetalle,
		   B.IdCompra AS Compras,
		   E.NombreProducto,
		   C.IdProducto AS Producto,
		   E.Cantidad,
		   E.PrecioUnitario,
		   E.Categoria
	FROM DetalleCompra E
	INNER JOIN Compras B ON E.IdCompra = B.IdCompra
	INNER JOIN Producto C ON E.IdProducto = C.IdProducto
	ORDER BY E.NombreProducto DESC;
END
