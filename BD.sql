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
	TypeID	int				identity(1,1)	not null,	-- auto -increment feature
	Name	varchar(max)							,

	primary key	(TypeID)
);

create table [Event] (
	EventID		int				identity(1,1)	not null,	-- auto -increment feature
	UserName	varchar(max)					not null,
	TypeID		int								not null,
	RegDate		date									,
	ChangeDate	date									,

	primary key (EventID)
);