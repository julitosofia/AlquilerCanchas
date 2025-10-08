USE AlquilerCanchas
GO

CREATE PROCEDURE SP_ObtenerProductoPorId
    @IdProducto INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT IdProducto, Nombre, Precio, Stock, Categoria
    FROM Producto
    WHERE IdProducto = @IdProducto;
END
