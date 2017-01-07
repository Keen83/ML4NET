#I @"..\packages"
#r @"FSharp.Data\lib\net40\FSharp.Data.dll"

open System
open FSharp.Data

type Questions = JsonProvider<"""https://api.stackexchange.com/2.2/questions?site=stackoverflow""">

let csQuestions = """https://api.stackexchange.com/2.2/questions?order=desc&sort=activity&tagged=C%23&site=stackoverflow"""

let toDateTime (timestamp:int) =
  let start = DateTime(1970,1,1,0,0,0,DateTimeKind.Utc)
  start.AddSeconds(float timestamp).ToLocalTime()

let yymmdd (date:DateTime) = date.ToString("yy.MM.dd")

let getFormattedDate ticks =
    ticks 
    |> toDateTime
    |> yymmdd

Questions.Load(csQuestions).Items |> Seq.iter (fun q -> printfn "Question creation: %s. Display name: %s. Rate: %i" (getFormattedDate q.CreationDate) q.Owner.DisplayName q.Owner.Reputation)