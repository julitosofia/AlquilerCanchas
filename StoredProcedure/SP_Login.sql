USE AlquilerCanchas
GO

CREATE PROCEDURE SP_Login
	@Nombre NVARCHAR(50),
	@ClaveHash NVARCHAR(256)
AS
BEGIN
	SET NOCOUNT ON;
	IF EXISTS (
		SELECT 1 FROM Usuario
		WHERE Nombre = @Nombre AND Clave = @ClaveHash
	)
		SELECT 1 AS Autenticado;
	ELSE
		SELECT 0 AS Autenticado;
END
