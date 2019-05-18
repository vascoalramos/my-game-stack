drop procedure GamesDB.removeGame;
go

go
create procedure GamesDB.removeGame
	@gameID int,
	@responseMsg nvarchar(250) output
as
	begin
		set nocount on
		
		begin try
			delete from GamesDB.[Games] where GameID = @gameID
			set @responseMsg='Success'
		end try
		begin catch
			set @responseMsg=error_message()
		end catch
	end
go