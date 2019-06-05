drop procedure GamesDB.uspAddGameToList
go

go
create procedure GamesDB.uspAddGameToList
	@gameID int, 
    @userName varchar(max), 
    @listName varchar(max),
	@responseMsg nvarchar(250) output

as
	begin
		begin transaction
			begin try
				if 0 = ( select GamesDB.checkGameInUser (@userName, @gameID) )
					begin
						insert into GamesDB.GameEventList (GameID, EventID, UserName, RegDate, ChangeDate)
						values (@gameID, (select EventID from GamesDB.[Events] join GamesDB.EventType on GamesDB.[Events].TypeID=GamesDB.EventType.TypeID where Name=@listName and UserName=@userName), @userName, getdate(), null)
					end
	
				else
					begin
						declare @eventID as int;
						set @eventID = (select EventID from GameEventList where GameID=@gameID and UserName=@userName and ChangeDate is null)
				
						if exists (	select *
									from GamesDB.Games join GamesDB.GameEventList on GamesDB.Games.GameID=GamesDB.GameEventList.GameID
										join GamesDB.[Events] on GamesDB.GameEventList.EventID=GamesDB.[Events].EventID
									where GamesDB.[Events].UserName=@userName and GamesDB.Games.GameID=@gameID and GamesDB.[Events].TypeID = (select TypeID from GamesDB.EventType where Name=@listName)
									)
							begin
								update GamesDB.GameEventList
								set ChangeDate = null
								where UserName = @userName and GameID = @gameID and EventID = (select EventID from GamesDB.[Events] join GamesDB.EventType on GamesDB.[Events].TypeID=GamesDB.EventType.TypeID where Name=@listName and UserName=@userName)
	
								update GamesDB.GameEventList
								set ChangeDate = getdate()
								where UserName = @userName and GameID = @gameID and EventID = @eventID
							end

						else
							begin
								insert into GamesDB.GameEventList (GameID, EventID, UserName, RegDate, ChangeDate)
								values (@gameID, (select EventID from GamesDB.[Events] join GamesDB.EventType on GamesDB.[Events].TypeID=GamesDB.EventType.TypeID where Name=@listName and UserName=@userName), @userName, getdate(), null)
									
								update GamesDB.GameEventList
								set ChangeDate = getdate()
								where UserName = @userName and GameID = @gameID and EventID = @eventID
							end
					end
				SET @responseMsg='Success'
			end try
			begin catch
				SET @responseMsg='Error adding game to List'
			end catch
		commit transaction
	end
go