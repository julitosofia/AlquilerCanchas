USE AlquilerCanchas
GO

CREATE PROCEDURE SP_ObtenerUsuario
AS
BEGIN
	SELECT IdUsuario, Nombre, Rol, Clave
	FROM Usuario
	ORDER BY Nombre;
END
