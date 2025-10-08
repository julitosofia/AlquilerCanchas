USE AlquilerCanchas
GO

CREATE PROCEDURE SP_ObtenerCompra
AS
BEGIN
	SELECT IdCompra, Cliente, Fecha
	FROM Compras
	ORDER BY Cliente;
END
