CREATE PROCEDURE SP_ObtenerPorCredencialesPlano
    @Nombre NVARCHAR(50),
    @Clave NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM Usuario
    WHERE Nombre = @Nombre AND Clave = @Clave;
END