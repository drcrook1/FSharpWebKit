namespace FSharpWeb2.Controllers
open System
open System.Collections.Generic
open System.Linq
open System.Net.Http
open System.Web.Http

/// Retrieves values.
[<RoutePrefix("api2/values")>]
type ValuesController() =
    inherit ApiController()
    let values = [|"value1";"value2"|]

    /// Gets all values.
    [<Route("")>]
    member x.Get() = values

    /// Gets the value with index id.
    [<Route("{id:int}")>]
    member x.Get(id) : IHttpActionResult =
        if id > values.Length - 1 then
            x.BadRequest() :> _
        else x.Ok(values.[id]) :> _

    /// Gets the value with index id.
    [<Route("{id:int}")>]
    member x.Post(id) : IHttpActionResult =
        if ((id % 2) = 0)
        then  x.Ok(true) :> _
        else x.Ok(false) :> _