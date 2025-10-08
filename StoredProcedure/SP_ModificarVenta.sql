USE AlquilerCanchas
GO

CREATE PROCEDURE SP_ModificarVenta
	@IdVenta INT,
	@IdProducto INT,
	@Cantidad INT,
	@Fecha DATETIME,
	@Total DECIMAL(10,2),
	@IdUsuario INT,
	@PrecioUnitario DECIMAL(10,2)
AS
BEGIN
	UPDATE Venta
	SET IdProducto= @IdProducto,
		Cantidad = @Cantidad,
		Fecha = @Fecha,
		Total = @Total,
		IdUsuario = @IdUsuario,
		PrecioUnitario = @PrecioUnitario
	WHERE IdVenta = @IdVenta;
END
