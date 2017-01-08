#I @"..\packages"
#r @"FSharp.Data\lib\net40\FSharp.Data.dll"

open System
open FSharp.Data

type Questions = JsonProvider<"""https://api.stackexchange.com/2.2/questions?site=stackoverflow""">

let csQuestions = """https://api.stackexchange.com/2.2/questions?order=desc&sort=activity&tagged=C%23&site=stackoverflow"""

let toDateTime (timestamp:int) =
  let start = DateTime(1970,1,1,0,0,0,DateTimeKind.Utc)
  start.AddSeconds(float timestamp).ToLocalTime()

let yymmdd (date:DateTime) = date.ToString("yyyy.MM.dd")

let getFormattedDate ticks =
    ticks 
    |> toDateTime
    |> yymmdd

Questions.Load(csQuestions).Items |> Seq.iter (fun q -> printfn "Question creation: %s. Display name: %s. Rate: %i" (getFormattedDate q.CreationDate) q.Owner.DisplayName q.Owner.Reputation)

Questions.GetSample().Items |> Seq.iter (fun q -> printfn "Question creation: %s. Display name: %s. Rate: %i" (getFormattedDate q.CreationDate) q.Owner.DisplayName q.Owner.Reputation)

let questionQuery = """https://api.stackexchange.com/2.2/questions?site=stackoverflow"""

let tagged tags query = 
    let joinedTAgs = tags |> String.concat ";"
    sprintf "%s&tagged=%s" query joinedTAgs

let page p query = sprintf "%s&page=%i" query p
let pageSize s query = sprintf "%s&pagesize=%i" query s
let extractQuestions (query:string) = Questions.Load(query).Items

let ``C#`` = "C%23"
let ``F#`` = "F%23"

let fsSample =
    questionQuery
    |> tagged [``F#``]
    |> pageSize 100
    |> extractQuestions

let csSample = 
    questionQuery
    |> tagged [``C#``]
    |> pageSize 100
    |> extractQuestions

let analyzeTags (qs:Questions.Item seq) =
    qs
    |> Seq.collect (fun question -> question.Tags)
    |> Seq.countBy id
    |> Seq.filter (fun (_,count) -> count > 2)
    |> Seq.sortBy (fun (_,count) -> -count)
    |> Seq.iter (fun (tag,count) -> printfn "%s - %i" tag count)

analyzeTags fsSample
analyzeTags csSample