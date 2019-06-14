drop function GamesDB.gameFranchise;
go

go
create function GamesDB.gameFranchise (@gameID int) returns varchar(max)
as
	begin
		declare @fran varchar(max)
		select @fran=Name
		from GamesDB.GameBelongsFranchise join GamesDB.Franchises on GamesDB.GameBelongsFranchise.FranchiseID=GamesDB.Franchises.FranchiseID
		where GameID=@gameID
		return @fran
	end
go