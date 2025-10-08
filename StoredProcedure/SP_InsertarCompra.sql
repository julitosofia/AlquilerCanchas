USE AlquilerCanchas
GO

CREATE PROCEDURE SP_InsertarCompra
	@Cliente NVARCHAR(100),
	@Fecha DATETIME
AS
BEGIN
	INSERT INTO Compras(Cliente,Fecha)
	VALUES (@Cliente, @Fecha);
END
