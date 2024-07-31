#r "nuget: Newtonsoft.Json, 13.0.3"

open Newtonsoft.Json.Linq
open Newtonsoft.Json
open System
open System.IO

let specificSettings () =
  async {
    let source =
      match Environment.OSVersion.Platform with
      | PlatformID.Win32NT -> ".vscode/settings.windows.json"
      | PlatformID.Unix -> ".vscode/settings.linux.json"
      | _ -> ".vscode/settings.json"

    let! sourceData =
      async {
        use file = File.OpenText(source)
        use jsonReader = new JsonTextReader(file)
        return! JObject.LoadAsync(jsonReader) |> Async.AwaitTask
      }

    let! targetData =
      async {
        use file = File.OpenText(".vscode/settings.json")
        use jsonReader = new JsonTextReader(file)
        return! JObject.LoadAsync(jsonReader) |> Async.AwaitTask
      }

    let mergeSettings =
      let setting = new JsonMergeSettings()
      setting.MergeArrayHandling <- MergeArrayHandling.Union
      setting

    sourceData.Merge(targetData, mergeSettings)

    printfn "Merging %s to .vscode/settings.json" source
    File.WriteAllText(".vscode/settings.json", sourceData.ToString())
  }
