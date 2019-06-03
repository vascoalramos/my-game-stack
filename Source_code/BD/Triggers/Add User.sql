drop trigger GamesDB.triggerAddUser;
go

go
create trigger GamesDB.triggerAddUser on GamesDB.[Users]
after insert
as
	begin
		begin transaction
			declare @userName as varchar(30);
			select @userName = UserName from inserted;

			begin try
				insert into GamesDB.[Events] values (@userName,1);
				insert into GamesDB.[Events] values (@userName,2);
				insert into GamesDB.[Events] values (@userName,3);
				insert into GamesDB.[Events] values (@userName,4);
				insert into GamesDB.[Events] values (@userName,5);
			end try

			begin catch
				declare @ErrorMessage nvarchar(max), @ErrorSeverity int, @ErrorState int;
				select @ErrorMessage = ERROR_MESSAGE() + ' Line ' + cast(ERROR_LINE() as nvarchar(5)), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE();
			    raiserror (@ErrorMessage, @ErrorSeverity, @ErrorState); 
			end catch
		commit transaction
	end
go