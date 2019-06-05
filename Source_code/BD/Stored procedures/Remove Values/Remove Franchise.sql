drop procedure GamesDB.removeFranchise;
go

go
create procedure GamesDB.removeFranchise
	@franID int,
	@responseMsg nvarchar(250) output
as
	begin
		set nocount on
		
		begin try
			delete from GamesDB.[Franchises] where FranchiseID = @franID
			set @responseMsg='Success'
		end try
		begin catch
			set @responseMsg=error_message()
		end catch
	end
go