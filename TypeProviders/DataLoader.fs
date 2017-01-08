#I @"..\packages"
#r @"FSharp.Data\lib\net40\FSharp.Data.dll"

open FSharp.Data

namespace TypeProviders

module DataLoader =
    // https://avtobazar.ua/api2/search.json/car/?price_to=30000&model1=17162&year_from=2008&country1=1911&show_only=only_used&make1=1121&per-page=10
    let baseUrl = """https://avtobazar.ua/api2/search.json/car"""
    
    let reqAccept = """text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8"""
    let reqAcceptEncoding = """gzip, deflate, sdch, br"""
    let reqAcceptLanguage = "en-US,en;q=0.8,uk;q=0.6,ru;q=0.4,nb;q=0.2,da;q=0.2"
    let reqCacheControl = "no-cache"
    let reqConnection = "keep-alive"
    let userAgent = """Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36"""
    let reqMethod = "GET"
    let reqCookie = """csrftoken=4d1c8f16cca3c8c4ae125528b2f2fd8a; user_favorites=; _ym_uid=1483876410596243170; _ym_isad=2; __lnkrntdmcvrd=-1; my_last_query=make1%3D1121%26amp%3Bmodel1%3D17162%26amp%3Bprice_to%3D30000%26amp%3Byear_from%3D2008%26amp%3Bcountry1%3D1911%26amp%3Bshow_only%3Donly_used; 49b757c84352f4d092f3d2add210fcaf=d013cf48390f8fc2a5e91bf78339a032; _ga=GA1.2.475026849.1483876407"""
    let reqHost = "avtobazar.ua"
    let reqPragma = "no-cache"

    let testUrl = """https://91.230.121.40:443/api2/search.json/car/?price_to=30000&model1=17162&year_from=2008&country1=1911&show_only=only_used&make1=1121&per-page=10"""

    let reqHeaders = ["Accept", reqAccept; "Accept-Encoding", reqAcceptEncoding; "Accept-Language", reqAcceptLanguage; "Cache-Control", reqCacheControl; "Cookie", reqCookie; "Host", reqHost; "Pragma", reqPragma; "Upgrade-Insecure-Requests", "1"; "User-Agent", userAgent ]

    let res = Http.RequestString(testUrl, httpMethod = reqMethod, headers = reqHeaders)