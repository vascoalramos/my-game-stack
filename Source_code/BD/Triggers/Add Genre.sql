drop trigger GamesDB.triggerAddGenre;
go

go
create trigger GamesDB.triggerAddGenre on GamesDB.[Genres]
instead of insert
as
	begin
		set nocount on;

		declare @genreName as varchar(100);
		select @genreName = Name from inserted;
		if exists(select * from GamesDB.[Genres] where Name=@genreName )
		begin
			raiserror ('Genre already exists', 16, 1); 
		end
		else
		begin
			insert into GamesDB.[Genres] (Name)
			values (@genreName)
		end
	end
go