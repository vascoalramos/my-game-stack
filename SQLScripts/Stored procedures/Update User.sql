drop procedure GamesDB.uspUpdateUser;
go

go
create procedure GamesDB.uspUpdateUser
	@UserName varchar(max),
	@mail VARCHAR(max), 
    @password varchar(max), 
    @fname varchar(max),
    @lname varchar(max),
	@photo varchar(max),
    @responseMsg nvarchar(250) output
as
begin
	set nocount on

	declare @salt uniqueidentifier=newid()

	begin try
		if @fname <> 'None'
		begin
			update GamesDB.Users
			set Fname = @fname
			where UserName = @UserName
		end

		if @lname <> 'None'
		begin
			update GamesDB.Users
			set Lname = @lname
			where UserName = @UserName
		end

		if @mail <> 'None'
		begin
			update GamesDB.Users
			set Email = @mail
			where UserName = @UserName
		end

		if @password <> 'None'
		begin
			update GamesDB.Users
			set Password_hash = hashbytes('SHA2_512', @password + cast(@salt as nvarchar(36))), Salt = @salt
			where UserName = @UserName
		end

		if @photo <> 'None'
		begin
			update GamesDB.Users
			set Photo = @photo
			where UserName = @UserName
		end

		set @responseMsg='Success'
	end try
	begin catch
		set @responseMsg=error_message()
	end catch
end
go