drop function GamesDB.checkGameInUser;
go

go
create function GamesDB.checkGameInUser (@userName varchar(30), @gameID int) returns int
as
	begin
		if (not exists( select *
					from GamesDB.[GameEventList]
					where UserName=@userName and GameID=@gameID
					)
			)
			return 0
		return 1
	end
go
