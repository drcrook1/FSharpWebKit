namespace FSharpWeb2.Controllers
open System.Web.Mvc

/// Retrieves values.
type RoutesDemoController() =
    inherit Controller()

    member this.One () =
        this.View()

    member this.Two (donuts : int) =
        this.ViewData.Add("Donuts", donuts)
        this.View()
    
    [<Authorize>]
    member this.Three () =
        this.View()