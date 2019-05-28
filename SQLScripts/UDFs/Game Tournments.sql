drop function GamesDB.gameTournments;
go

go
create function GamesDB.gameTournments (@gameID int) returns Table
as
	return (select Name
			from GamesDB.Tournments
			where GamesDB.Tournments.GameID=@gameID
			)
go