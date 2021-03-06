DROP PROCEDURE GamesDB.uspAddTournment;
GO

GO
CREATE PROCEDURE GamesDB.uspAddTournment
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