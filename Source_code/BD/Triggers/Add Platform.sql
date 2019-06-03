drop trigger GamesDB.triggerAddPlatform;
go

go
create trigger GamesDB.triggerAddPlatform on GamesDB.[Platforms]
instead of insert
as
	begin
		set nocount on;

		declare @platName as varchar(100);
		declare @owner as varchar(100);
		declare @date as date;
		select @platName = Name, @owner= [Owner], @date = ReleaseDate from inserted;
		if exists(select * from GamesDB.[Platforms] where Name=@platName )
		begin
			raiserror ('Platform already exists', 16, 1); 
		end
		else
		begin
			insert into GamesDB.[Platforms] (Name, [Owner], ReleaseDate)
			values (@platName, @owner, @date)
		end
	end
go