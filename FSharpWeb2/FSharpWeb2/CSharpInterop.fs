namespace FSharpWeb2

module CSharpInterop =
    let ToCSharpDelegate(f : function) = 
        let f' = new 
