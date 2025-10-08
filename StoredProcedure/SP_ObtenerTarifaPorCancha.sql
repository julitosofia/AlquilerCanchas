USE AlquilerCanchas
GO

CREATE PROCEDURE SP_ObtenerTarifaPorCancha
	@IdCancha INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT PrecioHora
	FROM Cancha
	WHERE IdCancha = @IdCancha;
END
