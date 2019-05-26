drop procedure GamesDB.uspFilterTournments;
go
exec [GamesDB].uspFilterTournments 10, 1, 'None', 'None', 'Counter'
go
create procedure [GamesDB].uspFilterTournments
	@pageSize int,
	@pageNumber int,
	@opt varchar(65),
	@name varchar(65),
	@title varchar(65)
as
	begin
		declare @tempTable table (
			TournmentID int,
			Name varchar(max) not null,
			PrizePool int,
			Location varchar(max),
			GameID int,
			Title varchar(max)
		)
		set nocount on;

		if @name <> 'None'
		begin
			insert into @tempTable (TournmentID, Name, PrizePool, Location, GameID, Title)
			select TournmentID, Name, PrizePool, Location, GamesDB.[Tournments].GameID, Title
			from GamesDB.[Tournments] join GamesDB.[Games] on GamesDB.[Tournments].GameID=GamesDB.[Games].GameID
			where Name like '%'+@name+'%'
		end

		else
		begin
			insert into @tempTable (TournmentID, Name, PrizePool, Location, GameID, Title)
			select TournmentID, Name, PrizePool, Location, GamesDB.[Tournments].GameID, Title
			from GamesDB.[Tournments] join GamesDB.[Games] on GamesDB.[Tournments].GameID=GamesDB.[Games].GameID
		end

		if @title <> 'None'
		begin
			insert into @tempTable (TournmentID, Name, PrizePool, Location, GameID, Title)
			select distinct TournmentID, Name, PrizePool, Location, tt.GameID, GamesDB.[Games].Title
			from @tempTable tt join GamesDB.[Games] on tt.GameID=GamesDB.[Games].GameID
			where GamesDB.[Games].Title like '%'+@title+'%' and not exists (select tt.* from @tempTable tt)

			delete from @tempTable
			where TournmentID not in (	select GamesDB.Tournments.TournmentID
										from GamesDB.[Tournments] join GamesDB.[Games] on GamesDB.[Tournments].GameID=GamesDB.[Games].GameID
										where Title like '%'+@title+'%')
		end

		if @opt = 'PrizePoolAsc'
		begin
			select tt.* from @tempTable tt
			where tt.PrizePool is not null
			order by tt.PrizePool asc
			offset @pageSize * (@pageNumber - 1) rows
			fetch next @pageSize rows only option (recompile)
		end

		if @opt = 'PrizePoolDesc'
		begin
			select tt.* from @tempTable tt
			where tt.PrizePool is not null
			order by tt.PrizePool desc
			offset @pageSize * (@pageNumber - 1) rows
			fetch next @pageSize rows only option (recompile)
		end
		
		if @opt = 'None'
		begin
			select tt.* from @tempTable tt
			order by tt.TournmentID
			offset @pageSize * (@pageNumber - 1) rows
			fetch next @pageSize rows only option (recompile)
		end
	end
go
