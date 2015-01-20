namespace FSharpWebKit2.Core

open System
open FSharp.Data
open ROP
//CREATE TABLE Nerd(
//	Id int NOT NULL IDENTITY(1,1) PRIMARY KEY,
//	FirstName varchar(255) NOT NULL,
//	LastName varchar(255) NOT NULL,
//	Phone varchar(30) NOT NULL,
//	EmailAddress varchar(255) NOT NULL,
//	StreetAddressId int NOT NULL,
//	FavoriteGames varchar(255) NOT NULL
//	)

[<CLIMutable>]
type Nerd = {id:int; FirstName:string; LastName:string option; 
    Phone:string; EmailAddress:string; StreetAddressId:int; FavoriteGames:string}

type INerdRepository =
    abstract member Get : int -> Result<Nerd> 
    abstract member GetAll : unit -> Result<Nerd list>
    abstract member GetAllForAContactInformation : int -> Result<Nerd list>
    abstract member Add : Nerd -> Result<Nerd>
    abstract member Update : Nerd -> Result<Nerd>

type SqlServerNerdRepository (connectionString:string) =
    let context = sqlSchema.GetDataContext()

    let convertSqlToNerd(nerd : sqlSchema.dataContext.``[dbo].[Nerd]Entity``) =
        let nameResult = FormatHelper.CreateFirstName nerd.FirstName
        let fullResult = bubbleUpR(nameResult, FormatHelper.CreateLastName, nerd.LastName)
        let r = FormatHelper.CreateFirstName nerd.FirstName
                <*!*>FormatHelper.CreateLastName, nerd.LastName
        match fullResult with
        | Success s -> Success s
        | Failure f -> Failure f
    