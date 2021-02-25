open Farmer
open Farmer.Builders
open System

[<EntryPoint>]
let main argv =

    let rgName = argv.[0]
    
    // Create ARM resources here e.g. webApp { } or storageAccount { } etc.
    // See https://compositionalit.github.io/farmer/api-overview/resources/ for more details.
    let pocServicebus = serviceBus {
        name "sb-elisa-ghactions-poc"
        sku ServiceBus.Sku.Standard
        add_topics [
            topic { 
                name "msgfromsource"
                add_subscriptions [
                    subscription {
                        name "submsgfromsource"
                    }
                ]
            }
        ]
    }

    let pocStorage = storageAccount {
        name "stelisaghactionspoc"
        sku Storage.Sku.Standard_LRS
    }

    let pocWebapp = webApp {
        name "app-elisa-farmer-poc"
        service_plan_name "plan-elisaghactions-poc"
        sku WebApp.Sku.S1
        always_on
        app_insights_off
        system_identity
    }

    let pocFunctions = functions {
        name "func-elisa-ghactions-poc"
        link_to_service_plan "plan-elisaghactions-poc"
        link_to_storage_account "stelisaghactionspoc"
        app_insights_off
        system_identity
    }

    let pocVault = keyVault {
        name "kv-elisa-ghactions-poc"
        add_secrets ["demouser";"demouserpw"]
        add_access_policies [
            AccessPolicy.create pocFunctions.SystemIdentity
        ]
    }

    // Add resources to the ARM deployment using the add_resource keyword.
    // See https://compositionalit.github.io/farmer/api-overview/resources/arm/ for more details.
    let deployment = arm {
        location Location.WestEurope
        add_resource pocServicebus
        add_resource pocStorage
        add_resource pocWebapp
        add_resource pocFunctions
        add_resource pocVault
    }


    deployment
    |> Deploy.execute rgName Deploy.NoParameters
    |> printfn "%A"

    0