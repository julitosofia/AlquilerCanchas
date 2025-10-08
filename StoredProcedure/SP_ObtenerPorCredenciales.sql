USE AlquilerCanchas
GO

CREATE PROCEDURE SP_ObtenerPorCredenciales
	@Nombre NVARCHAR(50),
	@ClaveHash NVARCHAR(256)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM Usuario
	WHERE Nombre = @Nombre AND Clave = @ClaveHash;
END
