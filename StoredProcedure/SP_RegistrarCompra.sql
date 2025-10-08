USE AlquilerCanchas
GO

CREATE PROCEDURE RegistrarCompra
    @Cliente NVARCHAR(100),
    @Fecha DATETIME
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Compras (Cliente, Fecha)
    VALUES (@Cliente, @Fecha);

    SELECT SCOPE_IDENTITY();
END
