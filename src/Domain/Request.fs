module Domain.Request

open Fable.Core

//add multi domain
type Url = Url of string

let unwrapUrl (Url a) = a

type Response =
    { html: string
      statusCode: int
      ``try``: int }

type Request = Url -> JS.Promise<Response>

module Url =
  let create (s: string) =
       Url <|
       if s.StartsWith "/" then
         s
       else
         sprintf "/%s" s


