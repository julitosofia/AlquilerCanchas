USE AlquilerCanchas
GO

CREATE PROCEDURE SP_ListarUsuarios
AS
BEGIN
	SET NOCOUNT ON;
	SELECT IdUsuario, Nombre, Rol FROM Usuario;
END
