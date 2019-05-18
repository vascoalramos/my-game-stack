drop trigger GamesDB.deleteUser
go

go
create trigger GamesDB.deleteUser on GamesDB.[Users]
instead of delete
as
	begin
	    begin transaction
			declare @userName as varchar(30);
			select @userName = UserName FROM deleted;

			if( not exists( select * from information_schema.tables
							where table_schema = 'GamesDB' and table_name = 'Users_deleted'
							)
				)
			begin
				create table GamesDB.Users_deleted (
					UserName		varchar(30)		unique		not null,
					Email			varchar(max)				not null,
					Fname			varchar(max)						,
					Lname			varchar(max)						,
					Photo			varchar(max)						,
					Password_hash	binary(64)					not null,	-- secure way to store passwords
					Salt			uniqueidentifier			not null,	-- used to more secure password storing

					primary key (UserName)
				);
			end

			begin try
				insert into GamesDB.Users_deleted
				select * from deleted

				if exists ( select 1 from GamesDB.[Reviews] where UserName = @userName )
					delete from GamesDB.[Reviews] where UserName = @userName

				if exists ( select 1 from GamesDB.[GameEventList] where UserName = @userName )
					delete from GamesDB.[GameEventList] where UserName = @userName
					
				if exists ( select 1 from GamesDB.[Events] where UserName = @userName )
					delete from GamesDB.[Events] where UserName = @userName

				if exists ( select 1 from GamesDB.[Admin] where UserName = @userName )
					delete from GamesDB.[Admin] where UserName = @userName

				delete from GamesDB.[Users] where UserName = @userName
			end try

			begin catch
				raiserror ('Error deleting user!', 16, 1)   
			end catch
		commit transaction           
	end
go