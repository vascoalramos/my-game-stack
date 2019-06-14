drop procedure GamesDB.uspAddReview
go

go
create procedure GamesDB.uspAddReview
	@score VARCHAR(max), 
    @title varchar(max), 
    @description varchar(max),
	@userName varchar(max),
	@gameID int

as
	begin
		insert into GamesDB.Reviews (Score, Title, [Description], UserName, GameID)
		values (@score, @title, @description, @userName, @gameID)
	end
go
