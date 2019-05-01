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

create table [User] (
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

create table [Event] (
	EventID		int				identity(1,1)	not null,	-- auto-increment feature
	UserName	varchar(30)						not null,
	TypeID		int								not null,
	RegDate		date									,
	ChangeDate	date									,

	primary key (EventID)
);

create table Game (
	GameID			int				identity(1,1)	not null,	-- auto-increment feature
	Title			varchar(max)					not null,
	LauchDate		date									,
	[Description]	varchar(max)							,
	PubID			int								not null,
	CoverImage		varchar(max)							,
	
	primary key (GameID)	
);

create table Franchise (
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

create table Tournment (
	TournmentID		int				identity(1,1)	not null,	-- auto-increment feature
	PrizePool		int										,
	Location		varchar(max)							, -- ver notas!!! dicidir
	Name			varchar(max)							,
	GameID			int								not null,
	
	primary key (TournmentID)		
);

create table Genre (
	GenreID		int				identity(1,1)	not null,	-- auto-increment feature
	Name		varchar(max)					not null,

	primary key (GenreID)
);

create table GameGenre (
	GenreID		int		not null,
	GameID		int		not null,

	primary key (GenreID, GameID)
);

create table Review (
	ReviewID		int				identity(1,1)	not null,	-- auto-increment feature
	Score			int										,	-- stars
	Title			varchar(max)					not null,
	[Description]	varchar(max)							,
	[Date]			date							not null,
	UserName		varchar(30)								not null,
	GameID			int								not null,

	primary key (ReviewID, GameID)
);

create table Publisher (
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

create table Developer (
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

create table [Platform] (
	PlatformID		int				identity(1,1)	not null,	-- auto-increment feature
	Name			varchar(max)					not null,
	[Owner]			varchar(max)							,
	Support			varchar(max)							,	-- see what to do here
	ReleaseDate		date									,
	City			varchar(max)							,
	Country			varchar(max)							,

	primary key (PlatformID)
);

create table Releases (
	GameID		int		not null,
	PlatformID	int		not null,

	primary key (GameID, PlatformID)
);

create table GameEventList (
	GameID		int		not null,
	EventID		int		not null,

	primary key (GameID, EventID)
);

create table GameDeveloper (
	GameID		int		not null,
	DeveloperID	int		not null,

	primary key (GameID, DeveloperID)
);

alter table [Event] add constraint eventUser foreign key (UserName) references [User] (UserName);
alter table [Event] add constraint event_type foreign key (TypeID) references EventType (TypeID);

alter table Review add constraint reviewGame foreign key (GameID) references Game (GameID);
alter table Review add constraint reviewUser foreign key (UserName) references [User] (UserName);

alter table GameEventList add constraint gameEventListEvent foreign key (EventID) references [Event] (EventID);
alter table GameEventList add constraint gameEventListGame foreign key (GameID) references Game (GameID);

alter table Game add constraint gamePublisher foreign key (PubID) references Publisher (PublisherID);

alter table GameBelongsFranchise add constraint gameBelongsFranchiseGame foreign key (GameID) references Game (GameID);
alter table GameBelongsFranchise add constraint gameBelongsFranchiseFranchise foreign key (FranchiseID) references Franchise (FranchiseID);

alter table GameGenre add constraint gameGenreGame foreign key (GameID) references Game (GameID);
alter table GameGenre add constraint gameGenreGenre foreign key (GenreID) references Genre (GenreID);

alter table Releases add constraint releasesGame foreign key (GameID) references Game (GameID);
alter table Releases add constraint releasesPlatform foreign key (PlatformID) references [Platform] (PlatformID);

alter table Tournment add constraint tournmentGame foreign key (GameID) references Game (GameID);

alter table GameDeveloper add constraint developerGameDeveloper foreign key (DeveloperID) references Developer (DeveloperID);
alter table GameDeveloper add constraint gameDeveloperGame foreign key (GameID) references Game (GameID);