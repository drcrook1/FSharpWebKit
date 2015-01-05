namespace FSharpWeb2.Models

open System.ComponentModel.DataAnnotations

type LoginViewModel () =

    [<Required>]
    [<Display(Name = "Email")>]
    [<EmailAddress>]
    member val Email : string = ""
        with get,set

    [<Required>] 
    [<DataType(DataType.Password)>] 
    [<Display(Name = "Password")>] 
    member val Password : string = ""
        with get,set

    [<Display(Name = "RememberMe")>] 
    member val RememberMe : bool = false
        with get,set

type RegisterViewModel () =

    [<Required>]
    [<Display(Name = "Email")>]
    [<EmailAddress>]
    member val Email : string = ""
        with get,set

    [<Required>] 
    [<StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)>] 
    [<DataType(DataType.Password)>] 
    [<Display(Name = "Password")>] 
    member val Password : string = ""
        with get,set

    [<DataType(DataType.Password)>] 
    [<Display(Name = "Confirm password")>] 
    [<Compare("Password", ErrorMessage = "The password and confirmation password do not match.")>]
    member val ConfirmPassword : string = ""
        with get,set