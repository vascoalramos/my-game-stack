drop procedure GamesDB.uspSearchGames;
go

go
create procedure [GamesDB].[uspSearchGames]
	@pageSize int,
	@pageNumber int
as
	begin
		set nocount on;

		select * from GamesDB.[Games]
		order by GameID asc
		offset @pageSize * (@pageNumber - 1) rows
		fetch next @pageSize rows only option (recompile)
	end
go