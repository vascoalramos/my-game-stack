drop trigger GamesDB.OnAddToFranchise;
go

go
create trigger GamesDB.OnAddToFranchise on GamesDB.GameBelongsFranchise
after insert
as
	begin
		declare @franchiseID int;

		SELECT @franchiseID=FranchiseID FROM inserted;

		UPDATE GamesDB.Franchises
		SET NoOfGames = (SELECT distinct NoOfGames
						FROM GamesDB.Franchises JOIN GamesDB.GameBelongsFranchise ON GamesDB.Franchises.FranchiseID = GamesDB.GameBelongsFranchise.FranchiseID
						WHERE GamesDB.Franchises.FranchiseID = @franchiseID
						) + 1
		WHERE FranchiseID = @franchiseID
	end
go