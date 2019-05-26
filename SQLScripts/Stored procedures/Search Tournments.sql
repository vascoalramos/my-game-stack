drop procedure GamesDB.uspSearchTournments;
go

go
create procedure [GamesDB].uspSearchTournments
	@pageSize int,
	@pageNumber int
as
	begin
		set nocount on;

		select TournmentID, Name, PrizePool, Location, GamesDB.[Tournments].GameID, Title
		from GamesDB.[Tournments] join GamesDB.[Games] on GamesDB.[Tournments].GameID=GamesDB.[Games].GameID
		order by TournmentID asc
		offset @pageSize * (@pageNumber - 1) rows
		fetch next @pageSize rows only option (recompile)
	end
go