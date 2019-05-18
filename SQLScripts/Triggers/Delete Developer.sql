drop trigger GamesDB.deleteDeveloper
go

go
create trigger GamesDB.deleteDeveloper on GamesDB.[Developers]
instead of delete
as
	begin
	    begin transaction
			declare @devID as int;
			select @devID = DeveloperID FROM deleted;

			if( not exists( select * from information_schema.tables
							where table_schema = 'GamesDB' and table_name = 'Developers_deleted'
							)
				)
			begin
				create table GamesDB.Developers_deleted (
					DeveloperID		int				not null,	
					Name			varchar(max)	not null,
					Email			varchar(max)			,
					Phone			varchar(max)			,
					Website			varchar(max)			,
					City			varchar(max)			,
					Country			varchar(max)			,
					Logo			varchar(max)			,			

					primary key (DeveloperID)
				);
			end

			begin try
				insert into GamesDB.Developers_deleted
				select * from deleted

				if exists ( select 1 from GamesDB.[GameDeveloper] where DeveloperID = @devID )
					delete from GamesDB.[GameDeveloper] where DeveloperID = @devID

				delete from GamesDB.[Developers] where DeveloperID = @devID
			end try

			begin catch
				raiserror ('Error deleting user!', 16, 1)   
			end catch
		commit transaction           
	end
go