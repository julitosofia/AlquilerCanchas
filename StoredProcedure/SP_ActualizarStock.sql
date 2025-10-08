USE AlquilerCanchas
GO

CREATE PROCEDURE SP_ActualizarStock
    @IdProducto INT,
    @Cantidad INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Producto
    SET Stock = Stock - @Cantidad
    WHERE IdProducto = @IdProducto;
END