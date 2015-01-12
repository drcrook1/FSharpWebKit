namespace FSharpWeb2

open Microsoft.Owin
open Owin
open System
open Microsoft.AspNet.Identity.Owin

type appUserManagerD = delegate of (IdentityFactoryOptions<ApplicationUserManager> * IOwinContext) -> ApplicationUserManager

type Startup () =

    let ConfigureAuth(app : IAppBuilder) =
        app.CreatePerOwinContext(ApplicationDbContext.Create) |> ignore
        //new fn(fun x -> fun y -> function_1 x &y))
        //let appUserManagerD = new fn(fun x -> fun y -> ApplicationUserManager.Create x &y)
        //let appUserManagerD = new Action<'T, 'U>(ApplicationUserManager.Create)
        
        let func = new System.Func<Microsoft.AspNet.Identity.Owin.IdentityFactoryOptions<ApplicationUserManager>,
                                    IOwinContext,ApplicationUserManager>(fun options context -> ApplicationUserManager.Create (options, context) )

        app.CreatePerOwinContext<ApplicationUserManager>(func) |> ignore
        

        //FuncConvert.ToFSharpFunc(app.CreatePerOwinContext<ApplicationUserManager>)
        //app.CreatePerOwinContext<ApplicationUserManager>(delegateSample) |> ignore
        //app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create) |> ignore
//        app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create)
//        app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create) |> ignore
//        app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create) |> ignore
        ()

    let Configuration (app : IAppBuilder) =
        ConfigureAuth app
        ()
        