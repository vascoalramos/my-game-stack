use master;
go

alter database Games set single_user with rollback immediate;
go

drop database Games;
go

create database Games;
go

use Games;
go

create table [Users] (
	UserName		varchar(30)		unique		not null,
	Email			varchar(max)				not null,
	Fname			varchar(max)						,
	Lname			varchar(max)						,
	Photo			varchar(max)						,
	Password_hash	binary(64)					not null,	-- secure way to store passwords
	Salt			uniqueidentifier			not null,	-- used to more secure password storing

	primary key (UserName)
);

create table EventType (
	TypeID	int				identity(1,1)	not null,	-- auto-increment feature
	Name	varchar(30)							,

	primary key	(TypeID)
);

create table [Events] (
	EventID		int				identity(1,1)	not null,	-- auto-increment feature
	UserName	varchar(30)						not null,
	TypeID		int								not null,

	primary key (EventID, UserName)
);

create table Games (
	GameID			int				identity(1,1)	not null,	-- auto-increment feature
	Title			varchar(max)					not null,
	LauchDate		date									,
	[Description]	varchar(max)							,
	PubID			int								not null,
	CoverImage		varchar(max)							,
	
	primary key (GameID)	
);

create table Franchises (
	FranchiseID		int				identity(1,1)	not null,	-- auto-increment feature
	Name			varchar(max)					not null,
	NoOfGames		int										,
	Logo			varchar(max)							,

	primary key(FranchiseID)
);

create table GameBelongsFranchise (
	GameID			int		not null,
	FranchiseID		int		not null,

	primary key	(GameID, FranchiseID)
);

create table Tournments (
	TournmentID		int				identity(1,1)	not null,	-- auto-increment feature
	GameID			int								not null,
	Name			varchar(max)							,
	StartDate		date									,
	EndDate			date									,
	Location		varchar(max)							, -- ver notas!!! dicidir
	PrizePool		int										,
	
	primary key (TournmentID)		
);

create table Genres (
	GenreID		int				identity(1,1)	not null,	-- auto-increment feature
	Name		varchar(max)					not null,

	primary key (GenreID)
);

create table GameGenre (
	GenreID		int		not null,
	GameID		int		not null,

	primary key (GenreID, GameID)
);

create table Reviews (
	ReviewID		int				identity(1,1)	not null,	-- auto-increment feature
	Score			int										,	-- stars
	Title			varchar(max)					not null,
	[Description]	varchar(max)							,
	[Date]			date							not null,
	UserName		varchar(30)						not null,
	GameID			int								not null,

	primary key (ReviewID, GameID)
);

create table Publishers (
	PublisherID		int				identity(1,1)	not null,	-- auto-increment feature
	Name			varchar(max)					not null,
	Email			varchar(max)							,
	Phone			varchar(max)							,
	Website			varchar(max)							,
	City			varchar(max)							,
	Country			varchar(max)							,
	Logo			varchar(max)							,			

	primary key (PublisherID)
);

create table Developers (
	DeveloperID		int				identity(1,1)	not null,	-- auto-increment feature
	Name			varchar(max)					not null,
	Email			varchar(max)							,
	Phone			varchar(max)							,
	Website			varchar(max)							,
	City			varchar(max)							,
	Country			varchar(max)							,
	Logo			varchar(max)							,			

	primary key (DeveloperID)
);

create table [Platforms] (
	PlatformID		int				identity(1,1)	not null,	-- auto-increment feature
	Name			varchar(max)					not null,
	[Owner]			varchar(max)							,
	ReleaseDate		date									,

	primary key (PlatformID)
);

create table Releases (
	GameID		int		not null,
	PlatformID	int		not null,

	primary key (GameID, PlatformID)
);

create table GameEventList (
	GameID		int				not null,
	EventID		int				not null,
	UserName	varchar(30)		not null,
	RegDate		date					,
	ChangeDate	date					,

	primary key (GameID, EventID, UserName)
);

create table GameDeveloper (
	GameID		int		not null,
	DeveloperID	int		not null,

	primary key (GameID, DeveloperID)
);

alter table [Events] add constraint eventUser foreign key (UserName) references [Users] (UserName);
alter table [Events] add constraint event_type foreign key (TypeID) references EventType (TypeID);

alter table Reviews add constraint reviewGame foreign key (GameID) references Games (GameID);
alter table Reviews add constraint reviewUser foreign key (UserName) references [Users] (UserName);

alter table GameEventList add constraint gameEventListEvent foreign key (EventID, UserName) references [Events] (EventID, UserName);

alter table GameEventList add constraint gameEventListGame foreign key (GameID) references Games (GameID);

alter table Games add constraint gamePublisher foreign key (PubID) references Publishers (PublisherID);

alter table GameBelongsFranchise add constraint gameBelongsFranchiseGame foreign key (GameID) references Games (GameID);
alter table GameBelongsFranchise add constraint gameBelongsFranchiseFranchise foreign key (FranchiseID) references Franchises (FranchiseID);

alter table GameGenre add constraint gameGenreGame foreign key (GameID) references Games (GameID);
alter table GameGenre add constraint gameGenreGenre foreign key (GenreID) references Genres (GenreID);

alter table Releases add constraint releasesGame foreign key (GameID) references Games (GameID);
alter table Releases add constraint releasesPlatform foreign key (PlatformID) references [Platforms] (PlatformID);

alter table Tournments add constraint tournmentGame foreign key (GameID) references Games (GameID);

alter table GameDeveloper add constraint developerGameDeveloper foreign key (DeveloperID) references Developers (DeveloperID);
alter table GameDeveloper add constraint gameDeveloperGame foreign key (GameID) references Games (GameID);