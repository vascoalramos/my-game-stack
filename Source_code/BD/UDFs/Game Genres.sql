drop function GamesDB.gameGenres;
go

go
create function GamesDB.gameGenres (@gameID int) returns Table
as
	return (select Name
			from GamesDB.GameGenre join GamesDB.Genres on GamesDB.GameGenre.GenreID=GamesDB.Genres.GenreID
			where GamesDB.GameGenre.GameID=@gameID
			)
go