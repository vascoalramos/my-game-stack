drop function GamesDB.checkGameInList;
go

go
create function GamesDB.checkGameInList (@userName varchar(100), @gameID int, @listName varchar(max)) returns int
as
	begin
		if (not exists( select *
						from GamesDB.Games join GamesDB.GameEventList on GamesDB.Games.GameID=GamesDB.GameEventList.GameID
							join GamesDB.[Events] on GamesDB.GameEventList.EventID=GamesDB.[Events].EventID
						where GamesDB.[Events].UserName=@userName and GamesDB.Games.GameID=@gameID and GamesDB.[Events].TypeID = (select TypeID from GamesDB.EventType where Name=@listName and ChangeDate is null)
					)
			)
			return 0
		return 1
	end
go
