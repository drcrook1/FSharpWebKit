// Learn more about F# at http://fsharp.net. See the 'F# Tutorial' project
// for more guidance on F# programming.


open System
//#r @"C:\projects\FSharpWebKit\FSharpWeb2\packages\FSharp.Core.3.0.0.2\lib\net40\FSharp.Core.dll"
#r @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\System.dll"
// Define your library scripting code here

type FailureMessage =
    //Nerd Data Type Errors
    | FirstNameMustNotBeBlank
    | FirstNameMustNotBeOver255Characters
    | LastNameMustNotBeBlank
    | LastNameMustNotBeOver255Characters
    | NamesCannontContainATSymbol
    | PhoneMustNotBeBlank
    | PhoneMustBe7Characters
    | EmailAddressMustNotBeBlank
    | EmailAddressMustNotBeOver255Characters
    | AddressForeignKeyMustExist
    | DefaultFailure
    | PhoneNumberWrong

///Result is either a Success or a Failure with a list of errors
type Result<'TSuccess> =
    | Success of 'TSuccess
    | Failure of FailureMessage list

module ROP =
    /// create a Success with no messages
    let succeed x =
        Success (x)

    let fail x f =
        Failure f

    /// given a function wrapped in a result
    /// and a value wrapped in a result
    /// apply the function to the value only if both are Success
    /// Result<a> Result<b> turns into Result<a;b>
    /// So now we do CreateNerd Result<a;b;c;d;e;f;g>
    let applyR f result =
        match f,result with
        | Success f, Success x -> 
            f x |> Success 
        | Failure errs, Success (_) 
        | Success (_), Failure errs -> 
            errs |> Failure
        | Failure errs1, Failure errs2 -> 
            errs1 @ errs2 |> Failure 

    /// infix version of apply
    let (<*>) = applyR

    /// given a function that transforms a value
    /// apply it only if the result is on the Success branch
    let liftR f result =
        let f' =  f |> succeed
        applyR f' result 

    let (<!>) = liftR

    let bind i f =
        match i with
            | Success s -> f s
            | Failure f -> Failure f
    
    let (>>=) = bind


[<CLIMutable>]
type Nerd = {id:int; FirstName:string; LastName:string; 
    Phone:string; EmailAddress:string; StreetAddressId:int; FavoriteGames:string}


let CreateNerd id firstName lastName phone emailAddress streetAddressId favGames =
    {id = id; FirstName = firstName; LastName = lastName; Phone = phone; 
        EmailAddress = emailAddress; StreetAddressId = streetAddressId; 
        FavoriteGames = favGames}

open ROP
open System.Text.RegularExpressions

module FormatHelper =
    let FirstNameNotBlank(s:string)=
        if not (String.IsNullOrEmpty s)
        then Success s
        else Failure [FirstNameMustNotBeBlank]
    
    let FirstNameNotOver255Characters(s:string)=
        if s.Length < 256
        then Success s
        else Failure [FirstNameMustNotBeOver255Characters]



    let NameDoesNotContainATSymbol(s:string)=
        if Regex.IsMatch(s, @"*(@)")
        then Success s
        else Failure [NamesCannontContainATSymbol]

    let LastNameNotBlank(s:string)=
        if not (String.IsNullOrEmpty s)
        then Success s
        else Failure [LastNameMustNotBeBlank]
    
    let LastNameNotOver255Characters(s:string)=
        if s.Length < 256
        then Success s
        else Failure [LastNameMustNotBeOver255Characters]

    let CreateFirstName(s:string)=
        s |> FirstNameNotBlank
            >>= FirstNameNotOver255Characters
            //>>= NameDoesNotContainATSymbol

    let CreateLastName(s:string)=
        Console.WriteLine("Creating Last Name")
        s |> LastNameNotBlank
            >>= LastNameNotOver255Characters
            //>>= NameDoesNotContainATSymbol

let DoStuff () =
    CreateNerd
    <!> succeed 1
    <*> FormatHelper.CreateFirstName ""
    <*> FormatHelper.CreateLastName ""
    <*> succeed "9802286278"
    <*> succeed "david@david.com"
    <*> succeed 1
    <*> succeed "1,2,4"


//    match r with
//    | Success s -> Success s
//    | Failure f -> Failure f

let stuff = DoStuff()