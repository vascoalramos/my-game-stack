DROP PROCEDURE GamesDB.uspAddFranchise;
GO

GO
CREATE PROCEDURE GamesDB.uspAddFranchise
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
