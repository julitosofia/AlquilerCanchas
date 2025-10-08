USE AlquilerCanchas
GO

CREATE PROCEDURE SP_ObtenerDetallePorCompra
	@IdVenta INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT vd.IdProducto, p.Nombre, vd.Cantidad, vd.PrecioUnitario, vd.Subtotal
	FROM VentaDetalle vd
	inner join Producto p ON vd.IdProducto = P.IdProducto
	WHERE vd.IdVenta = @IdVenta;
end
