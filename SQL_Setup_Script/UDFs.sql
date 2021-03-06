USE [p5g4]
GO
/****** Object:  UserDefinedFunction [GamesDB].[checkAdmin]    Script Date: 07/06/2019 15:48:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create function [GamesDB].[checkAdmin] (@userName varchar(30)) returns int
as
	begin
		if (not exists( select *
					from GamesDB.[Admin]
					where UserName=@userName
					)
			)
			return 0
		return 1
	end

GO
/****** Object:  UserDefinedFunction [GamesDB].[checkGameInList]    Script Date: 07/06/2019 15:48:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create function [GamesDB].[checkGameInList] (@userName varchar(100), @gameID int, @listName varchar(max)) returns int
as
	begin
		if (not exists( select *
						from GamesDB.Games join GamesDB.GameEventList on GamesDB.Games.GameID=GamesDB.GameEventList.GameID
							join GamesDB.[Events] on GamesDB.GameEventList.EventID=GamesDB.[Events].EventID
						where GamesDB.[Events].UserName=@userName and GamesDB.Games.GameID=@gameID and GamesDB.[Events].TypeID = (select TypeID from GamesDB.EventType where Name=@listName and ChangeDate is null)
					)
			)
			return 0
		return 1
	end

GO
/****** Object:  UserDefinedFunction [GamesDB].[checkGameInUser]    Script Date: 07/06/2019 15:48:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create function [GamesDB].[checkGameInUser] (@userName varchar(30), @gameID int) returns int
as
	begin
		if (not exists( select *
					from GamesDB.[GameEventList]
					where UserName=@userName and GameID=@gameID
					)
			)
			return 0
		return 1
	end

GO
/****** Object:  UserDefinedFunction [GamesDB].[gameAvgScore]    Script Date: 07/06/2019 15:48:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create function [GamesDB].[gameAvgScore] (@gameID int) returns float
as
	begin
		declare @avg_score float
		select @avg_score=round(avg(cast(Score as float)),1)
		from GamesDB.Reviews
		where GameID=@gameID
		return @avg_score
	end

GO
/****** Object:  UserDefinedFunction [GamesDB].[gameFranchise]    Script Date: 07/06/2019 15:48:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create function [GamesDB].[gameFranchise] (@gameID int) returns varchar(max)
as
	begin
		declare @fran varchar(max)
		select @fran=Name
		from GamesDB.GameBelongsFranchise join GamesDB.Franchises on GamesDB.GameBelongsFranchise.FranchiseID=GamesDB.Franchises.FranchiseID
		where GameID=@gameID
		return @fran
	end

GO
/****** Object:  UserDefinedFunction [GamesDB].[splitArray]    Script Date: 07/06/2019 15:48:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE FUNCTION [GamesDB].[splitArray] --This function creates a table with the split values of an array to be used on the stored procedure
    (
	  @argSourceStr varchar(MAX) = NULL, 
      @argDelimiter char(1) = ';'
	)
	RETURNS 
		@SPLIT_STR TABLE (subStr VARCHAR(MAX))
AS

BEGIN
	DECLARE @currentStr varchar(MAX)
	DECLARE @subStr varchar(MAX)
	
	SET @currentStr = @argSourceStr
	 
	WHILE Datalength(@currentStr) > 0
	BEGIN
		IF CHARINDEX(@argDelimiter, @currentStr,1) > 0 
			BEGIN
	           	SET @subStr = SUBSTRING (@currentStr, 1, CHARINDEX(@argDelimiter, @currentStr,1) - 1)
	            SET @currentStr = SUBSTRING (@currentStr, CHARINDEX(@argDelimiter, @currentStr,1) + 1, (Datalength(@currentStr) - CHARINDEX(@argDelimiter, @currentStr,1) + 1))
				INSERT @SPLIT_STR (subStr) VALUES (@subStr)
	   		END
		 ELSE
		 	BEGIN                
				INSERT @SPLIT_STR (subStr) VALUES (@currentStr)	 		
	           	BREAK;
	       	 END 
	END
	RETURN
END



GO
/****** Object:  UserDefinedFunction [GamesDB].[gameDevelopers]    Script Date: 07/06/2019 15:48:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create function [GamesDB].[gameDevelopers] (@gameID int) returns Table
as
	return (select Name
			from GamesDB.GameDeveloper join GamesDB.Developers on GamesDB.GameDeveloper.DeveloperID=GamesDB.Developers.DeveloperID
			where GamesDB.GameDeveloper.GameID=@gameID
			)

GO
/****** Object:  UserDefinedFunction [GamesDB].[gameGenres]    Script Date: 07/06/2019 15:48:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create function [GamesDB].[gameGenres] (@gameID int) returns Table
as
	return (select Name
			from GamesDB.GameGenre join GamesDB.Genres on GamesDB.GameGenre.GenreID=GamesDB.Genres.GenreID
			where GamesDB.GameGenre.GameID=@gameID
			)

GO
/****** Object:  UserDefinedFunction [GamesDB].[gameInfo]    Script Date: 07/06/2019 15:48:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create function [GamesDB].[gameInfo] (@gameID int) returns Table
as
	return (select Title, LauchDate, [Description], Name as PubName 
			from GamesDB.[Games] join GamesDB.Publishers on GamesDB.[Games].PubID=GamesDB.Publishers.PublisherID
			where GamesDB.[Games].GameID=@gameID
			)

GO
/****** Object:  UserDefinedFunction [GamesDB].[gameReleases]    Script Date: 07/06/2019 15:48:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create function [GamesDB].[gameReleases] (@gameID int) returns Table
as
	return (select Name
			from GamesDB.Releases join GamesDB.Platforms on GamesDB.Releases.PlatformID=GamesDB.Platforms.PlatformID
			where GamesDB.Releases.GameID=@gameID
			)

GO
/****** Object:  UserDefinedFunction [GamesDB].[gameReviews]    Script Date: 07/06/2019 15:48:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create function [GamesDB].[gameReviews] (@gameID int) returns Table
as
	return (select *
			from GamesDB.Reviews
			where GamesDB.Reviews.GameID=@gameID
			)

GO
/****** Object:  UserDefinedFunction [GamesDB].[gameTournments]    Script Date: 07/06/2019 15:48:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create function [GamesDB].[gameTournments] (@gameID int) returns Table
as
	return (select Name
			from GamesDB.Tournments
			where GamesDB.Tournments.GameID=@gameID
			)

GO
/****** Object:  UserDefinedFunction [GamesDB].[getGameUserReview]    Script Date: 07/06/2019 15:48:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create function [GamesDB].[getGameUserReview] (@UserName varchar(max), @gameID int) returns Table
as
	return (select *
			from GamesDB.Reviews
			where GamesDB.Reviews.GameID=@gameID and GamesDB.Reviews.UserName=@UserName
			)

GO
/****** Object:  UserDefinedFunction [GamesDB].[userInfo]    Script Date: 07/06/2019 15:48:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create function [GamesDB].[userInfo] (@userName char(30)) returns Table
as
	return (select *
			from GamesDB.[Users]
			where UserName=@userName
			)

GO
