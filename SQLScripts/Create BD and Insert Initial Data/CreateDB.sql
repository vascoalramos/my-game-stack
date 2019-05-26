--create schema GamesDB;
--go

alter table GamesDB.[Events] drop constraint eventUser;
alter table GamesDB.[Events] drop constraint event_type;
alter table GamesDB.Reviews drop constraint reviewGame;
alter table GamesDB.Reviews drop constraint reviewUser;
alter table GamesDB.GameEventList drop constraint gameEventListEvent;
alter table GamesDB.GameEventList drop constraint gameEventListGame;
alter table GamesDB.Games drop constraint gamePublisher;
alter table GamesDB.GameBelongsFranchise drop constraint gameBelongsFranchiseGame;
alter table GamesDB.GameBelongsFranchise drop constraint gameBelongsFranchiseFranchise;
alter table GamesDB.GameGenre drop constraint gameGenreGame;
alter table GamesDB.GameGenre drop constraint gameGenreGenre;
alter table GamesDB.Releases drop constraint releasesGame;
alter table GamesDB.Releases drop constraint releasesPlatform;
alter table GamesDB.Tournments drop constraint tournmentGame;
alter table GamesDB.GameDeveloper drop constraint developerGameDeveloper;
alter table GamesDB.GameDeveloper drop constraint gameDeveloperGame;
alter table GamesDB.[Admin] drop constraint admin_user;

drop table GamesDB.[Users];
drop table GamesDB.[Admin];
drop table GamesDB.EventType;
drop table GamesDB.[Events];
drop table GamesDB.Games;
drop table GamesDB.Franchises;
drop table GamesDB.GameBelongsFranchise;
drop table GamesDB.Tournments;
drop table GamesDB.Genres;
drop table GamesDB.GameGenre;
drop table GamesDB.Reviews;
drop table GamesDB.Publishers;
drop table GamesDB.Developers;
drop table GamesDB.[Platforms];
drop table GamesDB.Releases;
drop table GamesDB.GameEventList;
drop table GamesDB.GameDeveloper;

create table GamesDB.[Users] (
	UserName		varchar(30)		unique		not null,
	Email			varchar(max)				not null,
	Fname			varchar(max)						,
	Lname			varchar(max)						,
	Photo			varchar(max)						,
	Password_hash	binary(64)					not null,	-- secure way to store passwords
	Salt			uniqueidentifier			not null,	-- used to more secure password storing

	primary key (UserName)
);

create table GamesDB.[Admin](
	UserName		varchar(30)	unique	not null,
	[Start_date]	date						,

	primary key (UserName)
);

create table GamesDB.EventType (
	TypeID	int				identity(1,1)	not null,	-- auto-increment feature
	Name	varchar(30)							,

	primary key	(TypeID)
);

create table GamesDB.[Events] (
	EventID		int				identity(1,1)	not null,	-- auto-increment feature
	UserName	varchar(30)						not null,
	TypeID		int								not null,

	primary key (EventID, UserName)
);

create table GamesDB.Games (
	GameID			int				identity(1,1)	not null,	-- auto-increment feature
	Title			varchar(100)	unique			not null,
	LauchDate		date									,
	[Description]	varchar(max)							,
	PubID			int										,
	CoverImage		varchar(max)							,
	
	primary key (GameID)	
);

create table GamesDB.Franchises (
	FranchiseID		int				identity(1,1)	not null,	-- auto-increment feature
	Name			varchar(100)	unique			not null,
	NoOfGames		int										,
	Logo			varchar(max)							,

	primary key(FranchiseID)
);

create table GamesDB.GameBelongsFranchise (
	GameID			int		not null,
	FranchiseID		int		not null,

	primary key	(GameID, FranchiseID)
);

create table GamesDB.Tournments (
	TournmentID		int				identity(1,1)	not null,	-- auto-increment feature
	GameID			int								not null,
	Name			varchar(100)	unique			not null,
	StartDate		date									,
	EndDate			date									,
	Location		varchar(max)							, -- ver notas!!! dicidir
	PrizePool		int										,
	
	primary key (TournmentID)		
);

create table GamesDB.Genres (
	GenreID		int				identity(1,1)	not null,	-- auto-increment feature
	Name		varchar(100)	unique			not null,

	primary key (GenreID)
);

create table GamesDB.GameGenre (
	GenreID		int		not null,
	GameID		int		not null,

	primary key (GenreID, GameID)
);

create table GamesDB.Reviews (
	ReviewID		int				identity(1,1)	not null,	-- auto-increment feature
	Score			int										,	-- stars
	Title			varchar(max)					not null,
	[Description]	varchar(max)							,
	[Date]			date							not null,
	UserName		varchar(30)						not null,
	GameID			int								not null,

	primary key (ReviewID, GameID)
);

create table GamesDB.Publishers (
	PublisherID		int				identity(1,1)	not null,	-- auto-increment feature
	Name			varchar(100)	unique			not null,
	Email			varchar(max)							,
	Phone			varchar(max)							,
	Website			varchar(max)							,
	City			varchar(max)							,
	Country			varchar(max)							,
	Logo			varchar(max)							,			

	primary key (PublisherID)
);

create table GamesDB.Developers (
	DeveloperID		int				identity(1,1)	not null,	-- auto-increment feature
	Name			varchar(100)	unique			not null,
	Email			varchar(max)							,
	Phone			varchar(max)							,
	Website			varchar(max)							,
	City			varchar(max)							,
	Country			varchar(max)							,
	Logo			varchar(max)							,			

	primary key (DeveloperID)
);

create table GamesDB.[Platforms] (
	PlatformID		int				identity(1,1)	not null,	-- auto-increment feature
	Name			varchar(100)	unique			not null,
	[Owner]			varchar(max)							,
	ReleaseDate		date									,

	primary key (PlatformID)
);

create table GamesDB.Releases (
	GameID		int		not null,
	PlatformID	int		not null,

	primary key (GameID, PlatformID)
);

create table GamesDB.GameEventList (
	GameID		int				not null,
	EventID		int				not null,
	UserName	varchar(30)		not null,
	RegDate		date					,
	ChangeDate	date					,

	primary key (GameID, EventID, UserName)
);

create table GamesDB.GameDeveloper (
	GameID		int		not null,
	DeveloperID	int		not null,

	primary key (GameID, DeveloperID)
);

alter table GamesDB.[Events] add constraint eventUser foreign key (UserName) references GamesDB.[Users] (UserName) on delete cascade;
alter table GamesDB.[Events] add constraint event_type foreign key (TypeID) references GamesDB.EventType (TypeID) on delete cascade;

alter table GamesDB.[Admin] add constraint admin_user foreign key (UserName) references GamesDB.[Users] (UserName) on delete cascade;

alter table GamesDB.Reviews add constraint reviewGame foreign key (GameID) references GamesDB.Games (GameID) on delete cascade;
alter table GamesDB.Reviews add constraint reviewUser foreign key (UserName) references GamesDB.[Users] (UserName) on delete cascade;

alter table GamesDB.GameEventList add constraint gameEventListEvent foreign key (EventID, UserName) references GamesDB.[Events] (EventID, UserName) on delete cascade;

alter table GamesDB.GameEventList add constraint gameEventListGame foreign key (GameID) references GamesDB.Games (GameID) on delete cascade;

alter table GamesDB.Games add constraint gamePublisher foreign key (PubID) references GamesDB.Publishers (PublisherID) on delete cascade;

alter table GamesDB.GameBelongsFranchise add constraint gameBelongsFranchiseGame foreign key (GameID) references GamesDB.Games (GameID) on delete cascade;
alter table GamesDB.GameBelongsFranchise add constraint gameBelongsFranchiseFranchise foreign key (FranchiseID) references GamesDB.Franchises (FranchiseID) on delete cascade;

alter table GamesDB.GameGenre add constraint gameGenreGame foreign key (GameID) references GamesDB.Games (GameID) on delete cascade;
alter table GamesDB.GameGenre add constraint gameGenreGenre foreign key (GenreID) references GamesDB.Genres (GenreID) on delete cascade;

alter table GamesDB.Releases add constraint releasesGame foreign key (GameID) references GamesDB.Games (GameID) on delete cascade;
alter table GamesDB.Releases add constraint releasesPlatform foreign key (PlatformID) references GamesDB.[Platforms] (PlatformID) on delete cascade;

alter table GamesDB.Tournments add constraint tournmentGame foreign key (GameID) references GamesDB.Games (GameID) on delete cascade;

alter table GamesDB.GameDeveloper add constraint developerGameDeveloper foreign key (DeveloperID) references GamesDB.Developers (DeveloperID) on delete cascade;
alter table GamesDB.GameDeveloper add constraint gameDeveloperGame foreign key (GameID) references GamesDB.Games (GameID) on delete cascade;