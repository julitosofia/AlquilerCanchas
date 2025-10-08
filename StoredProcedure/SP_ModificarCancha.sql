USE AlquilerCanchas
GO

CREATE PROCEDURE SP_ModificarCancha
	@IdCancha INT,
	@Nombre NVARCHAR(50),
	@Tipo INT,
	@PrecioHora DECIMAL
AS
BEGIN
	UPDATE Cancha
	SET Nombre = @Nombre,
		Tipo = @Tipo,
		PrecioHora = @PrecioHora
	WHERE IdCancha = @IdCancha;
END
