namespace FSharpWeb2

open Microsoft.AspNet.Identity.Owin
open Microsoft.AspNet.Identity
open Microsoft.AspNet.Identity.EntityFramework
open System.Threading.Tasks
open System.Security.Claims
open System.Data
open Microsoft.Owin
open Microsoft.Owin.Security
open System

type ApplicationUser() as this =
    inherit IdentityUser()
    member x.GenerateUserIdentityAsync (manager : UserManager<ApplicationUser>) = 
        manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie)
//        async{   
//            //let! is a let binding with await while Async.AwaitTask converts C# Tast<t> to F# Async<a>
//            let! userIdentity = Async.AwaitTask(manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie)) 
//            return(userIdentity)
//        }

type ApplicationDbContext() =
    inherit IdentityDbContext<ApplicationUser>("DefaultConnection", throwIfV1Schema=false)
    static member Create() =
        new ApplicationDbContext()

[<AllowNullLiteral>]
type ApplicationUserManager(store : IUserStore<ApplicationUser>) =
    inherit UserManager<ApplicationUser>(store)
    static member Create(options : IdentityFactoryOptions<ApplicationUserManager>, context : IOwinContext) =
        let manager = new ApplicationUserManager(new UserStore<ApplicationUser> (context.Get<ApplicationDbContext>()), 
                        PasswordValidator = new PasswordValidator(RequiredLength = 6, 
                            RequireNonLetterOrDigit = true, 
                            RequireDigit = true, 
                            RequireLowercase = true, 
                            RequireUppercase = true))
        manager.UserValidator <- UserValidator<ApplicationUser>(manager,
                                    AllowOnlyAlphanumericUserNames = false,
                                    RequireUniqueEmail = true)
        manager.UserLockoutEnabledByDefault <- true
        manager.DefaultAccountLockoutTimeSpan <- TimeSpan.FromMinutes(5.0)
        manager.MaxFailedAccessAttemptsBeforeLockout <- 5
        let dataProtectionProvider = options.DataProtectionProvider
        if dataProtectionProvider <> null
            then
            manager.UserTokenProvider <- DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
        manager

[<AllowNullLiteral>]
type ApplicationSignInManager(userManager : ApplicationUserManager, authenticationManager : IAuthenticationManager) =
    inherit SignInManager<ApplicationUser, string>(userManager, authenticationManager)
        override this.CreateUserIdentityAsync(user : ApplicationUser) =
            let userManager = this.UserManager :?> ApplicationUserManager
            user.GenerateUserIdentityAsync(userManager)
        static member Create(options : IdentityFactoryOptions<ApplicationSignInManager>, context : IOwinContext) = 
            new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication)

