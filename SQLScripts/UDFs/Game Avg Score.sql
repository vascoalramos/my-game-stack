drop function GamesDB.gameAvgScore;
go

go
create function GamesDB.gameAvgScore (@gameID int) returns float
as
	begin
		declare @avg_score float
		select @avg_score=round(avg(cast(Score as float)),2)
		from GamesDB.Reviews
		where GameID=@gameID
		return @avg_score
	end
go