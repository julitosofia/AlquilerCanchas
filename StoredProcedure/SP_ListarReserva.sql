USE AlquilerCanchas
GO

CREATE PROCEDURE SP_ListarReserva
AS
BEGIN
	SET NOCOUNT ON;
	SELECT r.IdReserva, r.Fecha, r.HoraInicio, r.HoraFin, r.Total,
		   u.Nombre AS Usuario, ca.Tipo AS Cancha
	FROM Reserva r
	INNER JOIN Usuario u ON r.IdUsuario = u.IdUsuario
	INNER JOIN Cancha ca ON r.IdCancha = ca.IdCancha;
END
