USE AlquilerCanchas
GO

CREATE PROCEDURE SP_ModificarUsuario
	@IdUsuario INT,
	@Nombre NVARCHAR(50),
	@Rol NVARCHAR(50),
	@Clave NVARCHAR(50)
AS
BEGIN
	UPDATE Usuario
	SET Nombre = @Nombre,
		Rol = @Rol,
		Clave = @Clave
	WHERE IdUsuario = @IdUsuario;
END
