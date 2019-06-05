drop procedure GamesDB.uspFilterPublishers;
go

go
create procedure [GamesDB].uspFilterPublishers
	@pageSize int,
	@pageNumber int,
	@name varchar(65)
as
	begin
		declare @tempTable table (
			PublisherID int,
			Name varchar(max) not null,
			City varchar(max),
			Country varchar(max),
			Logo varchar(max)
		)
		set nocount on;

		if @name <> 'None'
		begin
			insert into @tempTable (PublisherID, Name, City, Country, Logo)
			select PublisherID, Name, City, Country, Logo from GamesDB.Publishers where Name like '%'+@name+'%'
		end

		else
		begin
			insert into @tempTable select PublisherID, Name, City, Country, Logo from GamesDB.Publishers
		end
		
		select tt.* from @tempTable tt
		order by PublisherID
		offset @pageSize * (@pageNumber - 1) rows
		fetch next @pageSize rows only option (recompile)
	end
go