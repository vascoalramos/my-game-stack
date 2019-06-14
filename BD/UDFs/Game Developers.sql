drop function GamesDB.gameDevelopers;
go

go
create function GamesDB.gameDevelopers (@gameID int) returns Table
as
	return (select Name
			from GamesDB.GameDeveloper join GamesDB.Developers on GamesDB.GameDeveloper.DeveloperID=GamesDB.Developers.DeveloperID
			where GamesDB.GameDeveloper.GameID=@gameID
			)
go