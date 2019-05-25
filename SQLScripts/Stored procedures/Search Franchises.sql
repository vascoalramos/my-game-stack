drop procedure GamesDB.uspSearchFranchises;
go

go
create procedure [GamesDB].uspSearchFranchises
	@pageSize int,
	@pageNumber int
as
	begin
		set nocount on;

		select * from GamesDB.[Franchises]
		order by GameID asc
		offset @pageSize * (@pageNumber - 1) rows
		fetch next @pageSize rows only option (recompile)
	end
go