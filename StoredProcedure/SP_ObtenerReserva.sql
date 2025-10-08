USE AlquilerCanchas
GO

CREATE PROCEDURE SP_ObtenerReserva
AS
BEGIN
	SELECT E.IdReserva,
		   B.IdCancha AS Cancha,
		   E.Cliente,
		   E.Fecha,
		   E.HoraInicio,
		   E.HoraFin,
		   E.Total,
		   E.Estado,
		   V.IdUsuario AS Usuario
	FROM Reserva E
	INNER JOIN Cancha B ON E.IdCancha = B.IdCancha
	INNER JOIN Usuario V ON E.IdUsuario = V.IdUsuario
	ORDER BY E.Fecha DESC;
END
