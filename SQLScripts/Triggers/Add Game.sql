drop trigger GamesDB.triggerAddGame;
go

go
create trigger GamesDB.triggerAddGame on GamesDB.[Games]
instead of insert
as
	begin
		set nocount on;

		declare @gameName as varchar(100);
		declare @date as date;
		declare @logo as varchar(max);
		declare @description as varchar(max);
		declare @pubID as int;

		select @gameName = Title, @date = LauchDate, @logo = CoverImage, @description = [Description], @pubID = PubID from inserted;
		if exists(select * from GamesDB.[Games] where Title=@gameName )
		begin
			raiserror ('Game already exists', 16, 1); 
		end
		else
		begin
			insert into GamesDB.[Games] (Title, LauchDate, CoverImage, [Description], PubID)
			values ( @gameName, @date, @logo, @description, @pubID)
		end
	end