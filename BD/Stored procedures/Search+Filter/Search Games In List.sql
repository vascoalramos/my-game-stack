drop procedure GamesDB.uspSearchGamesInList;
go

go
create procedure [GamesDB].uspSearchGamesInList
	@listName varchar(100),
	@userName varchar(max)
as
	begin
		set nocount on;
		select *
		from GamesDB.Games join GamesDB.GameEventList on GamesDB.Games.GameID=GamesDB.GameEventList.GameID
			join GamesDB.[Events] on GamesDB.GameEventList.EventID=GamesDB.[Events].EventID
		where GamesDB.[Events].UserName=@userName and ChangeDate is null and  GamesDB.[Events].TypeID = (select TypeID from GamesDB.EventType where Name=@listName)
		order by GamesDB.Games.GameID asc
	end
go