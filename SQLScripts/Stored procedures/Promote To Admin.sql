drop procedure GamesDB.promoteToAdmin;
go

go
create procedure GamesDB.promoteToAdmin
	@userName varchar(30),
	@responseMsg nvarchar(250) output
as
	begin
		set nocount on

		begin try
			insert into GamesDB.[Admin] values (@userName, getdate());
			set @responseMsg = 'Success'
		end try

		begin catch
			set @responseMsg=error_message()
		end catch
	end
go