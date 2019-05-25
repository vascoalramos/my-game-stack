DROP PROCEDURE GamesDB.uspAddTournment;
GO

GO
CREATE PROCEDURE GamesDB.uspAddTournment
	@tourn_name VARCHAR(max), 
    @prize varchar(max), 
    @location varchar(max),
    @start_date varchar(max),
    @end_date varchar(max),
	@game_title varchar(max),
    @responseMsg nvarchar(250) output
AS
BEGIN
	SET NOCOUNT ON

	BEGIN TRY

		INSERT INTO GamesDB.[Tournments] (GameID,[Name],StartDate,EndDate,[Location],PrizePool)
		VALUES (@game_title, @tourn_name, @start_date, @end_date, @location, @prize)

		SET @responseMsg='Success'
	END TRY
	BEGIN CATCH
		SET @responseMsg=error_message()
	END CATCH
END
GO