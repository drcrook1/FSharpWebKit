namespace FSharpWeb2

open Microsoft.Owin
open Owin
open System
open Microsoft.AspNet.Identity.Owin
open Microsoft.Owin.Security.Cookies
open Microsoft.AspNet.Identity

type appUserManagerD = delegate of (IdentityFactoryOptions<ApplicationUserManager> * IOwinContext) -> ApplicationUserManager

type Startup () =

    let regenIdentity(manager : ApplicationUserManager, user : ApplicationUser) =
        user.GenerateUserIdentityAsync(manager)

    let ConfigureAuth(app : IAppBuilder) =
        app.CreatePerOwinContext(ApplicationDbContext.Create) |> ignore
        let func = new System.Func<Microsoft.AspNet.Identity.Owin.IdentityFactoryOptions<ApplicationUserManager>,
                                    IOwinContext,ApplicationUserManager>(fun options context -> ApplicationUserManager.Create (options, context) )
        let func2 = new System.Func<Microsoft.AspNet.Identity.Owin.IdentityFactoryOptions<ApplicationSignInManager>,
                                    IOwinContext,ApplicationSignInManager>(fun options context -> ApplicationSignInManager.Create (options, context) )
        let func3 = new System.Func<ApplicationUserManager, ApplicationUser, 
                                    Threading.Tasks.Task<Security.Claims.ClaimsIdentity>>(fun manager user -> regenIdentity (manager, user) )

        app.CreatePerOwinContext<ApplicationUserManager>(func) |> ignore
        app.CreatePerOwinContext<ApplicationSignInManager>(func2) |> ignore
        let t = TimeSpan.FromMinutes 30.0
        let validIdent = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(t, func3)

        let cookieAuthProvider = new CookieAuthenticationProvider(OnValidateIdentity = validIdent)
        let cookieAuthOptions = new CookieAuthenticationOptions(AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                                                                    LoginPath = new PathString("/Account/Login"),
                                                                    Provider = cookieAuthProvider)
        app.UseCookieAuthentication(cookieAuthOptions) |> ignore
        app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie)
        let t' = TimeSpan.FromMinutes 5.0
        app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, t')
        app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie)
        //TODO: Add additional authentication with third party providers.
        //app.UseMicrosoftAccountAuthentication
        //(clientId = "", clientSecret = "")
        //
        //app.UseTwitterAuthentication
        //app.UseFacebookAuthentication
        //app.UseGoogleAuthentication -> remember you need GoogleOAuth2AuthenticationOptions as well. probably jsut blank ones.
        ()

    member x.Configuration (app : IAppBuilder) =
        ConfigureAuth app
        ()
        