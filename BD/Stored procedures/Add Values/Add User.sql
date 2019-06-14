drop procedure GamesDB.uspAddUser;
go

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