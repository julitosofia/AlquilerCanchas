use AlquilerCanchas
GO

CREATE PROCEDURE SP_ExisteNombreUsuario
	@Nombre NVARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;
	IF EXISTS(
		SELECT 1 FROM Usuario WHERE Nombre = @Nombre
	)
		SELECT 1 AS Existe;
	ELSE
		SELECT 0 AS Existe;
END
