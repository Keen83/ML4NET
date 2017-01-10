#I @"..\packages"
#r @"FSharp.Data\lib\net40\FSharp.Data.dll"
#r @"R.NET.Community\lib\net40\RDotNet.dll"
#r @"RProvider\lib\net40\RProvider.Runtime.dll"
#r @"RProvider\lib\net40\RProvider.dll"

open RProvider
open RProvider.``base``
open RProvider.graphics
open FSharp.Data

let wb = WorldBankData.GetDataContext ()
let countries = wb.Countries
let pop2010 = [for c in countries -> c.Indicators.``Population, total``.[2010]]
let surface = [for c in countries -> c.Indicators.``Surface area (sq. km)``.[2010]]

R.summary(surface) |> R.print

R.hist surface
R.hist (surface |> R.log)

R.plot (surface, pop2010)