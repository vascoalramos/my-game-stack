drop procedure GamesDB.removeDeveloper;
go

go
create procedure GamesDB.removeDeveloper
	@devID int,
	@responseMsg nvarchar(250) output
as
	begin
		set nocount on
		
		begin try
			delete from GamesDB.[Developers] where DeveloperID = @devID
			set @responseMsg='Success'
		end try
		begin catch
			set @responseMsg=error_message()
		end catch
	end
go
