#I @"..\packages"
#r @"FSharp.Data\lib\net40\FSharp.Data.dll"

open FSharp.Data

let wb = WorldBankData.GetDataContext ()
wb.Countries.Japan.CapitalCity

let countries = wb.Countries

let pop2000 = [for c in countries -> c.Indicators.``Population, total``.[2000]]
let pop2010 = [for c in countries -> c.Indicators.``Population, total``.[2010]]
let pop2013 = [for c in countries -> (c.Name, c.Indicators.``Electricity production from renewable sources, excluding hydroelectric (% of total)``.[2013], c.Indicators.``Electricity production from renewable sources, excluding hydroelectric (kWh)``.[2013])]
let pop2014 = [for c in countries -> (c.Name, c.Indicators.``Electricity production from renewable sources, excluding hydroelectric (% of total)``.[2014], c.Indicators.``Electricity production from renewable sources, excluding hydroelectric (kWh)``.[2014])]
let pop2015 = [for c in countries -> (c.Name, c.Indicators.``Electricity production from renewable sources, excluding hydroelectric (% of total)``.[2015], c.Indicators.``Electricity production from renewable sources, excluding hydroelectric (kWh)``.[2015])]

let filterExistingData (dataList:(string*float*float) list) = 
    let sortedList = 
        dataList 
        |> List.filter (fun (_,pros,_) -> pros > 0.0)
        |> List.sortBy (fun (_,pros,_) -> -pros)

    let count = min (sortedList |> List.length) 10

    let top10 = 
        sortedList
        |> List.take count

    dataList 
    |> List.filter (fun (name,_,_) -> name = "Ukraine")
    |> List.append top10


let greenEl2013 = filterExistingData pop2013
let greenEl2014 = filterExistingData pop2014
let greenEl2015 = filterExistingData pop2015