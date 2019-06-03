drop procedure GamesDB.uspSearchPublishers;
go

go
create procedure [GamesDB].uspSearchPublishers
	@pageSize int,
	@pageNumber int
as
	begin
		set nocount on;

		select PublisherID, Name, City, Country, Logo from GamesDB.[Publishers]
		order by PublisherID asc
		offset @pageSize * (@pageNumber - 1) rows
		fetch next @pageSize rows only option (recompile)
	end
go