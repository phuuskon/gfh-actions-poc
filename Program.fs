open Farmer
open Farmer.Builders
open System

[<EntryPoint>]
let main argv =

    let azAppId = argv.[0]
    let azSecret = argv.[1]
    let azTenantId = argv.[2]

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


    printf "azAppId: %s" azAppId
    //deployment |> Writer.quickWrite "output"
    //printfn "all done! Template written to output.json"

    // Alternatively, deploy your resource group directly to Azure here.
    Deploy.authenticate azAppId azSecret azTenantId
    |> ignore

    //deployment
    //|> Deploy.authenticate
    //|> Deploy.execute "rg-ghactions-poc" Deploy.NoParameters
    //|> printfn "%A"

    0