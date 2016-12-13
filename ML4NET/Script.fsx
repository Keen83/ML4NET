// Learn more about F# at http://fsharp.org. See the 'F# Tutorial' project
// for more guidance on F# programming.

#load "Library1.fs"
open ML4NET
open System.IO

// Define your library scripting code here

let trainingPath = Path.Combine(__SOURCE_DIRECTORY__, @"Data\trainingsample.csv")
let validationPath = Path.Combine(__SOURCE_DIRECTORY__, @"Data\validationsample.csv")

File.Exists trainingPath
File.Exists validationPath

type Observation = { Label: string; Pixels: int[]}

let toInt (str:string) = 
    let str = "Hello!"
    printf "%s" str
    int str

let toObservation (csvData:string) = 
    let columns = csvData.Split(',')
    let label = columns.[0]
    let pixels = 
        columns.[1..] 
        |> Array.map int 
    {Label = label; Pixels = pixels }
    
let reader path =
    let data = File.ReadAllLines path
    data.[1..]
    |> Array.map toObservation

type Distance = int[] * int[] -> int

let manhattanDistance (pixels1, pixels2) = 
    Array.zip pixels1 pixels2
    |> Array.map (fun (x,y) -> abs(x - y))
    |> Array.sum

let euclideanDistance (pixels1, pixels2) = 
    Array.zip pixels1 pixels2
    |> Array.map (fun (x, y) -> pown (x-y) 2)
    |> Array.sum

let train(trainingset:Observation[]) (dist:Distance) = 
    let classify (pixels:int[]) = 
        trainingset
        |> Array.minBy (fun x -> dist (x.Pixels, pixels))
        |> fun x -> x.Label
    classify

let trainingData = reader trainingPath

let manhattanClassifier = train trainingData manhattanDistance
let euclideanCassifier = train trainingData euclideanDistance

let validationData = reader validationPath

let evaluate data classifier = 
    data
    |> Array.averageBy (fun x -> if classifier x.Pixels = x.Label then 1. else 0.)
    |> printfn "Correct: %.3f"

printfn "Manhatten"
evaluate validationData manhattanClassifier

printfn "Euclidean"
evaluate validationData euclideanCassifier