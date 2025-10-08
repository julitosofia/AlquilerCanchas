USE AlquilerCanchas
GO

CREATE PROCEDURE SP_InsertarDetalleVenta
    @IdVenta INT,
    @IdProducto INT,
    @Cantidad INT,
    @PrecioUnitario DECIMAL(10,2)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO VentaDetalle (IdVenta, IdProducto, Cantidad, PrecioUnitario)
    VALUES (@IdVenta, @IdProducto, @Cantidad, @PrecioUnitario);
END