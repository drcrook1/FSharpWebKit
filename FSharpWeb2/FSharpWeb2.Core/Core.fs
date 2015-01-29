namespace FSharpWebKit2.Core

open System
open FSharp.Data.Sql

//type sqlSchema = SqlDataProvider<ConnectionString = @"Server=tcp:dzpnvmkk2x.database.windows.net,1433;Database=nerdgaming;
//                                                        User ID=drcrook@dzpnvmkk2x;Password=thepark23!;Trusted_Connection=False;
//                                                        Encrypt=True;Connection Timeout=30;", UseOptionTypes = true>
type FailureMessage =
    //Nerd Data Type Errors
    | FirstNameMustNotBeBlank
    | FirstNameMustNotBeOver255Characters
    | LastNameMustNotBeBlank
    | LastNameMustNotBeOver255Characters
    | PhoneMustNotBeBlank
    | PhoneMustBe7Characters
    | EmailAddressMustNotBeBlank
    | EmailAddressMustNotBeOver255Characters
    | AddressForeignKeyMustExist

type Result<'TSuccess> =
    | Success of 'TSuccess
    | Failure of FailureMessage list

type Result<'TSuccess, 'TFailure> with
    member this.SuccessValue =
        match this with
        | Success s -> s
        | Failure _ -> Unchecked.defaultof<'TSuccess>
    member this.FailureValue =
        match this with
        | Success _ -> Unchecked.defaultof<'TFailure>
        | Failure f -> f

module ROP =
    /// create a Success with no messages
    let succeed x =
        Success (x)

    let fail x f =
        Failure f

    /// given a function wrapped in a result
    /// and a value wrapped in a result
    /// apply the function to the value only if both are Success
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