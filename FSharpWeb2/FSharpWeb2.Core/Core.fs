namespace FSharpWebKit2.Core

open System

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
    let bind f i =
        match i with
            | Success s -> f s
            | Failure f -> Failure f
    
    let (>>=) i f =
        bind f i