drop trigger GamesDB.triggerAddFranchise;
go

go
create trigger GamesDB.triggerAddFranchise on GamesDB.[Franchises]
instead of insert
as
	begin
		set nocount on;

		declare @franName as varchar(100);
		declare @nOfGames as int;
		declare @logo as varchar(max);

		select @franName = Name, @nOfGames = NoOfGames, @logo = Logo from inserted;
		if exists(select * from GamesDB.[Franchises] where Name=@franName )
		begin
			raiserror ('Franchise already exists', 16, 1); 
		end
		else
		begin
			insert into GamesDB.[Franchises] (Name, NoOfGames, Logo)
			values (@franName, @nOfGames, @logo)
		end
	end