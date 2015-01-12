Drop Table Nerd
Drop Table StreetAddress
Drop Table GameType
Drop Table NerdGames

CREATE TABLE Nerd(
	Id int NOT NULL IDENTITY(1,1) PRIMARY KEY,
	FirstName varchar(255) NOT NULL,
	LastName varchar(255) NOT NULL,
	Phone varchar(30) NOT NULL,
	EmailAddress varchar(255) NOT NULL,
	StreetAddressId int NOT NULL,
	FavoriteGames varchar(255) NOT NULL
	)

CREATE TABLE StreetAddress(
	Id int NOT NULL IDENTITY(1,1) PRIMARY KEY,
	StreetOne varchar(255) NOT NULL,
	StreetTwo varchar(255),
	City varchar (255) NOT NULL,
	StateCode varchar (2) NOT NULL,
	PostalCode varchar (10) NOT NULL,
	Latitude float NOT NULL,
	Longitude float NOT NULL
)

Create Table GameType(
	Id int NOT NULL IDENTITY(1,1) PRIMARY KEY,
	GameTypeName varchar(255) NOT NULL
)

INSERT GameType VALUES
('Strategy'),
('Action'),
('Role Playing')

Create Table NerdGames(
	Id int NOT NULL IDENTITY(1,1) PRIMARY KEY,
	Name varchar(255) NOT NULL,
	GameTypeId int NOT NULL,
	GameDescription varchar(255) NOT NULL
)

