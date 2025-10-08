USE AlquilerCanchas
GO

CREATE PROCEDURE SP_ObtenerVenta
AS
BEGIN
	SELECT A.IdVenta,
		   B.IdProducto AS Producto,
		   A.Cantidad,
		   A.Fecha,
		   A.Total,
		   C.IdUsuario AS Usuario,
		   A.PrecioUnitario
	FROM Venta A
	INNER JOIN Producto B ON A.IdProducto = B.IdProducto
	INNER JOIN Usuario C ON A.IdUsuario = C.IdUsuario
	ORDER BY A.Fecha DESC;
END
