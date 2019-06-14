drop function GamesDB.userInfo;
go

go
create function GamesDB.userInfo (@userName char(30)) returns Table
as
	return (select *
			from GamesDB.[Users]
			where UserName=@userName
			)
go