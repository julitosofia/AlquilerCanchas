USE AlquilerCanchas
GO

CREATE PROCEDURE SP_InsertarVenta
	@IdProducto INT,
	@Cantidad int,
	@Fecha DATETIME,
	@Total DECIMAL(10,2),
	@IdUsuario INT,
	@PrecioUnitario DECIMAL (10,2)
AS
BEGIN
	INSERT INTO Venta(IdProducto, Cantidad, Fecha, Total, IdUsuario, PrecioUnitario)
	VALUES (@IdProducto, @Cantidad, @Fecha, @Total, @IdUsuario, @PrecioUnitario);
END
