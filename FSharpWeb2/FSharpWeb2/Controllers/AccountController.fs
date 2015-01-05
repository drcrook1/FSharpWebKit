namespace FSharpWeb2.Controllers

open Microsoft.AspNet.Identity.Owin
open Microsoft.Owin.Host.SystemWeb
open Microsoft.AspNet.Identity
open System.Threading.Tasks
open System.Web
open System.Web.Mvc
open FSharpWeb2
open FSharpWeb2.Models

[<Authorize>]
type AccountController =
    inherit Controller 

    [<DefaultValue>]val mutable private _userManager : ApplicationUserManager
    [<DefaultValue>]val mutable private _signInManager : ApplicationSignInManager

    new() = { inherit Controller() }
    new(userManager : ApplicationUserManager, signInManager : ApplicationSignInManager) =
        { inherit Controller() }

    member x.UserManager 
        with get () = 
            if x._userManager <> null
            then HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
            else x._userManager
        and set (value) =
            x._userManager <- value
    
    member x.SignInManager 
        with get () = 
            if x._signInManager <> null
            then HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>()
            else x._signInManager
        and set (value) =
            x._signInManager <- value

    [<AllowAnonymous>]
    member x.Login () =
        x.View()

    [<AllowAnonymous>]
    member x.Register () =
        x.View()

    [<HttpPost>]
    [<AllowAnonymous>]
    member x.Login (model : LoginViewModel) =
        async{
            let! result = Async.AwaitTask(x.SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false))
            match result with
            | SignInStatus.Success -> return (true)
            | _ -> 
                x.ModelState.AddModelError("", "Invalid Login Attempt") 
                return (false)
        }

    [<HttpPost>]
    [<AllowAnonymous>]
    member x.Register (model : RegisterViewModel) =
        async{
            let user = ApplicationUser(UserName = model.Email, Email = model.Email)
            let! result = Async.AwaitTask(x.UserManager.CreateAsync(user, model.Password))
            match result.Succeeded with
            | false -> return (false)
            | _ -> 
                x.SignInManager.SignInAsync(user, false, false) |> ignore
                return (true)
        }