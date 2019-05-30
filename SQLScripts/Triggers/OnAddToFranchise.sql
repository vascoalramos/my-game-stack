drop trigger GamesDB.OnAddToFranchise;
go

go
create trigger GamesDB.OnAddToFranchise on GamesDB.GameBelongsFranchise
instead of insert
as
	begin
		begin transaction
			declare @franchiseID int;

			SELECT @franchiseID AS FranchiseID FROM inserted;

			UPDATE GamesDB.Franchises
			SET NoOfGames = (SELECT NoOfGames
							FROM GamesDB.Franchises JOIN GamesDB.GameBelongsFranchise ON GamesDB.Franchises.FranchiseID = GamesDB.GameBelongsFranchise.FranchiseID
							) + 1
			WHERE FranchiseID = @franchiseID

		commit transaction
	end
go