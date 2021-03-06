USE [p5g4]
GO

/****** Object:  Index [ixdxFranchise]    Script Date: 07/06/2019 15:42:43 ******/
CREATE NONCLUSTERED INDEX [ixdxFranchise] ON [GamesDB].[GameBelongsFranchise]
(
	[GameID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [idxtournment]    Script Date: 07/06/2019 15:42:43 ******/
CREATE NONCLUSTERED INDEX [idxtournment] ON [GamesDB].[Tournments]
(
	[GameID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Trigger [GamesDB].[triggerAddDeveloper]    Script Date: 07/06/2019 15:42:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create trigger [GamesDB].[triggerAddDeveloper] on [GamesDB].[Developers]
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

GO
/****** Object:  Trigger [GamesDB].[triggerAddFranchise]    Script Date: 07/06/2019 15:42:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create trigger [GamesDB].[triggerAddFranchise] on [GamesDB].[Franchises]
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
GO
/****** Object:  Trigger [GamesDB].[OnAddToFranchise]    Script Date: 07/06/2019 15:42:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create trigger [GamesDB].[OnAddToFranchise] on [GamesDB].[GameBelongsFranchise]
after insert
as
	begin
		declare @franchiseID int;

		SELECT @franchiseID=FranchiseID FROM inserted;

		UPDATE GamesDB.Franchises
		SET NoOfGames = (SELECT distinct NoOfGames
						FROM GamesDB.Franchises JOIN GamesDB.GameBelongsFranchise ON GamesDB.Franchises.FranchiseID = GamesDB.GameBelongsFranchise.FranchiseID
						WHERE GamesDB.Franchises.FranchiseID = @franchiseID
						) + 1
		WHERE FranchiseID = @franchiseID
	end

GO
/****** Object:  Trigger [GamesDB].[triggerAddGame]    Script Date: 07/06/2019 15:42:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create trigger [GamesDB].[triggerAddGame] on [GamesDB].[Games]
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
GO
/****** Object:  Trigger [GamesDB].[triggerAddGenre]    Script Date: 07/06/2019 15:42:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create trigger [GamesDB].[triggerAddGenre] on [GamesDB].[Genres]
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

GO
/****** Object:  Trigger [GamesDB].[triggerAddPlatform]    Script Date: 07/06/2019 15:42:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create trigger [GamesDB].[triggerAddPlatform] on [GamesDB].[Platforms]
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

GO
/****** Object:  Trigger [GamesDB].[triggerAddPublisher]    Script Date: 07/06/2019 15:42:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create trigger [GamesDB].[triggerAddPublisher] on [GamesDB].[Publishers]
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

GO
/****** Object:  Trigger [GamesDB].[triggerAddReview]    Script Date: 07/06/2019 15:42:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create trigger [GamesDB].[triggerAddReview] on [GamesDB].[Reviews]
instead of insert
as
	begin
		begin try
			declare @userName as varchar(100);
			declare @gameID as int;
			declare @score as int;
			declare @title as varchar(max);
			declare @description as varchar(max);
			select @userName = UserName, @gameID = GameID, @score = Score, @title = Title, @description = [Description] from inserted;

			if not exists(select * from GamesDB.[Reviews] where UserName=@userName and GameID=@gameID )
			begin
				insert into GamesDB.[Reviews] (Score, Title, [Description], [Date], UserName, GameID)
				values (@score, @title, @description, getdate(), @userName, @gameID)
			end
			else
			begin
				update GamesDB.[Reviews]
				set Score = @score, Title=@title, [Description] = @description, [Date] = getdate(), UserName = @userName, GameID = @gameID 
				where UserName=@userName and GameID=@gameID
			end
		end try
		begin catch
			declare @ErrorMessage nvarchar(max), @ErrorSeverity int, @ErrorState int;
			select @ErrorMessage = ERROR_MESSAGE() + ' Line ' + cast(ERROR_LINE() as nvarchar(5)), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE();
			raiserror (@ErrorMessage, @ErrorSeverity, @ErrorState); 
		end catch
	end

GO
/****** Object:  Trigger [GamesDB].[triggerAddTournment]    Script Date: 07/06/2019 15:42:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create trigger [GamesDB].[triggerAddTournment] on [GamesDB].[Tournments]
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

GO
/****** Object:  Trigger [GamesDB].[triggerAddUser]    Script Date: 07/06/2019 15:42:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create trigger [GamesDB].[triggerAddUser] on [GamesDB].[Users]
after insert
as
	begin
		begin transaction
			declare @userName as varchar(30);
			select @userName = UserName from inserted;

			begin try
				insert into GamesDB.[Events] values (@userName,1);
				insert into GamesDB.[Events] values (@userName,2);
				insert into GamesDB.[Events] values (@userName,3);
				insert into GamesDB.[Events] values (@userName,4);
				insert into GamesDB.[Events] values (@userName,5);
			end try

			begin catch
				declare @ErrorMessage nvarchar(max), @ErrorSeverity int, @ErrorState int;
				select @ErrorMessage = ERROR_MESSAGE() + ' Line ' + cast(ERROR_LINE() as nvarchar(5)), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE();
			    raiserror (@ErrorMessage, @ErrorSeverity, @ErrorState); 
			end catch
		commit transaction
	end

GO
