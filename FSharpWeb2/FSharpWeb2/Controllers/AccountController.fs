﻿namespace FSharpWeb2.Controllers

open Microsoft.AspNet.Identity.Owin
open Microsoft.Owin.Host.SystemWeb
open Microsoft.AspNet.Identity
open System.Threading.Tasks
open System.Web
open System.Web.Mvc
open FSharpWeb2
open FSharpWeb2.Models
open System.Web.Http
//suggestions from angular perspective
//follow john papa guide.
//convert controllers to API controllers isntead of MVC controllers.
(*



*)
[<Authorize>]
[<RoutePrefix("api/Account")>]
type AccountController =
    inherit ApiController 

    [<DefaultValue>]val mutable private _userManager : ApplicationUserManager
    [<DefaultValue>]val mutable private _signInManager : ApplicationSignInManager

    new() = { inherit ApiController() }
    new(userManager : ApplicationUserManager, signInManager : ApplicationSignInManager) =
        { inherit ApiController() }

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

    [<HttpPost>]
    [<AllowAnonymous>]
    [<Route("Login")>]
    member x.Login (model : LoginViewModel) : IHttpActionResult =
        let r = async{
                        let! result = Async.AwaitTask(x.SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false))
                        return(result)
                    }
        let re = Async.RunSynchronously(r)
        match re with
        | SignInStatus.Success -> x.Ok(true) :> _
        | _ -> x.Ok(false) :> _

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

    [<HttpPost>]
    [<AllowAnonymous>]
    member x.Register (email : string, password: string) =
        async{
            let user = ApplicationUser(UserName = email, Email = email)
            let! result = Async.AwaitTask(x.UserManager.CreateAsync(user, password))
            match result.Succeeded with
            | false -> return (false)
            | _ -> 
                x.SignInManager.SignInAsync(user, false, false) |> ignore
                return (true)
        }