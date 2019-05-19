drop procedure GamesDB.removeTournment;
go

go
create procedure GamesDB.removeTournment
	@tourID int,
	@responseMsg nvarchar(250) output
as
	begin
		set nocount on
		
		begin try
			delete from GamesDB.[Tournments] where TournmentID = @tourID
			set @responseMsg='Success'
		end try
		begin catch
			set @responseMsg=error_message()
		end catch
	end
go