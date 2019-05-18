drop function GamesDB.checkAdmin;
go

go
create function GamesDB.checkAdmin (@userName varchar(30)) returns int
as
	begin
		if (not exists( select *
					from GamesDB.[Admin]
					where UserName=@userName
					)
			)
			return 0
		return 1
	end
go

select GamesDB.checkAdmin ('admin1');
go