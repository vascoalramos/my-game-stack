drop procedure GamesDB.uspLogin;
go

create procedure GamesDB.uspLogin
    @loginName varchar(max),
    @password varchar(max),
    @responseMessage varchar(250)='' output
as
begin
    set nocount on

	declare @userName varchar(max)

    if exists (select top 1 UserName from GamesDB.[Users] where UserName=@loginName)
    begin
		set @userName=(select UserName from GamesDB.[Users] where UserName=@loginName and Password_hash=hashbytes('SHA2_512', @password+cast(Salt as nvarchar(36))))
		
		if(@userName is null)
			set @responseMessage='Incorrect password'
		else
			set @responseMessage='User successfully logged in'
    end
    else
		set @responseMessage='Invalid login'

end
go