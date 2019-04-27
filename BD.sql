create database Games;
go;


create table [User](
	UserName		varchar(max)	not null,
	Email			varchar(max)	not null,
	Fname			varchar(max)			,
	Lname			varchar(max)			,
	Password_hash	binary(64)		not null,	-- secure way to store passwords

	primary key (UserName)
);

create table EventType(
	ID		int				identity(1,1)	not null,	-- auto -increment feature
	Name	varchar(max)							,

	primary key	(ID)
);