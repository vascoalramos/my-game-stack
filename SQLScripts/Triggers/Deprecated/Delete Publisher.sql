drop trigger GamesDB.deletePublisher
go

go
create trigger GamesDB.deletePublisher on GamesDB.[Publishers]
instead of delete
as
	begin
	    begin transaction
			declare @pubID as int;
			select @pubID = PublisherID FROM deleted;

			if( not exists( select * from information_schema.tables
							where table_schema = 'GamesDB' and table_name = 'Publishers_deleted'
							)
				)
			begin
				create table GamesDB.Publishers_deleted (
					PublisherID		int				not null,
					Name			varchar(max)	not null,
					Email			varchar(max)			,
					Phone			varchar(max)			,
					Website			varchar(max)			,
					City			varchar(max)			,
					Country			varchar(max)			,
					Logo			varchar(max)			,			

					primary key (PublisherID)
				);
			end

			begin try
				insert into GamesDB.Publishers_deleted
				select * from deleted

				if exists ( select 1 from GamesDB.[Games] where PubID = @pubID )
				begin
					declare @responseMsg nvarchar(250)
					exec GamesDB.removeGame @gameID=@pubID, @responseMsg=@responseMsg

					select * from GamesDB.[Games] where PubID=2
				end

				delete from GamesDB.[Publishers] where PublisherID = @pubID
			end try

			begin catch
				declare @ErrorMessage nvarchar(max), @ErrorSeverity int, @ErrorState int;
				select @ErrorMessage = ERROR_MESSAGE() + ' Line ' + cast(ERROR_LINE() as nvarchar(5)), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE();
			    raiserror (@ErrorMessage, @ErrorSeverity, @ErrorState);
			end catch
		commit transaction           
	end
go


select * from GamesDB.[Publishers]
delete from GamesDB.Publishers where PublisherID=2