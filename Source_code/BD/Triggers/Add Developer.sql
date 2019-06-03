drop trigger GamesDB.triggerAddDeveloper;
go

go
create trigger GamesDB.triggerAddDeveloper on GamesDB.[Developers]
instead of insert
as
	begin
		set nocount on;

		declare @devName as varchar(100);
		declare @email as varchar(100);
		declare @phone as varchar(100);
		declare @site as varchar(100);
		declare @image as varchar(max);
		declare @city as varchar(100);
		declare @country as varchar(100);
		select @devName = Name, @email= Email, @phone = Phone, @site = Website, @image = Logo, @city = City, @country = Country from inserted;
		if exists(select * from GamesDB.[Developers] where Name=@devName )
		begin
			raiserror ('Developer already exists', 16, 1); 
		end
		else
		begin
			insert into GamesDB.[Developers] (Name, Email, Phone, Website, Logo, City, Country)
			values (@devName, @email, @phone, @site, @image, @city, @country)
		end
	end
go