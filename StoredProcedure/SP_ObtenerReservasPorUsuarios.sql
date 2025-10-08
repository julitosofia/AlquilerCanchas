USE AlquilerCanchas
GO

CREATE PROCEDURE SP_ObtenerReservasPorUsuarios
    @IdUsuario INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        r.IdReserva,
        r.IdCancha,
        r.IdUsuario,
        r.Cliente AS NombreCliente,
        r.Fecha,
        r.HoraInicio,
        r.HoraFin,
        r.Total,
        c.Nombre AS NombreCancha,
        r.Estado
    FROM Reserva r
    INNER JOIN Cancha c ON r.IdCancha = c.IdCancha
    WHERE r.IdUsuario = @IdUsuario
    ORDER BY r.Fecha DESC;
END
