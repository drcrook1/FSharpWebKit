namespace FSharpWebKit2.Core

open System
open FSharp.Data.Sql

type sqlSchema = SqlDataProvider<ConnectionString = @"Server=tcp:dzpnvmkk2x.database.windows.net,1433;Database=nerdgaming;
                                                        User ID=drcrook@dzpnvmkk2x;Password=thepark23!;Trusted_Connection=False;
                                                        Encrypt=True;Connection Timeout=30;", UseOptionTypes = true>
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
    let bubbleUpR(result : Result<'TSuccess>, fa, [<ParamArray>] args) =
        match result with
        | Success s -> fa args
        | Failure f -> Failure f

    let (<*!*>) r f a = 
        bubbleUpR(r, f, a)

    let bind f i =
        match i with
            | Success s -> f s
            | Failure f -> Failure f
    
    let (>>=) i f =
        bind f i