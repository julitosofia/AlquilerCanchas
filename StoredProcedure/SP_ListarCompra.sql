USE AlquilerCanchas
GO

CREATE PROCEDURE SP_ListarCompra
AS
BEGIN	
	SET NOCOUNT ON;
	SELECT v.IdVenta, v.Fecha, u.Nombre AS Usuario,
		   SUM(vd.Subtotal) AS Total
	FROM Venta v
	INNER JOIN Usuario u ON v.IdUsuario = u.IdUsuario
	INNER JOIN VentaDetalle vd ON v.IdVenta = vd.IdVenta
	GROUP BY v.IdVenta, v.Fecha, u.Nombre;
END
