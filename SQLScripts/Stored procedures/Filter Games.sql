drop procedure GamesDB.uspFilterGames;
go

go
create procedure [GamesDB].[uspFilterGames]
	@pageSize int,
	@pageNumber int,
	@opt varchar(30),
	@name varchar(65),
	@genreID varchar(30),
	@franID varchar(30),
	@pubID varchar(30)
as
	begin
		declare @tempTable table (
			GameID int,
			Title varchar(max) not null,
			LaunchDate date,
			[Description] varchar(max),
			PubID int,
			CoverImage varchar(max)
		)
		set nocount on;

		if @name <> 'None'
		begin
			insert into @tempTable (GameID, Title, LaunchDate, [Description], PubID, CoverImage)
			select * from GamesDB.Games where Title like '%'+@name+'%'
		end

		else
		begin
			insert into @tempTable select * from GamesDB.Games
		end

		if @genreID <> 'None'
		begin
			insert into @tempTable
			select tt.*
			from @tempTable tt join GamesDB.GameGenre on tt.GameID=GamesDB.GameGenre.GameID
			where GenreID = @genreID and not exists (select * from GamesDB.Games where GameID=GamesDB.GameGenre.GameID)

			delete from @tempTable
			where GameID not in (	select GamesDB.Games.GameID
								from GamesDB.Games join GamesDB.GameGenre on GamesDB.Games.GameID=GamesDB.GameGenre.GameID
								where GenreID = @genreID)
 		end

		if @franID <> 'None'
		begin
			insert into @tempTable
			select tt.*
			from @tempTable tt join GamesDB.GameBelongsFranchise on tt.GameID=GamesDB.GameBelongsFranchise.GameID
			where FranchiseID = @franID and not exists (select * from GamesDB.Games where GameID=GamesDB.GameBelongsFranchise.GameID)

			delete from @tempTable
			where GameID not in (	select GamesDB.Games.GameID
								from GamesDB.Games join GamesDB.GameBelongsFranchise on GamesDB.Games.GameID=GamesDB.GameBelongsFranchise.GameID
								where FranchiseID = @franID)
 		end

		if @pubID <> 'None'
		begin
			insert into @tempTable
			select tt.*
			from @tempTable tt
			where PubID = @pubID and not exists (select * from GamesDB.Games)

			delete from @tempTable
			where GameID not in (	select GamesDB.Games.GameID
								from GamesDB.Games
								where PubID = @pubID)
 		end

		if @opt = 'LaunchDateDesc'
		begin
			select tt.* from @tempTable tt
			where tt.LaunchDate is not null
			order by tt.LaunchDate desc
			offset @pageSize * (@pageNumber - 1) rows
			fetch next @pageSize rows only option (recompile)
		end

		if @opt = 'LaunchDateAsc'
		begin
			select tt.* from @tempTable tt
			where tt.LaunchDate is not null
			order by tt.LaunchDate asc
			offset @pageSize * (@pageNumber - 1) rows
			fetch next @pageSize rows only option (recompile)
		end
		
		if @opt = 'None'
		begin
			select tt.* from @tempTable tt
			order by GameID
			offset @pageSize * (@pageNumber - 1) rows
			fetch next @pageSize rows only option (recompile)
		end
	end
go