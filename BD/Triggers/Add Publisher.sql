drop trigger GamesDB.triggerAddPublisher;
go

go
create trigger GamesDB.triggerAddPublisher on GamesDB.[Publishers]
instead of insert
as
	begin
		set nocount on;

		declare @pubName as varchar(100);
		declare @email as varchar(100);
		declare @phone as varchar(100);
		declare @site as varchar(100);
		declare @image as varchar(max);
		declare @city as varchar(100);
		declare @country as varchar(100);
		select @pubName = Name, @email= Email, @phone = Phone, @site = Website, @image = Logo, @city = City, @country = Country from inserted;
		if exists(select * from GamesDB.[Publishers] where Name=@pubName )
		begin
			raiserror ('Publisher already exists', 16, 1); 
		end
		else
		begin
			insert into GamesDB.[Publishers] (Name, Email, Phone, Website, Logo, City, Country)
			values (@pubName, @email, @phone, @site, @image, @city, @country)
		end
	end
go