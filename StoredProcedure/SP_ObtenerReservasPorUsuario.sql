USE AlquilerCanchas;
GO

CREATE PROCEDURE SP_ObtenerReservasPorUsuario
    @NombreCliente NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        r.IdReserva,
        r.Fecha,
        r.HoraInicio,
        r.HoraFin,
        r.Total,
        r.Estado,
        c.Nombre AS Cancha
    FROM Reserva r
    INNER JOIN Cancha c ON r.IdCancha = c.IdCancha
    WHERE LOWER(r.Cliente) LIKE '%' + LOWER(@NombreCliente) + '%'
    ORDER BY r.Fecha DESC;
END

