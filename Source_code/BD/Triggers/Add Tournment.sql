drop trigger GamesDB.triggerAddTournment;
go

go
create trigger GamesDB.triggerAddTournment on GamesDB.[Tournments]
instead of insert
as
	begin
		set nocount on;

		declare @tourName as varchar(100);
		declare @prizepool as int;
		declare @location as varchar(max);
		declare @start as date;
		declare @end as date;
		declare @gameid as int;

		select @gameid = GameID, @tourName = Name, @prizepool = PrizePool, @location = Location, @start = StartDate, @end = EndDate from inserted;
		if exists(select * from GamesDB.[Tournments] where Name=@tourName )
		begin
			raiserror ('Tournment already exists', 16, 1); 
		end
		else
		begin
			insert into GamesDB.[Tournments] (Name, PrizePool, Location, StartDate, EndDate, GameID)
			values (@tourName, @prizepool, @location, @start, @end, @gameid)
		end
	end
go