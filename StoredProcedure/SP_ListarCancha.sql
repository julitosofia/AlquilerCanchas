USE AlquilerCanchas
GO

CREATE PROCEDURE SP_ListarCancha
AS
BEGIN
	SET NOCOUNT ON;

	SELECT IdCancha, Nombre, Tipo, PrecioHora
	FROM Cancha
	ORDER BY Nombre;
END
