DROP PROCEDURE GamesDB.uspAddGenre;
GO

GO
CREATE PROCEDURE GamesDB.uspAddGenre
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