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

		INSERT INTO GamesDB.[Franchises] ([Name],NoOfGames,Logo)
		VALUES (@Name, 0, @Photo)

		SET @responseMsg='Success'
	END TRY
	BEGIN CATCH
		SET @responseMsg=error_message()
	END CATCH
END
GO