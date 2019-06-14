drop procedure GamesDB.removeUser;
go

go
create procedure GamesDB.removeUser
	@userName varchar(30),
	@responseMsg nvarchar(250) output
as
	begin
		set nocount on
		
		begin try
			delete from GamesDB.[Users] where UserName = @userName
			set @responseMsg='Success'
		end try
		begin catch
			set @responseMsg=error_message()
		end catch
	end
go