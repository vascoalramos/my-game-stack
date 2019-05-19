drop trigger GamesDB.deletePlatform
go

go
create trigger GamesDB.deletePlatform on GamesDB.[Platforms]
instead of delete
as
	begin
	    begin transaction
			declare @platID as int;
			select @platID = PlatformID FROM deleted;

			if( not exists( select * from information_schema.tables
							where table_schema = 'GamesDB' and table_name = 'Platforms_deleted'
							)
				)
			begin
				create table GamesDB.Platforms_deleted (
					PlatformID		int				not null,
					Name			varchar(max)	not null,
					[Owner]			varchar(max)			,
					ReleaseDate		date					,

					primary key (PlatformID)
				);
			end

			begin try
				insert into GamesDB.Platforms_deleted
				select * from deleted

				if exists ( select 1 from GamesDB.[Releases] where PlatformID = @platID )
					delete from GamesDB.[Releases] where PlatformID = @platID

				delete from GamesDB.[Platforms] where PlatformID = @platID
			end try

			begin catch
				declare @ErrorMessage nvarchar(max), @ErrorSeverity int, @ErrorState int;
				select @ErrorMessage = ERROR_MESSAGE() + ' Line ' + cast(ERROR_LINE() as nvarchar(5)), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE();
			    raiserror (@ErrorMessage, @ErrorSeverity, @ErrorState);
			end catch
		commit transaction           
	end
go