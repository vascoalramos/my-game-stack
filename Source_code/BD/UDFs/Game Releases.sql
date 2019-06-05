drop function GamesDB.gameReleases;
go

go
create function GamesDB.gameReleases (@gameID int) returns Table
as
	return (select Name
			from GamesDB.Releases join GamesDB.Platforms on GamesDB.Releases.PlatformID=GamesDB.Platforms.PlatformID
			where GamesDB.Releases.GameID=@gameID
			)
go