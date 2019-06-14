drop function GamesDB.getGameUserReview;
go

go
create function GamesDB.getGameUserReview (@UserName varchar(max), @gameID int) returns Table
as
	return (select *
			from GamesDB.Reviews
			where GamesDB.Reviews.GameID=@gameID and GamesDB.Reviews.UserName=@UserName
			)
go