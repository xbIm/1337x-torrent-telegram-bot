module Domain.Request

open Fable.Core
open Common

type Url = Url of string

let unwrapUrl (Url a) = a

type Response =
    { html: string
      statusCode: int
      ``try``: int }

type Request = Url -> JS.Promise<Response>
