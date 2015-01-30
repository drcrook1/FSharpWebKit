#r @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\System.dll"

let function1 id : string =
    "1" + id
let function2 id : string = 
    "2" + id
let function3 id : string =
    "3" + id
let function4 id : float =
    let r = id.ToString()
    "4" + r