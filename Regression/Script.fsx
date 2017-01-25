#I @"..\packages"
#r @"FSharp.Data\lib\net40\FSharp.Data.dll"
#load @"FSharp.Charting\FSharp.Charting.fsx"

open FSharp.Data
open System.IO
open FSharp.Charting

let fileName = "day.csv"
let path = Path.Combine(__SOURCE_DIRECTORY__, @"..\Data\", fileName)

type Data = CsvProvider<"""..\Data\day.csv""">
let dataset = Data.Load(path)

let data = dataset.Rows
let all = Chart.Line [for obs in data -> obs.Cnt ]

let windowedExample = 
    [1..10]
    |> Seq.windowed 3
    |> Seq.toList


let ma n (series:float seq) = 
    series
    |> Seq.windowed n
    |> Seq.map (fun xs -> xs |> Seq.average)
    |> Seq.toList

let count = 
    [for obs in data -> float obs.Cnt ]
    |> Seq.ofList

Chart.Combine [
    Chart.Line count
    Chart.Line (ma 7 count)
    Chart.Line (ma 30 count) ]