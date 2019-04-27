create database Games;
go;


create table [User] (
	UserName		varchar(max)	not null,
	Email			varchar(max)	not null,
	Fname			varchar(max)			,
	Lname			varchar(max)			,
	Password_hash	binary(64)		not null,	-- secure way to store passwords

	primary key (UserName)
);

create table EventType (
	TypeID	int				identity(1,1)	not null,	-- auto-increment feature
	Name	varchar(max)							,

	primary key	(TypeID)
);

create table [Event] (
	EventID		int				identity(1,1)	not null,	-- auto-increment feature
	UserName	varchar(max)					not null,
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
	DevID			int								not null,
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
	UserName		int								not null,
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
	ZipCode			varchar(max)							,
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
	ZipCode			varchar(max)							,
	Logo			varchar(max)							,			

	primary key (DeveloperID)
);

create table [Platform] (
	PlatformID		int				identity(1,1)	not null,	-- auto-increment feature
	Name			varchar(max)					not null,
	[Owner]			varchar(max)							,
	Support			varchar(max)							,	-- see what to do here
	ReleaseDate		date									,

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