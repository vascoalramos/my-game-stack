DROP PROCEDURE GamesDB.uspAddGame;
GO

DROP FUNCTION GamesDB.splitArray;
GO


CREATE FUNCTION GamesDB.splitArray --This function creates a table with the split values of an array to be used on the stored procedure
    (
	  @argSourceStr varchar(MAX) = NULL, 
      @argDelimiter char(1) = ';'
	)
	RETURNS 
		@SPLIT_STR TABLE (id INT, subStr VARCHAR(MAX))
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

CREATE PROCEDURE GamesDB.uspAddGame
	@game_name VARCHAR(max), 
    @launch_date varchar(max), 
    @publisher varchar(max),
    @photo varchar(max),
	@franchise varchar(max),
	@description varchar(max),

	@developers varchar(max), 
    @genres varchar(max), 

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
	
			--Developer
	
			DECLARE @SPLIT_STR TABLE (id INT, sub_str VARCHAR(MAX))
	
			INSERT @SPLIT_STR
			SELECT * FROM splitArray (@developers, ';')
	
			DECLARE @id INT, @maxid INT
			SELECT @id = MIN(id), @maxid = MAX(id)
			FROM @SPLIT_STR
	
			WHILE @id <= @maxid
			BEGIN
			   DECLARE @working_str VARCHAR(MAX)
			   SELECT @working_str = sub_str
			   FROM @SPLIT_STR
			   WHERE id = @id
	
					INSERT INTO GamesDB.[GameDeveloper] (GameID,DeveloperID)
					VALUES (@addedGameID,@working_str)
	
			   SELECT @id += 1  
			END
	
			/*--Genre
			DECLARE @SPLIT_STR2 TABLE (id INT, sub_str VARCHAR(MAX))
	
			INSERT @SPLIT_STR2
			SELECT * FROM splitArray (@genres, ';')
	
			DECLARE @id2 INT, @maxid2 INT
			SELECT @id2 = MIN(id), @maxid2 = MAX(id)
			FROM @SPLIT_STR2
	
			WHILE @id2 <= @maxid2
			BEGIN
			   DECLARE @working_str2 VARCHAR(MAX)
			   SELECT @working_str2 = sub_str
			   FROM @SPLIT_STR2
			   WHERE id = @id2
	
					INSERT INTO GamesDB.[GameGenre] (GenreID,GameID)
					VALUES (@working_str2,@addedGameID)
	
			   SELECT @id2 += 1 
			END*/

			SET @responseMsg='Success'

	END TRY
	BEGIN CATCH
		SET @responseMsg=error_message()
	END CATCH
	COMMIT TRANSACTION
END
GO