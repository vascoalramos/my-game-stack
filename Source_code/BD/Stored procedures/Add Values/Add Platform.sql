DROP PROCEDURE GamesDB.uspAddPlatform;
GO

GO
CREATE PROCEDURE GamesDB.uspAddPlatform
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