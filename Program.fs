open Farmer
open Farmer.Builders
open System

[<EntryPoint>]
let main argv =

    let rgName = argv.[0]
    
    // Create ARM resources here e.g. webApp { } or storageAccount { } etc.
    // See https://compositionalit.github.io/farmer/api-overview/resources/ for more details.
    let stg = storageAccount {
        name "stelisaghactionspoc"
        sku Storage.Sku.Standard_LRS
    }

    // Add resources to the ARM deployment using the add_resource keyword.
    // See https://compositionalit.github.io/farmer/api-overview/resources/arm/ for more details.
    let deployment = arm {
        location Location.WestEurope
        add_resource stg
    }


    deployment
    |> Deploy.execute rgName Deploy.NoParameters
    |> printfn "%A"

    0