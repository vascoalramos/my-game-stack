drop function GamesDB.gameInfo;
go

go
create function GamesDB.gameInfo (@gameID int) returns Table
as
	return (select Title, LauchDate, [Description], Name as PubName 
			from GamesDB.[Games] join GamesDB.Publishers on GamesDB.[Games].PubID=GamesDB.Publishers.PublisherID
			where GamesDB.[Games].GameID=@gameID
			)
go