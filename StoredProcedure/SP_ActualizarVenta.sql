USE AlquilerCanchas
GO

CREATE PROCEDURE SP_ActualizarVenta
	@IdVenta INT,
	@IdUsuario INT,
	@Fecha DATETIME
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE Venta
	SET IdUsuario = @IdUsuario,
	    Fecha = @Fecha
	WHERE IdVenta = @IdVenta;
END
