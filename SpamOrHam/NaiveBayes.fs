namespace NaiveBayes
// Implement next logic:
// Score(SMS is Spam|SMS contains "driving,cant,txt") = 
//      Log(P(SMS is Spam)) + Log(Laplace(SMS contains "driving"|Spam))
//          + Log(Laplace(SMS contains "cant"|Spam)) + Log(Laplace(SMS contains "txt"|Spam))
module Classifier = 

    type Token = string
    type Tokenizer = string -> Token Set
    type TokenizedDoc = Token Set

    type DocsGroup = 
        { Proportion: float;
        TokenFrequencies: Map<Token, float> }

    let tokenScore (group:DocsGroup) (token:Token) =
        match group.TokenFrequencies.ContainsKey token with
        | true -> log group.TokenFrequencies.[token]
        | _ -> 0.0

    let tokenScore1 (group:DocsGroup) (token:Token) =
        if group.TokenFrequencies.ContainsKey token
        then log group.TokenFrequencies.[token]
        else 0.0

    let score (document:TokenizedDoc) (group:DocsGroup) = 
        let scoreToken = tokenScore group
        log group.Proportion + 
        (document |> Seq.sumBy scoreToken)

    let classify (groups:(_*DocsGroup)[]) (tokenizer:Tokenizer) (txt:string) =
        let tokenized = tokenizer txt
        groups
        |> Array.maxBy (fun(label, group) ->
            score tokenized group)
        |> fst

    let Hello name = printfn "Hello, %s from Classifier module" name
