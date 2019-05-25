drop procedure GamesDB.uspFilterFranchises;
go

go
create procedure [GamesDB].uspFilterFranchises
	@pageSize int,
	@pageNumber int,
	@opt varchar(30),
	@name varchar(65)
as
	begin
		declare @tempTable table (
			FranchiseID int,
			Name varchar(max) not null,
			NoOfGames int,
			Logo varchar(max)
		)
		set nocount on;

		if @name <> 'None'
		begin
			insert into @tempTable (FranchiseID, Name, NoOfGames, Logo)
			select * from GamesDB.Franchises where Name like '%'+@name+'%'
		end

		else
		begin
			insert into @tempTable select * from GamesDB.Franchises
		end

		if @opt = 'NofGamesDesc'
		begin
			select tt.* from @tempTable tt
			where tt.NoOfGames is not null
			order by tt.NoOfGames desc
			offset @pageSize * (@pageNumber - 1) rows
			fetch next @pageSize rows only option (recompile)
		end

		if @opt = 'NofGamesAsc'
		begin
			select tt.* from @tempTable tt
			where tt.NoOfGames is not null
			order by tt.NoOfGames asc
			offset @pageSize * (@pageNumber - 1) rows
			fetch next @pageSize rows only option (recompile)
		end
		
		if @opt = 'None'
		begin
			select tt.* from @tempTable tt
			order by FranchiseID
			offset @pageSize * (@pageNumber - 1) rows
			fetch next @pageSize rows only option (recompile)
		end
	end
go

