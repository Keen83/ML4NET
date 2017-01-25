#I @"..\packages"
#r @"FSharp.Data\lib\net40\FSharp.Data.dll"
#r @"Deedle\lib\net40\Deedle.dll"
#r @"R.NET.Community\lib\net40\RDotNet.dll"
#r @"RProvider\lib\net40\RProvider.Runtime.dll"
#r @"RProvider\lib\net40\RProvider.dll"
#r @"D:\Keen\Documents\Visual Studio 2015\Projects\ML4NET\packages\Deedle.RPlugin\lib\net40\Deedle.RProvider.Plugin.dll"

open FSharp.Data
open Deedle
open RProvider
open RProvider.``base``
open Deedle.RPlugin
open RProvider.rworldmap

let wb = WorldBankData.GetDataContext()
let countries = wb.Countries

let population2000 = series [for c in countries -> c.Code, c.Indicators.``Population, total``.[2000]]
let population2010 = series [for c in countries -> c.Code, c.Indicators.``Population, total``.[2010]]
let surface = series [for c in countries -> c.Code, c.Indicators.``Surface area (sq. km)``.[2010]]

let ddf = frame [ 
    "Pop2000", population2000
    "Pop2010", population2010
    "Surface", surface ]
ddf?Code <- ddf.RowKeys

#r @"R.NET.Community\lib\net40\RDotNet.dll"
#r @"RProvider\lib\net40\RProvider.Runtime.dll"
#r @"RProvider\lib\net40\RProvider.dll"
#r @"Deedle.RPlugin\lib\net40\Deedle.RProvider.Plugin.dll"

let dataframe = frame [
    "Pop2000", population2000
    "Pop2010", population2010
    "Surface", surface ]

dataframe?Code <- dataframe.RowKeys

let map = R.joinCountryData2Map(dataframe, "ISO3", "Code")
R.mapCountryData(map, "Pop2010")
R.mapCountryData(map, "Surface")

dataframe?Density <- dataframe?Pop2010 / dataframe?Surface
let map = R.joinCountryData2Map(dataframe, "ISO3", "Code")
R.mapCountryData(map, "Density")

dataframe?Growth <- dataframe?Pop2010 - dataframe?Pop2000
let map = R.joinCountryData2Map(dataframe, "ISO3", "Code")
R.mapCountryData(map, "Growth")