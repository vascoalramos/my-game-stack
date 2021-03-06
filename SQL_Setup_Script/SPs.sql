USE [p5g4]
GO
/****** Object:  StoredProcedure [GamesDB].[promoteToAdmin]    Script Date: 07/06/2019 15:47:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [GamesDB].[promoteToAdmin]
	@userName varchar(30),
	@responseMsg nvarchar(250) output
as
	begin
		set nocount on

		begin try
			insert into GamesDB.[Admin] values (@userName, getdate());
			set @responseMsg = 'Success'
		end try

		begin catch
			set @responseMsg=error_message()
		end catch
	end

GO
/****** Object:  StoredProcedure [GamesDB].[removeDeveloper]    Script Date: 07/06/2019 15:47:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [GamesDB].[removeDeveloper]
	@devID int,
	@responseMsg nvarchar(250) output
as
	begin
		set nocount on
		
		begin try
			delete from GamesDB.[Developers] where DeveloperID = @devID
			set @responseMsg='Success'
		end try
		begin catch
			set @responseMsg=error_message()
		end catch
	end

GO
/****** Object:  StoredProcedure [GamesDB].[removeFranchise]    Script Date: 07/06/2019 15:47:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [GamesDB].[removeFranchise]
	@franID int,
	@responseMsg nvarchar(250) output
as
	begin
		set nocount on
		
		begin try
			delete from GamesDB.[Franchises] where FranchiseID = @franID
			set @responseMsg='Success'
		end try
		begin catch
			set @responseMsg=error_message()
		end catch
	end

GO
/****** Object:  StoredProcedure [GamesDB].[removeGame]    Script Date: 07/06/2019 15:47:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [GamesDB].[removeGame]
	@gameID int,
	@responseMsg nvarchar(250) output
as
	begin
		set nocount on
		
		begin try
			delete from GamesDB.[Games] where PubID = @gameID
			set @responseMsg='Success'
		end try
		begin catch
			set @responseMsg=error_message()
		end catch
	end

GO
/****** Object:  StoredProcedure [GamesDB].[removeGenre]    Script Date: 07/06/2019 15:47:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [GamesDB].[removeGenre]
	@genreID int,
	@responseMsg nvarchar(250) output
as
	begin
		set nocount on
		
		begin try
			delete from GamesDB.[Genres] where GenreID = @genreID
			set @responseMsg='Success'
		end try
		begin catch
			set @responseMsg=error_message()
		end catch
	end

GO
/****** Object:  StoredProcedure [GamesDB].[removePlatform]    Script Date: 07/06/2019 15:47:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [GamesDB].[removePlatform]
	@platID int,
	@responseMsg nvarchar(250) output
as
	begin
		set nocount on
		
		begin try
			delete from GamesDB.[Platforms] where PlatformID = @platID
			set @responseMsg='Success'
		end try
		begin catch
			set @responseMsg=error_message()
		end catch
	end

GO
/****** Object:  StoredProcedure [GamesDB].[removePublisher]    Script Date: 07/06/2019 15:47:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [GamesDB].[removePublisher]
	@pubID int,
	@responseMsg nvarchar(250) output
as
	begin
		set nocount on
		
		begin try
			delete from GamesDB.[Publishers] where PublisherID = @pubID
			set @responseMsg='Success'
		end try
		begin catch
			set @responseMsg=error_message()
		end catch
	end

GO
/****** Object:  StoredProcedure [GamesDB].[removeTournment]    Script Date: 07/06/2019 15:47:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [GamesDB].[removeTournment]
	@tourID int,
	@responseMsg nvarchar(250) output
as
	begin
		set nocount on
		
		begin try
			delete from GamesDB.[Tournments] where TournmentID = @tourID
			set @responseMsg='Success'
		end try
		begin catch
			set @responseMsg=error_message()
		end catch
	end

GO
/****** Object:  StoredProcedure [GamesDB].[removeUser]    Script Date: 07/06/2019 15:47:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [GamesDB].[removeUser]
	@userName varchar(30),
	@responseMsg nvarchar(250) output
as
	begin
		set nocount on
		
		begin try
			delete from GamesDB.[Users] where UserName = @userName
			set @responseMsg='Success'
		end try
		begin catch
			set @responseMsg=error_message()
		end catch
	end

GO
/****** Object:  StoredProcedure [GamesDB].[uspAddDeveloper]    Script Date: 07/06/2019 15:47:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [GamesDB].[uspAddDeveloper]
	@mail VARCHAR(max), 
    @dev_name varchar(max), 
    @phone varchar(max),
    @photo varchar(max),
    @website varchar(max),
	@city varchar(max),
	@country varchar(max),
    @responseMsg nvarchar(250) output
AS
BEGIN
	SET NOCOUNT ON

	BEGIN TRY

		IF @mail = ''
		BEGIN
			SET @mail = NULL
		END

		IF @phone = ''
		BEGIN
			SET @phone = NULL
		END

		IF @photo = ''
		BEGIN
			SET @photo = NULL
		END

		IF @website = ''
		BEGIN
			SET @website = NULL
		END

		IF @city = ''
		BEGIN
			SET @city = NULL
		END

		IF @country = ''
		BEGIN
			SET @country = NULL
		END

		INSERT INTO GamesDB.[Developers] ([Name], Email, Phone, Website, Logo, City, Country)
		VALUES (@dev_name, @mail, @phone, @website, @photo, @city, @country)

		SET @responseMsg='Success'
	END TRY
	BEGIN CATCH
		SET @responseMsg=error_message()
	END CATCH
END

GO
/****** Object:  StoredProcedure [GamesDB].[uspAddFranchise]    Script Date: 07/06/2019 15:47:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [GamesDB].[uspAddFranchise]
	@Name VARCHAR(max), 
    @photo varchar(max),
    @responseMsg nvarchar(250) output
AS
BEGIN
	SET NOCOUNT ON

	BEGIN TRY

		IF @photo = ''
		BEGIN
			SET @photo = NULL
		END

		INSERT INTO GamesDB.[Franchises] ([Name],NoOfGames,Logo)
		VALUES (@Name, 0, @photo)

		SET @responseMsg='Success'
	END TRY
	BEGIN CATCH
		SET @responseMsg=error_message()
	END CATCH
END

GO
/****** Object:  StoredProcedure [GamesDB].[uspAddGame]    Script Date: 07/06/2019 15:47:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [GamesDB].[uspAddGame]
	@game_name VARCHAR(max), 
    @launch_date varchar(max), 
    @publisher varchar(max),
    @photo varchar(max),
	@franchise varchar(max),
	@description varchar(max),

	@developers varchar(max), 
    @genres varchar(max), 
	@platforms varchar(max),

    @responseMsg nvarchar(250) output,
	@addedGameID int output

AS
BEGIN
	SET NOCOUNT ON
	BEGIN TRANSACTION
		BEGIN TRY
			INSERT INTO GamesDB.[Games] (Title, LauchDate, [Description], PubID, CoverImage)
			VALUES (@game_name, @launch_date, @description, @publisher, @photo)
	
			SELECT TOP 1 @addedGameID=GameID FROM GamesDB.Games ORDER BY GameID DESC
	
			-- Franchise
			if @franchise <> 'None'
			begin
				insert into GamesDB.GameBelongsFranchise (GameID, FranchiseID)
				values (@addedGameID, @franchise)
			end

			--Developer
	
			DECLARE @SPLIT_STR TABLE (sub_str VARCHAR(MAX))
	
			INSERT @SPLIT_STR
			SELECT * FROM splitArray (@developers, ';')
	
			DECLARE @id INT, @maxid INT
			SELECT @id = MIN(sub_str), @maxid = MAX(sub_str)
			FROM @SPLIT_STR
	
			WHILE @id <= @maxid
			BEGIN
			   DECLARE @working_str VARCHAR(MAX)
			   SELECT @working_str = sub_str
			   FROM @SPLIT_STR
			   WHERE sub_str = @id
	
					INSERT INTO GamesDB.[GameDeveloper] (GameID,DeveloperID)
					VALUES (@addedGameID,@working_str)
	
			   SELECT @id += 1  
			END
	
			--Genre
			DECLARE @SPLIT_STR2 TABLE (sub_str VARCHAR(MAX))
	
			INSERT @SPLIT_STR2
			SELECT * FROM splitArray (@genres, ';')
	
			DECLARE @id2 INT, @maxid2 INT
			SELECT @id2 = MIN(sub_str), @maxid2 = MAX(sub_str)
			FROM @SPLIT_STR2
	
			WHILE @id2 <= @maxid2
			BEGIN
			   DECLARE @working_str2 VARCHAR(MAX)
			   SELECT @working_str2 = sub_str
			   FROM @SPLIT_STR2
			   WHERE sub_str = @id2
			   print @working_str2
	
					INSERT INTO GamesDB.[GameGenre] (GenreID,GameID)
					VALUES (@working_str2,@addedGameID)
	
			   SELECT @id2 += 1 
			END

			--Platform
			DECLARE @SPLIT_STR3 TABLE (sub_str VARCHAR(MAX))
	
			INSERT @SPLIT_STR3
			SELECT * FROM splitArray (@platforms, ';')
	
			DECLARE @id3 INT, @maxid3 INT
			SELECT @id3 = MIN(sub_str), @maxid3 = MAX(sub_str)
			FROM @SPLIT_STR3
	
			WHILE @id3 <= @maxid3
			BEGIN
			   DECLARE @working_str3 VARCHAR(MAX)
			   SELECT @working_str3= sub_str
			   FROM @SPLIT_STR3
			   WHERE sub_str = @id3
	
			   INSERT INTO GamesDB.[Releases] (GameID,PlatformID)
			   VALUES (@addedGameID,@working_str3)
	
			   SELECT @id3 += 1 
			END

			SET @responseMsg='Success'

		END TRY
		BEGIN CATCH
			SET @responseMsg=error_message()
		END CATCH
	COMMIT TRANSACTION
END

GO
/****** Object:  StoredProcedure [GamesDB].[uspAddGameToList]    Script Date: 07/06/2019 15:47:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [GamesDB].[uspAddGameToList]
	@gameID int, 
    @userName varchar(max), 
    @listName varchar(max),
	@responseMsg nvarchar(250) output

as
	begin
		begin transaction
			begin try
				if 0 = ( select GamesDB.checkGameInUser (@userName, @gameID) )
					begin
						insert into GamesDB.GameEventList (GameID, EventID, UserName, RegDate, ChangeDate)
						values (@gameID, (select EventID from GamesDB.[Events] join GamesDB.EventType on GamesDB.[Events].TypeID=GamesDB.EventType.TypeID where Name=@listName and UserName=@userName), @userName, getdate(), null)
					end
	
				else
					begin
						declare @eventID as int;
						set @eventID = (select EventID from GameEventList where GameID=@gameID and UserName=@userName and ChangeDate is null)
				
						if exists (	select *
									from GamesDB.Games join GamesDB.GameEventList on GamesDB.Games.GameID=GamesDB.GameEventList.GameID
										join GamesDB.[Events] on GamesDB.GameEventList.EventID=GamesDB.[Events].EventID
									where GamesDB.[Events].UserName=@userName and GamesDB.Games.GameID=@gameID and GamesDB.[Events].TypeID = (select TypeID from GamesDB.EventType where Name=@listName)
									)
							begin
								update GamesDB.GameEventList
								set ChangeDate = null
								where UserName = @userName and GameID = @gameID and EventID = (select EventID from GamesDB.[Events] join GamesDB.EventType on GamesDB.[Events].TypeID=GamesDB.EventType.TypeID where Name=@listName and UserName=@userName)
	
								update GamesDB.GameEventList
								set ChangeDate = getdate()
								where UserName = @userName and GameID = @gameID and EventID = @eventID
							end

						else
							begin
								insert into GamesDB.GameEventList (GameID, EventID, UserName, RegDate, ChangeDate)
								values (@gameID, (select EventID from GamesDB.[Events] join GamesDB.EventType on GamesDB.[Events].TypeID=GamesDB.EventType.TypeID where Name=@listName and UserName=@userName), @userName, getdate(), null)
									
								update GamesDB.GameEventList
								set ChangeDate = getdate()
								where UserName = @userName and GameID = @gameID and EventID = @eventID
							end
					end
				SET @responseMsg='Success'
			end try
			begin catch
				SET @responseMsg='Error adding game to List'
			end catch
		commit transaction
	end

GO
/****** Object:  StoredProcedure [GamesDB].[uspAddGenre]    Script Date: 07/06/2019 15:47:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [GamesDB].[uspAddGenre]
    @Name varchar(max),
    @responseMsg nvarchar(250) output
AS
BEGIN
	SET NOCOUNT ON

	BEGIN TRY

		INSERT INTO GamesDB.[Genres] ([Name])
		VALUES (@Name)

		SET @responseMsg='Success'
	END TRY
	BEGIN CATCH
		SET @responseMsg=error_message()
	END CATCH
END

GO
/****** Object:  StoredProcedure [GamesDB].[uspAddPlatform]    Script Date: 07/06/2019 15:47:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [GamesDB].[uspAddPlatform]
	@Name VARCHAR(max), 
    @Owner varchar(max), 
    @ReleaseDate varchar(max),
    @responseMsg nvarchar(250) output
AS
BEGIN
	SET NOCOUNT ON

	BEGIN TRY
		IF @ReleaseDate = ''
		BEGIN
			SET @ReleaseDate = NULL
		END

		IF @Owner = ''
		BEGIN
			SET @Owner = NULL
		END

		INSERT INTO GamesDB.[Platforms] ([Name],Owner,ReleaseDate)
		VALUES (@Name, @Owner, @ReleaseDate)

		SET @responseMsg='Success'
	END TRY
	BEGIN CATCH
		SET @responseMsg=error_message()
	END CATCH
END

GO
/****** Object:  StoredProcedure [GamesDB].[uspAddPublisher]    Script Date: 07/06/2019 15:47:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [GamesDB].[uspAddPublisher]
	@mail VARCHAR(max), 
    @pub_name varchar(max), 
    @phone varchar(max),
    @photo varchar(max),
    @website varchar(max),
	@city varchar(max),
	@country varchar(max),
    @responseMsg nvarchar(250) output
AS
BEGIN
	SET NOCOUNT ON

	BEGIN TRY

		IF @mail = ''
		BEGIN
			SET @mail = NULL
		END

		IF @phone = ''
		BEGIN
			SET @phone = NULL
		END

		IF @photo = ''
		BEGIN
			SET @photo = NULL
		END

		IF @city = ''
		BEGIN
			SET @city = NULL
		END

		IF @country = ''
		BEGIN
			SET @country = NULL
		END

		INSERT INTO GamesDB.[Publishers] ([Name], Email, Phone, Website, Logo, City, Country)
		VALUES (@pub_name, @mail, @phone, @website, @photo, @city, @country)

		SET @responseMsg='Success'
	END TRY
	BEGIN CATCH
		SET @responseMsg=error_message()
	END CATCH
END

GO
/****** Object:  StoredProcedure [GamesDB].[uspAddReview]    Script Date: 07/06/2019 15:47:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [GamesDB].[uspAddReview]
	@score VARCHAR(max), 
    @title varchar(max), 
    @description varchar(max),
	@userName varchar(max),
	@gameID int

as
	begin
		insert into GamesDB.Reviews (Score, Title, [Description], UserName, GameID)
		values (@score, @title, @description, @userName, @gameID)
	end

GO
/****** Object:  StoredProcedure [GamesDB].[uspAddTournment]    Script Date: 07/06/2019 15:47:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [GamesDB].[uspAddTournment]
	@tourn_name VARCHAR(max), 
    @prize varchar(max), 
    @location varchar(max),
    @start_date varchar(max),
    @end_date varchar(max),
	@game_id varchar(max),
    @responseMsg nvarchar(250) output
AS
BEGIN
	SET NOCOUNT ON

	BEGIN TRY
		IF @prize = ''
		BEGIN
			SET @prize = 0
		END

		IF @location = ''
		BEGIN
			SET @location = NULL
		END

		IF @start_date = ''
		BEGIN
			SET @start_date = NULL
		END
			
		IF @end_date = ''
		BEGIN
			SET @end_date = NULL
		END

		INSERT INTO GamesDB.[Tournments] (GameID,[Name],StartDate,EndDate,[Location],PrizePool)
		VALUES (@game_id, @tourn_name, @start_date, @end_date, @location, @prize)

		SET @responseMsg='Success'
	END TRY
	BEGIN CATCH
		SET @responseMsg=error_message()
	END CATCH
END

GO
/****** Object:  StoredProcedure [GamesDB].[uspAddUser]    Script Date: 07/06/2019 15:47:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [GamesDB].[uspAddUser]
	@mail VARCHAR(max), 
    @password varchar(max), 
    @fname varchar(max),
    @lname varchar(max),
    @UserName varchar(max),
	@photo varchar(max),
    @responseMsg nvarchar(250) output
as
begin
	set nocount on

	declare @salt uniqueidentifier=newid()
	begin try

		insert into GamesDB.[Users] (UserName, Email, Fname, Lname, Photo, Password_hash, Salt)
		values (@UserName, @mail, @fname, @lname, @photo, hashbytes('SHA2_512', @password + cast(@salt as nvarchar(36))), @salt)

		set @responseMsg='Success'
	end try
	begin catch
		set @responseMsg=error_message()
	end catch
end

GO
/****** Object:  StoredProcedure [GamesDB].[uspFilterDevelopers]    Script Date: 07/06/2019 15:47:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [GamesDB].[uspFilterDevelopers]
	@pageSize int,
	@pageNumber int,
	@name varchar(65)
as
	begin
		declare @tempTable table (
			DeveloperID int,
			Name varchar(max) not null,
			City varchar(max),
			Country varchar(max),
			Logo varchar(max)
		)
		set nocount on;

		if @name <> 'None'
		begin
			insert into @tempTable (DeveloperID, Name, City, Country, Logo)
			select DeveloperID, Name, City, Country, Logo from GamesDB.Developers where Name like '%'+@name+'%'
		end

		else
		begin
			insert into @tempTable select DeveloperID, Name, City, Country, Logo from GamesDB.Developers
		end
		
		select tt.* from @tempTable tt
		order by DeveloperID
		offset @pageSize * (@pageNumber - 1) rows
		fetch next @pageSize rows only option (recompile)
	end

GO
/****** Object:  StoredProcedure [GamesDB].[uspFilterFranchises]    Script Date: 07/06/2019 15:47:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [GamesDB].[uspFilterFranchises]
	@pageSize int,
	@pageNumber int,
	@opt varchar(30),
	@name varchar(65)
as
	begin
		declare @tempTable table (
			FranchiseID int,
			Name varchar(max) not null,
			NoOfGames int,
			Logo varchar(max)
		)
		set nocount on;

		if @name <> 'None'
		begin
			insert into @tempTable (FranchiseID, Name, NoOfGames, Logo)
			select * from GamesDB.Franchises where Name like '%'+@name+'%'
		end

		else
		begin
			insert into @tempTable select * from GamesDB.Franchises
		end

		if @opt = 'NofGamesDesc'
		begin
			select tt.* from @tempTable tt
			where tt.NoOfGames is not null
			order by tt.NoOfGames desc
			offset @pageSize * (@pageNumber - 1) rows
			fetch next @pageSize rows only option (recompile)
		end

		if @opt = 'NofGamesAsc'
		begin
			select tt.* from @tempTable tt
			where tt.NoOfGames is not null
			order by tt.NoOfGames asc
			offset @pageSize * (@pageNumber - 1) rows
			fetch next @pageSize rows only option (recompile)
		end
		
		if @opt = 'None'
		begin
			select tt.* from @tempTable tt
			order by FranchiseID
			offset @pageSize * (@pageNumber - 1) rows
			fetch next @pageSize rows only option (recompile)
		end
	end

GO
/****** Object:  StoredProcedure [GamesDB].[uspFilterGames]    Script Date: 07/06/2019 15:47:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [GamesDB].[uspFilterGames]
	@pageSize int,
	@pageNumber int,
	@opt varchar(30),
	@name varchar(65),
	@genreID varchar(30),
	@franID varchar(30),
	@pubID varchar(30)
as
	begin
		declare @tempTable table (
			GameID int,
			Title varchar(max) not null,
			LaunchDate date,
			[Description] varchar(max),
			PubID int,
			CoverImage varchar(max)
		)
		set nocount on;

		if @name <> 'None'
		begin
			insert into @tempTable (GameID, Title, LaunchDate, [Description], PubID, CoverImage)
			select * from GamesDB.Games where Title like '%'+@name+'%'
		end

		else
		begin
			insert into @tempTable select * from GamesDB.Games
		end

		if @genreID <> 'None'
		begin
			insert into @tempTable
			select distinct tt.*
			from @tempTable tt join GamesDB.GameGenre on tt.GameID=GamesDB.GameGenre.GameID
			where GenreID = @genreID and not exists (select * from GamesDB.Games where GameID=GamesDB.GameGenre.GameID)

			delete from @tempTable
			where GameID not in (	select GamesDB.Games.GameID
								from GamesDB.Games join GamesDB.GameGenre on GamesDB.Games.GameID=GamesDB.GameGenre.GameID
								where GenreID = @genreID)
 		end

		if @franID <> 'None'
		begin
			insert into @tempTable
			select tt.*
			from @tempTable tt join GamesDB.GameBelongsFranchise on tt.GameID=GamesDB.GameBelongsFranchise.GameID
			where FranchiseID = @franID and not exists (select * from GamesDB.Games where GameID=GamesDB.GameBelongsFranchise.GameID)

			delete from @tempTable
			where GameID not in (	select GamesDB.Games.GameID
								from GamesDB.Games join GamesDB.GameBelongsFranchise on GamesDB.Games.GameID=GamesDB.GameBelongsFranchise.GameID
								where FranchiseID = @franID)
 		end

		if @pubID <> 'None'
		begin
			insert into @tempTable
			select tt.*
			from @tempTable tt
			where PubID = @pubID and not exists (select * from GamesDB.Games)

			delete from @tempTable
			where GameID not in (	select GamesDB.Games.GameID
								from GamesDB.Games
								where PubID = @pubID)
 		end

		if @opt = 'LaunchDateDesc'
		begin
			select tt.* from @tempTable tt
			where tt.LaunchDate is not null
			order by tt.LaunchDate desc
			offset @pageSize * (@pageNumber - 1) rows
			fetch next @pageSize rows only option (recompile)
		end

		if @opt = 'LaunchDateAsc'
		begin
			select tt.* from @tempTable tt
			where tt.LaunchDate is not null
			order by tt.LaunchDate asc
			offset @pageSize * (@pageNumber - 1) rows
			fetch next @pageSize rows only option (recompile)
		end
		
		if @opt = 'None'
		begin
			select tt.* from @tempTable tt
			order by GameID
			offset @pageSize * (@pageNumber - 1) rows
			fetch next @pageSize rows only option (recompile)
		end
	end

GO
/****** Object:  StoredProcedure [GamesDB].[uspFilterPublishers]    Script Date: 07/06/2019 15:47:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [GamesDB].[uspFilterPublishers]
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

GO
/****** Object:  StoredProcedure [GamesDB].[uspFilterTournments]    Script Date: 07/06/2019 15:47:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [GamesDB].[uspFilterTournments]
	@pageSize int,
	@pageNumber int,
	@opt varchar(65),
	@name varchar(65),
	@title varchar(65)
as
	begin
		declare @tempTable table (
			TournmentID int,
			Name varchar(max) not null,
			PrizePool int,
			Location varchar(max),
			GameID int,
			Title varchar(max)
		)
		set nocount on;

		if @name <> 'None'
		begin
			insert into @tempTable (TournmentID, Name, PrizePool, Location, GameID, Title)
			select TournmentID, Name, PrizePool, Location, GamesDB.[Tournments].GameID, Title
			from GamesDB.[Tournments] join GamesDB.[Games] on GamesDB.[Tournments].GameID=GamesDB.[Games].GameID
			where Name like '%'+@name+'%'
		end

		else
		begin
			insert into @tempTable (TournmentID, Name, PrizePool, Location, GameID, Title)
			select TournmentID, Name, PrizePool, Location, GamesDB.[Tournments].GameID, Title
			from GamesDB.[Tournments] join GamesDB.[Games] on GamesDB.[Tournments].GameID=GamesDB.[Games].GameID
		end

		if @title <> 'None'
		begin
			insert into @tempTable (TournmentID, Name, PrizePool, Location, GameID, Title)
			select distinct TournmentID, Name, PrizePool, Location, tt.GameID, GamesDB.[Games].Title
			from @tempTable tt join GamesDB.[Games] on tt.GameID=GamesDB.[Games].GameID
			where GamesDB.[Games].Title like '%'+@title+'%' and not exists (select tt.* from @tempTable tt)

			delete from @tempTable
			where TournmentID not in (	select GamesDB.Tournments.TournmentID
										from GamesDB.[Tournments] join GamesDB.[Games] on GamesDB.[Tournments].GameID=GamesDB.[Games].GameID
										where Title like '%'+@title+'%')
		end

		if @opt = 'PrizePoolAsc'
		begin
			select tt.* from @tempTable tt
			where tt.PrizePool is not null
			order by tt.PrizePool asc
			offset @pageSize * (@pageNumber - 1) rows
			fetch next @pageSize rows only option (recompile)
		end

		if @opt = 'PrizePoolDesc'
		begin
			select tt.* from @tempTable tt
			where tt.PrizePool is not null
			order by tt.PrizePool desc
			offset @pageSize * (@pageNumber - 1) rows
			fetch next @pageSize rows only option (recompile)
		end
		
		if @opt = 'None'
		begin
			select tt.* from @tempTable tt
			order by tt.TournmentID
			offset @pageSize * (@pageNumber - 1) rows
			fetch next @pageSize rows only option (recompile)
		end
	end

GO
/****** Object:  StoredProcedure [GamesDB].[uspLogin]    Script Date: 07/06/2019 15:47:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [GamesDB].[uspLogin]
    @loginName varchar(max),
    @password varchar(max),
    @responseMessage varchar(250)='' output
as
begin
    set nocount on

	declare @userName varchar(max)

    if exists (select top 1 UserName from GamesDB.[Users] where UserName=@loginName)
    begin
		set @userName=(select UserName from GamesDB.[Users] where UserName=@loginName and Password_hash=hashbytes('SHA2_512', @password+cast(Salt as nvarchar(36))))
		
		if(@userName is null)
			set @responseMessage='Incorrect password'
		else
			set @responseMessage='User successfully logged in'
    end
    else
		set @responseMessage='Invalid login'

end

GO
/****** Object:  StoredProcedure [GamesDB].[uspSearchDevelopers]    Script Date: 07/06/2019 15:47:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [GamesDB].[uspSearchDevelopers]
	@pageSize int,
	@pageNumber int
as
	begin
		set nocount on;

		select DeveloperID, Name, City, Country, Logo from GamesDB.[Developers]
		order by DeveloperID asc
		offset @pageSize * (@pageNumber - 1) rows
		fetch next @pageSize rows only option (recompile)
	end

GO
/****** Object:  StoredProcedure [GamesDB].[uspSearchFranchises]    Script Date: 07/06/2019 15:47:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [GamesDB].[uspSearchFranchises]
	@pageSize int,
	@pageNumber int
as
	begin
		set nocount on;

		select * from GamesDB.[Franchises]
		order by FranchiseID asc
		offset @pageSize * (@pageNumber - 1) rows
		fetch next @pageSize rows only option (recompile)
	end

GO
/****** Object:  StoredProcedure [GamesDB].[uspSearchGames]    Script Date: 07/06/2019 15:47:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [GamesDB].[uspSearchGames]
	@pageSize int,
	@pageNumber int
as
	begin
		set nocount on;

		select * from GamesDB.[Games]
		order by GameID asc
		offset @pageSize * (@pageNumber - 1) rows
		fetch next @pageSize rows only option (recompile)
	end

GO
/****** Object:  StoredProcedure [GamesDB].[uspSearchGamesInList]    Script Date: 07/06/2019 15:47:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [GamesDB].[uspSearchGamesInList]
	@listName varchar(100),
	@userName varchar(max)
as
	begin
		set nocount on;
		select *
		from GamesDB.Games join GamesDB.GameEventList on GamesDB.Games.GameID=GamesDB.GameEventList.GameID
			join GamesDB.[Events] on GamesDB.GameEventList.EventID=GamesDB.[Events].EventID
		where GamesDB.[Events].UserName=@userName and ChangeDate is null and  GamesDB.[Events].TypeID = (select TypeID from GamesDB.EventType where Name=@listName)
		order by GamesDB.Games.GameID asc
	end

GO
/****** Object:  StoredProcedure [GamesDB].[uspSearchPublishers]    Script Date: 07/06/2019 15:47:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [GamesDB].[uspSearchPublishers]
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

GO
/****** Object:  StoredProcedure [GamesDB].[uspSearchTournments]    Script Date: 07/06/2019 15:47:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [GamesDB].[uspSearchTournments]
	@pageSize int,
	@pageNumber int
as
	begin
		set nocount on;

		select TournmentID, Name, PrizePool, Location, GamesDB.[Tournments].GameID, Title
		from GamesDB.[Tournments] join GamesDB.[Games] on GamesDB.[Tournments].GameID=GamesDB.[Games].GameID
		order by TournmentID asc
		offset @pageSize * (@pageNumber - 1) rows
		fetch next @pageSize rows only option (recompile)
	end

GO
/****** Object:  StoredProcedure [GamesDB].[uspUpdateUser]    Script Date: 07/06/2019 15:47:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [GamesDB].[uspUpdateUser]
	@UserName varchar(max),
	@mail VARCHAR(max), 
    @password varchar(max), 
    @fname varchar(max),
    @lname varchar(max),
	@photo varchar(max),
    @responseMsg nvarchar(250) output
as
begin
	set nocount on

	declare @salt uniqueidentifier=newid()

	begin try
		if @fname <> 'None'
		begin
			update GamesDB.Users
			set Fname = @fname
			where UserName = @UserName
		end

		if @lname <> 'None'
		begin
			update GamesDB.Users
			set Lname = @lname
			where UserName = @UserName
		end

		if @mail <> 'None'
		begin
			update GamesDB.Users
			set Email = @mail
			where UserName = @UserName
		end

		if @password <> 'None'
		begin
			update GamesDB.Users
			set Password_hash = hashbytes('SHA2_512', @password + cast(@salt as nvarchar(36))), Salt = @salt
			where UserName = @UserName
		end

		if @photo <> 'None'
		begin
			update GamesDB.Users
			set Photo = @photo
			where UserName = @UserName
		end

		set @responseMsg='Success'
	end try
	begin catch
		set @responseMsg=error_message()
	end catch
end

GO
