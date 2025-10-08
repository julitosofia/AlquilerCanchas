USE AlquilerCanchas
GO

CREATE PROCEDURE SP_InsertarCancha
	@Nombre NVARCHAR(50),
	@Tipo INT,
	@PrecioHora DECIMAL
AS
BEGIN
	INSERT INTO Cancha (Nombre, Tipo, PrecioHora)
	VALUES (@Nombre, @Tipo, @PrecioHora);
END
