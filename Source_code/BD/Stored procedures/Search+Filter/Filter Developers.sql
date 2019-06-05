drop procedure GamesDB.uspFilterDevelopers;
go

go
create procedure [GamesDB].uspFilterDevelopers
	@pageSize int,
	@pageNumber int,
	@name varchar(65)
as
	begin
		declare @tempTable table (
			DeveloperID int,
			Name varchar(max) not null,
			City varchar(max),
			Country varchar(max),
			Logo varchar(max)
		)
		set nocount on;

		if @name <> 'None'
		begin
			insert into @tempTable (DeveloperID, Name, City, Country, Logo)
			select DeveloperID, Name, City, Country, Logo from GamesDB.Developers where Name like '%'+@name+'%'
		end

		else
		begin
			insert into @tempTable select DeveloperID, Name, City, Country, Logo from GamesDB.Developers
		end
		
		select tt.* from @tempTable tt
		order by DeveloperID
		offset @pageSize * (@pageNumber - 1) rows
		fetch next @pageSize rows only option (recompile)
	end
go
