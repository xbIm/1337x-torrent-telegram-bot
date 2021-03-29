module Tests.Util

open Fable.Core
open Fable.Core.Testing

[<Global>]
let describe (name: string) (f: unit -> unit) = jsNative

[<Global>]
let it (msg: string) (f: unit -> unit) = jsNative

let assertEqual expected actual: unit =
    Assert.AreEqual(actual, expected)

let assertTrue actual: unit =
    Assert.AreEqual(actual, true)

[<ImportAll("fs")>]
let fs: Node.Fs.IExports = jsNative

let readFileSync: string -> string = fun str -> fs.readFileSync (str, "UTF-8")

let isOk (res) =
    match (res) with
    | Ok _ -> true
    | Error _ -> false
