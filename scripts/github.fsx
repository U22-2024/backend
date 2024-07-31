#r "nuget: Octokit, 13.0.1"

open Octokit
open System.Net.Http
open System.IO

let installRelease (owner: string, repo: string, assetName: string, installName: string) =
  let symbolicDir = Path.Combine(Directory.GetCurrentDirectory(), "Tools", "bin")
  let symbolicPath = Path.Combine(symbolicDir, installName)

  async {
    if File.Exists(symbolicPath) then
      printfn $"{installName} already installed"
      ()
    else
      if Directory.Exists(symbolicDir) |> not then
        Directory.CreateDirectory(symbolicDir) |> ignore

      let github = new GitHubClient(new ProductHeaderValue("GithubReleaseInstaller"))
      let! release = github.Repository.Release.GetLatest(owner, repo) |> Async.AwaitTask
      let asset = release.Assets |> Seq.find (fun asset -> asset.Name = assetName)

      let downloadDir =
        Path.Combine(Directory.GetCurrentDirectory(), "Tools", owner, repo, release.TagName)

      if Directory.Exists(downloadDir) |> not then
        Directory.CreateDirectory(downloadDir) |> ignore

      let downloadPath = Path.Combine(downloadDir, assetName)

      printfn "Downloading %s/%s/%s/%s" owner repo release.TagName assetName
      let client = new HttpClient()
      use! res = client.GetStreamAsync(asset.BrowserDownloadUrl) |> Async.AwaitTask

      use file =
        new FileStream(downloadPath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite)

      do! res.CopyToAsync(file) |> Async.AwaitTask
      printfn "Downloaded successfully"

      File.CreateSymbolicLink(symbolicPath, downloadPath) |> ignore
      printfn "Installed %s" installName
  }
