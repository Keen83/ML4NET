﻿namespace NaiveBayes
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

    let proportion count total = float count / float total

    let laplace count total = float (count + 1) / float (total + 1)

    let countIn (group:TokenizedDoc seq) (token:Token) =
        group
        |> Seq.filter (Set.contains token)
        |> Seq.length

    let analyze (group:TokenizedDoc seq)
                (totalDocs:int)
                (classificationTokens:Token Set) =
        let groupSize = group |> Seq.length
        
        let score token = 
            let count = countIn group token
            laplace count groupSize
        
        let scoredTokens = 
            classificationTokens
            |> Set.map (fun token -> token, score token)
            |> Map.ofSeq
        
        let groupProportion = proportion groupSize totalDocs

        {
            Proportion = groupProportion
            TokenFrequencies = scoredTokens
        }

    let learn (docs:(_*string)[])
              (tokenizer:Tokenizer)
              (classificationTokens:Token Set) =
        let total = docs.Length
        docs
        |> Seq.map (fun (label, docs) -> label, tokenizer docs)
        |> Seq.groupBy fst
        |> Seq.map (fun (label, group) -> label, group |> Seq.map snd)
        |> Seq.map (fun (label, group) -> label, analyze group total classificationTokens)
        |> Seq.toArray       

    let train (docs:(_*string)[])
              (tokenizer:Tokenizer)
              (classificationTokens:Token Set) =
        let groups = learn docs tokenizer classificationTokens
        let classifier = classify groups tokenizer
        classifier

    let Hello name = printfn "Hello, %s from Classifier module" name
