#r @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\System.dll"



let func1 = new System.Func<string, string>(fun s -> s + "func1" )

let printer(s : string) =
    printfn "%s" s
    s

let stuff = System.Func<System.Threading.Tasks.Task<string>, string>(fun t -> t |> printer t.Result)

let f1 x = 
    let r = async{
                        let! r' = Async.Sleep(1000) 
                        return "f1"
                    } 
                    //Task<string>
                    //needs: stem.Func<Task<T>, TResult>>
    let rom = Async.StartAsTask(r).ContinueWith(stuff)
    printfn "%A" rom.Result


let f2 x =
    let r = async{

                        return "f2"
                    } 
    let rom = Async.StartAsTask(r)
    rom.Result

let f3 x =
    let r = async{

                        return "f2"
                    } 
    let rom = Async.StartAsTask(r)
    "I FAILED"

let myFunction x = 
    f1()
    f2()
    f3()

myFunction()