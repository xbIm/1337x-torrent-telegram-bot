module Tests.Validation

open Domain.Bot
open App.SearchOnSite
open Tests.Util

let validation () =
    describe "Validation" <| fun () ->
        it "normal frase" <| fun () ->
            UserRequest "test"
            |> validateUserRequest
            |> isOk
            |> assertTrue
