go
create procedure GamesDB.addReview
as
	begin
		insert int GamesDB.Reviews (Score, Title, [Description], [Date], UserName, GameID)
		values ()
	end
go