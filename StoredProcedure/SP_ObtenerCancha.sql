USE AlquilerCanchas
GO

CREATE PROCEDURE SP_ObtenerCancha
AS
BEGIN
	SELECT IdCancha, Nombre, Tipo, PrecioHora
	FROM Cancha
	ORDER BY Nombre;
END
