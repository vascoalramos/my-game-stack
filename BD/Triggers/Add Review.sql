drop trigger GamesDB.triggerAddReview;
go

go
create trigger GamesDB.triggerAddReview on GamesDB.[Reviews]
instead of insert
as
	begin
		begin try
			declare @userName as varchar(100);
			declare @gameID as int;
			declare @score as int;
			declare @title as varchar(max);
			declare @description as varchar(max);
			select @userName = UserName, @gameID = GameID, @score = Score, @title = Title, @description = [Description] from inserted;

			if not exists(select * from GamesDB.[Reviews] where UserName=@userName and GameID=@gameID )
			begin
				insert into GamesDB.[Reviews] (Score, Title, [Description], [Date], UserName, GameID)
				values (@score, @title, @description, getdate(), @userName, @gameID)
			end
			else
			begin
				update GamesDB.[Reviews]
				set Score = @score, Title=@title, [Description] = @description, [Date] = getdate(), UserName = @userName, GameID = @gameID 
				where UserName=@userName and GameID=@gameID
			end
		end try
		begin catch
			declare @ErrorMessage nvarchar(max), @ErrorSeverity int, @ErrorState int;
			select @ErrorMessage = ERROR_MESSAGE() + ' Line ' + cast(ERROR_LINE() as nvarchar(5)), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE();
			raiserror (@ErrorMessage, @ErrorSeverity, @ErrorState); 
		end catch
	end
go