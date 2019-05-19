drop trigger GamesDB.deleteGame
go

go
create trigger GamesDB.deleteGame on GamesDB.[Games]
instead of delete
as
	begin
	    begin transaction
			declare @gameID as int;
			select @gameID = GameID FROM deleted;

			if( not exists( select * from information_schema.tables
							where table_schema = 'GamesDB' and table_name = 'Games_deleted'
							)
				)
			begin
				create table GamesDB.Games_deleted (
					GameID			int				not null,
					Title			varchar(max)	not null,
					LauchDate		date					,
					[Description]	varchar(max)			,
					PubID			int				not null,
					CoverImage		varchar(max)			,
	
					primary key (GameID)
				);
			end

			begin try
				insert into GamesDB.Games_deleted
				select * from deleted

				if exists ( select 1 from GamesDB.[GameGenre] where GameID = @gameID )
					delete from GamesDB.[GameGenre] where GameID = @gameID

				if exists ( select 1 from GamesDB.[GameDeveloper] where GameID = @gameID )
					delete from GamesDB.[GameDeveloper] where GameID = @gameID

				if exists ( select 1 from GamesDB.[Releases] where GameID = @gameID )
					delete from GamesDB.[Releases] where GameID = @gameID

				if exists ( select 1 from GamesDB.[Tournments] where GameID = @gameID )
					delete from GamesDB.[Tournments] where GameID = @gameID

				if exists ( select 1 from GamesDB.[GameBelongsFranchise] where GameID = @gameID )
					delete from GamesDB.[GameBelongsFranchise] where GameID = @gameID

				if exists ( select 1 from GamesDB.[GameEventList] where GameID = @gameID )
					delete from GamesDB.[GameEventList] where GameID = @gameID

				if exists ( select 1 from GamesDB.[Reviews] where GameID = @gameID )
					delete from GamesDB.[Reviews] where GameID = @gameID

				delete from GamesDB.[Games] where GameID = @gameID
			end try

			begin catch
				declare @ErrorMessage nvarchar(max), @ErrorSeverity int, @ErrorState int;
				select @ErrorMessage = ERROR_MESSAGE() + ' Line ' + cast(ERROR_LINE() as nvarchar(5)), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE();
			    raiserror (@ErrorMessage, @ErrorSeverity, @ErrorState); 
			end catch
		commit transaction           
	end
go