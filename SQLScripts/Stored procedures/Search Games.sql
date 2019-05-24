drop procedure GamesDB.uspSearchGames;
go

go
create procedure [GamesDB].[uspSearchGames]
	@pageSize int,
	@pageNumber int,
	@opt varchar(30)
as
	begin
		set nocount on;

		if @opt = 'LaunchDateDesc'
		begin
			select * from GamesDB.[Games]
			order by GameID asc
			offset @pageSize * (@pageNumber - 1) rows
			fetch next @pageSize rows only option (recompile)
		end

		if @opt = 'LaunchDateAsc'
		begin
			select * from GamesDB.[Games]
			order by LauchDate asc
			offset @pageSize * (@pageNumber - 1) rows
			fetch next @pageSize rows only option (recompile)
		end
		
		select * from GamesDB.[Games]
		order by LauchDate desc
		offset @pageSize * (@pageNumber - 1) rows
		fetch next @pageSize rows only option (recompile)
	end
go