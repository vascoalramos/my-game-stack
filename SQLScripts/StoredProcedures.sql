use Games;
go

---------- Procedure to Insert New User ----------
drop procedure dbo.uspAddUser;
go

create procedure dbo.uspAddUser
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

		insert into dbo.[User] (UserName, Email, Fname, Lname, Photo, Password_hash, Salt)
		values (@UserName, @mail, @fname, @lname, @photo, hashbytes('SHA2_512', @password + cast(@salt as nvarchar(36))), @salt)

		set @responseMsg='Success'
	end try
	begin catch
		set @responseMsg=error_message()
	end catch
end
go

 -- DECLARE @responseMsg NVARCHAR(250);
 -- exec dbo.uspAddUser @UserName = 'vramos99', @mail = 'vascoarlamos@ua.pt', @fname = 'Vasco', @lname = 'Ramos', @password = 'ola123password', @responseMsg=@responseMsg OUTPUT

select * from [User];