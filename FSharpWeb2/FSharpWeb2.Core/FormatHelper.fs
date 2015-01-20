namespace FSharpWebKit2.Core

open System
open ROP

//    | FirstNameMustNotBeBlank
//    | FirstNameMustNotBeOver255Characters
//    | LastNameMustNotBeBlank
//    | LastNameMustNotBeOver255Characters
//    | PhoneMustNotBeBlank
//    | PhoneMustBe7Characters
//    | EmailAddressMustNotBeBlank
//    | EmailAddressMustNotBeOver255Characters
//    | AddressForeignKeyMustExist

module FormatHelper =
    let FirstNameNotBlank(s:string)=
        if not (String.IsNullOrEmpty s)
        then Success s
        else Failure [FirstNameMustNotBeBlank]
    
    let FirstNameNotOver255Characters(s:string)=
        if s.Length < 256
        then Success s
        else Failure [FirstNameMustNotBeOver255Characters]

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

    let CreateLastName(s:string)=
        s |> LastNameNotBlank
            >>= LastNameNotOver255Characters