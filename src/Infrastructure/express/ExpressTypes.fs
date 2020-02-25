module ExpressTypes

open System
open System.Collections.Generic

type IRequest=
    abstract body: obj with get
    abstract ``params``: obj with get
    abstract headers: IDictionary<string, string>

type IResponse =
    abstract member sendStatus: int -> unit
    abstract member redirect: string -> unit

type RouterHandler = IRequest -> IResponse -> unit

type IRouter =
     abstract member get: string -> RouterHandler -> unit
     abstract member post: string -> RouterHandler -> unit


type Application =
    inherit IRouter
    abstract member ``use``: obj -> unit
    abstract member listen: int -> (Exception -> unit) -> unit
