use AlquilerCanchas
GO

CREATE PROCEDURE RegistrarDetalleCompra
    @IdCompra INT,
    @IdProducto INT,
    @NombreProducto NVARCHAR(100),
    @Cantidad INT,
    @PrecioUnitario DECIMAL(10,2),
    @Categoria NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO DetalleCompra (IdCompra, IdProducto, NombreProducto, Cantidad, PrecioUnitario, Categoria)
    VALUES (@IdCompra, @IdProducto, @NombreProducto, @Cantidad, @PrecioUnitario, @Categoria);
END