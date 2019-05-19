drop procedure GamesDB.removeGenre;
go

go
create procedure GamesDB.removeGenre
	@genreID int,
	@responseMsg nvarchar(250) output
as
	begin
		set nocount on
		
		begin try
			delete from GamesDB.[Genres] where GenreID = @genreID
			set @responseMsg='Success'
		end try
		begin catch
			set @responseMsg=error_message()
		end catch
	end
go