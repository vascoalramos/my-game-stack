drop procedure GamesDB.uspFilterGames;
go

go
create procedure [GamesDB].[uspFilterGames]
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
		
		if @opt = 'None'
		begin
			select * from GamesDB.[Games]
			order by LauchDate desc
			offset @pageSize * (@pageNumber - 1) rows
			fetch next @pageSize rows only option (recompile)
		end
		
	end
go