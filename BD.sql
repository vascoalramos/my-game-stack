create database Games;
go;


create table [User](
	UserName		varchar(max)	not null,
	Email			varchar(max)	not null,
	Fname			varchar(15)				,
	Lname			varchar(15)				,
	Password_hash	binary(64)		not null,	-- secure way to store passwords

	primary key (UserName)
);

create table [Event