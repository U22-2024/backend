#load "github.fsx"
#load "settings.fsx"

open Github
open System

// specificSettings ()

let bufAssetName =
  match System.Environment.OSVersion.Platform with
  | PlatformID.Win32NT -> "buf-Windows-x86_64.exe"
  | PlatformID.Unix -> "buf-Linux-x86_64"
  | _ -> failwith "Unsupported platform"

let bufInstallName =
  match System.Environment.OSVersion.Platform with
  | PlatformID.Win32NT -> "buf.exe"
  | PlatformID.Unix -> "buf"
  | _ -> failwith "Unsupported platform"

Async.RunSynchronously(installRelease ("bufbuild", "buf", bufAssetName, bufInstallName))
