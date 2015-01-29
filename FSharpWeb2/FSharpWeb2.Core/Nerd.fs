namespace FSharpWebKit2.Core

open System
open FSharp.Data
open ROP

[<CLIMutable>]
type Nerd = {id:int; FirstName:string; LastName:string; 
    Phone:string; EmailAddress:string; StreetAddressId:int; FavoriteGames:string}

module NerdHelper =
    let CreateNerd id firstName lastName phone emailAddress streetAddressId favGames =
        {id = id; FirstName = firstName; LastName = lastName; Phone = phone; 
            EmailAddress = emailAddress; StreetAddressId = streetAddressId; 
            FavoriteGames = favGames}


type INerdRepository =
    abstract member Get : int -> Result<Nerd> 
    abstract member GetAll : unit -> Result<Nerd list>
    abstract member GetAllForAContactInformation : int -> Result<Nerd list>
    abstract member Add : Nerd -> Result<Nerd>
    abstract member Update : Nerd -> Result<Nerd>

//type SqlServerNerdRepository (connectionString:string) =
//    let context = sqlSchema.GetDataContext()
//
//    let convertSqlToNerd(nerd : sqlSchema.dataContext.``[dbo].[Nerd]Entity``) =
//        0
    