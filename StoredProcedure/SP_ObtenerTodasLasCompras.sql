USE AlquilerCanchas
GO

CREATE PROCEDURE SP_ObtenerTodasLasCompras
AS
BEGIN
	SET NOCOUNT ON;
	SELECT v.IdVenta, v.Fecha, u.Nombre AS Usuario
	FROM Venta v
	INNER JOIN Usuario u ON v.IdUsuario = u.IdUsuario;
END
