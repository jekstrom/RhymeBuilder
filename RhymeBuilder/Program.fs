open System
open FSharp.Data
open RestSharp

type WordRelations = JsonProvider<"relatedWords.json">
let apiKey = "ea56ee02fa9178f6b8201030bb6026b4fbbdb4bba1d2c018c"
let baseUri = "http://api.wordnik.com/v4/word.json"
let client = new RestClient(baseUri)

let getRelatedWordsUri word relationshipType =
    let query = sprintf "useCanonical=false&relationshipTypes=%s&limitPerRelationshipType=10&api_key=%s" relationshipType apiKey
    sprintf "/%s/relatedWords?%s" word query

let sendRequest (resource : string) =
    let request = new RestRequest(resource, Method.GET)
    client.Execute(request)

[<EntryPoint>]
let rec main argv = 
    let word = Console.ReadLine()
    let response = getRelatedWordsUri word "rhyme" |> sendRequest
    let relations = WordRelations.Parse(response.Content)
    let rhymes = relations |> Seq.map (fun r -> r.Words |> String.concat ", ")
    rhymes |> String.concat "\n" |> printf "%s\n"
    main argv