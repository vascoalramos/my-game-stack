drop procedure GamesDB.removePlatform;
go

go
create procedure GamesDB.removePlatform
	@platID int,
	@responseMsg nvarchar(250) output
as
	begin
		set nocount on
		
		begin try
			delete from GamesDB.[Platforms] where PlatformID = @platID
			set @responseMsg='Success'
		end try
		begin catch
			set @responseMsg=error_message()
		end catch
	end
go