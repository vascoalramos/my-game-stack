drop procedure GamesDB.removePublisher;
go

go
create procedure GamesDB.removePublisher
	@pubID int,
	@responseMsg nvarchar(250) output
as
	begin
		set nocount on
		
		begin try
			delete from GamesDB.[Publishers] where PublisherID = @pubID
			set @responseMsg='Success'
		end try
		begin catch
			set @responseMsg=error_message()
		end catch
	end
go