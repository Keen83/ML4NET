// Learn more about F# at http://fsharp.org. See the 'F# Tutorial' project
// for more guidance on F# programming.

#load "NaiveBayes.fs"
open NaiveBayes.Classifier
open System.IO

// Define your library scripting code here

type DocType = 
    | Ham
    | Spam

let parseDocType (label:string) = 
    match label with
    | "ham" -> Ham
    | "spam" -> Spam
    | _ -> failwith "Unknown label"

let parseLine (line: string) =
    let split = line.Split('\t')
    let label = split.[0] |> parseDocType
    let message = split.[1]
    (label, message)

let fileName = "SMSSpamCollection"
let path = Path.Combine(__SOURCE_DIRECTORY__, @"..\Data\", fileName)

printfn "%s" path

let streamReader = new StreamReader(path)

let readLines (filePath:string) = seq {
    use sr = new StreamReader (filePath)
    while not sr.EndOfStream do
        yield sr.ReadLine ()
} 

let dataset = 
    readLines path
    |> Seq.map parseLine

let spamWithFREE = 
    dataset
    |> Seq.filter (fun(docType,_) -> docType = Spam)
    |> Seq.filter (fun(_,sms) -> sms.Contains("FREE"))
    |> Seq.length

let hamWithFREE =
    dataset
    |> Seq.filter (fun(docType,_) -> docType = Ham)
    |> Seq.filter (fun(_,sms) -> sms.Contains("FREE"))
    |> Seq.length

let primitiveClassifier (sms: string) =
    match sms.Contains("FREE") with
    | true -> Spam
    | false -> Ham

open System.Text.RegularExpressions
let matchWords = Regex(@"\w+")

let wordTokenizer (text:string) =
    text.ToLowerInvariant()
    |> matchWords.Matches
    |> Seq.cast<Match>
    |> Seq.map (fun m -> m.Value)
    |> Set.ofSeq

let training = 
    dataset
    |> Seq.skip 1000
    |> Array.ofSeq

let validation = 
    dataset
    |> Seq.take 1000
    |> Array.ofSeq

let vocabulary (tokenizer:Tokenizer) (corpus:string seq) =
    corpus
    |> Seq.map tokenizer
    |> Set.unionMany

let allTokens =
    training
    |> Seq.map snd
    |> vocabulary wordTokenizer

let txtClassifier = train training wordTokenizer (["txt"] |> Set)
let fullClassifier = train training wordTokenizer allTokens


let evaluate (tokenizer:Tokenizer) (tokens:Token Set) =
    let classifier = train training tokenizer tokens
    validation 
    |> Seq.averageBy (fun (docType, sms) ->
        if docType = classifier sms then 1.0 else 0.0)
    |> printfn "Correctly classified: %.3f"

evaluate wordTokenizer (["txt"] |> Set)
evaluate wordTokenizer allTokens

let casedTokenizer (text:string) =
    text
    |> matchWords.Matches
    |> Seq.cast<Match>
    |> Seq.map (fun m -> m.Value)
    |> Set.ofSeq

let casedTokens =
    training
    |> Seq.map snd
    |> vocabulary casedTokenizer
