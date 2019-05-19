drop trigger GamesDB.deleteFranchise
go

go
create trigger GamesDB.deleteFranchise on GamesDB.[Franchises]
instead of delete
as
	begin
	    begin transaction
			declare @franID as int;
			select @franID = FranchiseID FROM deleted;

			if( not exists( select * from information_schema.tables
							where table_schema = 'GamesDB' and table_name = 'Franchises_deleted'
							)
				)
			begin
				create table GamesDB.Franchises_deleted (
					FranchiseID		int				not null,
					Name			varchar(max)	not null,
					NoOfGames		int						,
					Logo			varchar(max)			,

					primary key(FranchiseID)
				);
			end

			begin try
				insert into GamesDB.Franchises_deleted
				select * from deleted

				if exists ( select 1 from GamesDB.[GameBelongsFranchise] where FranchiseID = @franID )
					delete from GamesDB.[GameBelongsFranchise] where FranchiseID = @franID

				delete from GamesDB.[Franchises] where FranchiseID = @franID
			end try

			begin catch
				raiserror ('Error deleting franchise!', 16, 1)   
			end catch
		commit transaction           
	end
go