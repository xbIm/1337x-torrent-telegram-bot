module Tests.Parse

open App.Parse
open System

open Domain.MagneticLink
open Tests
open Util


let parse() =
  describe "parse table" <| fun () ->
    it "parse table 1" <| fun () ->
        let html = readFileSync "./test/html/table1.html"
        let result = parseTorrentTable html 1
        match (result) with
        | Ok result -> assertEqual result.torrents.Count 20
        | Error _ -> raise <| Exception("didn't parse")

    it "parse magnetic link" <| fun () ->
        let html = readFileSync "./test/html/magnetic_link.html"
        let result = parseMagnetLink { html = html; ``try`` = 0; statusCode = 200}
        match (result) with
        | Ok result ->
            assertEqual (unwrapMagneticLink result) "magnet:?xt=urn:btih:A5E1AFF8B598D7D89D034D69B9E077AF939682FD&dn=Pawn+Sacrifice+%282014%29+%281080p+BluRay+x265+HEVC+10bit+AAC+5.1+Tigole%29+%5BQxR%5D&tr=udp%3A%2F%2Ftracker.torrent.eu.org%3A451%2Fannounce&tr=udp%3A%2F%2Ftracker.opentrackr.org%3A1337%2Fannounce&tr=udp%3A%2F%2Ftracker.internetwarriors.net%3A1337%2Fannounce&tr=udp%3A%2F%2Ftracker.zer0day.to%3A1337%2Fannounce&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969%2Fannounce&tr=udp%3A%2F%2Fcoppersurfer.tk%3A6969%2Fannounce"
        | Error _ -> raise <| Exception("didn't parse magnetic link")
  ()
