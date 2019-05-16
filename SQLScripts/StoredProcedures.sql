---------- Procedure to Insert New User ----------
drop procedure GamesDB.uspAddUser;
go

drop procedure GamesDB.uspLogin;
go

create procedure GamesDB.uspAddUser
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
go


create procedure GamesDB.uspLogin
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
go

--declare	@responseMessage nvarchar(250)

--EXEC	dbo.uspLogin
--		@loginName = 'adamLamb',
--		@password = 'D3X9It57',
--		@responseMessage = @responseMessage OUTPUT

--SELECT	@responseMessage as '@responseMessage'

----Incorrect login
--EXEC	dbo.uspLogin
--		@loginName = 'adamLamww', 
--		@password = 'D3X9It57',
--		@responseMessage = @responseMessage OUTPUT

--SELECT	@responseMessage as '@responseMessage'

----Incorrect password
--EXEC	dbo.uspLogin
--		@pLoginName = 'adamLamb', 
--		@pPassword = 'D3X9It58',
--		@responseMessage = @responseMessage OUTPUT

--SELECT	@responseMessage as '@responseMessage'