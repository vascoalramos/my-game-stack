drop trigger GamesDB.deleteGenre
go

go
create trigger GamesDB.deleteGenre on GamesDB.[Genres]
instead of delete
as
	begin
	    begin transaction
			declare @genreID as int;
			select @genreID = GenreID FROM deleted;

			if( not exists( select * from information_schema.tables
							where table_schema = 'GamesDB' and table_name = 'Genres_deleted'
							)
				)
			begin
				create table GamesDB.Genres_deleted (
					GenreID		int				not null,
					Name		varchar(max)	not null,

					primary key (GenreID)
				);
			end

			begin try
				insert into GamesDB.Genres_deleted
				select * from deleted

				if exists ( select 1 from GamesDB.[GameGenre] where GenreID = @genreID )
					delete from GamesDB.[GameGenre] where GenreID = @genreID

				delete from GamesDB.[Genres] where GenreID = @genreID
			end try

			begin catch
				declare @ErrorMessage nvarchar(max), @ErrorSeverity int, @ErrorState int;
				select @ErrorMessage = ERROR_MESSAGE() + ' Line ' + cast(ERROR_LINE() as nvarchar(5)), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE();
			    raiserror (@ErrorMessage, @ErrorSeverity, @ErrorState);
			end catch
		commit transaction           
	end
go