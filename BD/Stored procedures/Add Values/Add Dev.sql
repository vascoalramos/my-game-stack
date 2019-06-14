DROP PROCEDURE GamesDB.uspAddDeveloper;
GO

GO
CREATE PROCEDURE GamesDB.uspAddDeveloper
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