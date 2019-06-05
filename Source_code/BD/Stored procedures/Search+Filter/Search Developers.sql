drop procedure GamesDB.uspSearchDevelopers;
go

go
create procedure [GamesDB].uspSearchDevelopers
	@pageSize int,
	@pageNumber int
as
	begin
		set nocount on;

		select DeveloperID, Name, City, Country, Logo from GamesDB.[Developers]
		order by DeveloperID asc
		offset @pageSize * (@pageNumber - 1) rows
		fetch next @pageSize rows only option (recompile)
	end
go