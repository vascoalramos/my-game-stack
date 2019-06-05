drop function GamesDB.gameReviews;
go

go
create function GamesDB.gameReviews (@gameID int) returns Table
as
	return (select *
			from GamesDB.Reviews
			where GamesDB.Reviews.GameID=@gameID
			)
go