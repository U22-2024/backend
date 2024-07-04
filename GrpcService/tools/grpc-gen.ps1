$BIN = "./tools/bin"
$VERSION = "1.34.0"
$REPO = "https://github.com/U22-2024/proto.git"
$BUF = "https://github.com/bufbuild/buf/releases/download/v${VERSION}/buf-Windows-x86_64.exe"
$CMD = "${BIN}/buf.exe"

if (-not (Test-Path $CMD)) {
    New-Item -ItemType Directory -Path $BIN -Force
    Write-Output "Downloading buf..."
    Invoke-WebRequest -Uri $BUF -OutFile $CMD
    Write-Output "Downloaded buf"
}

Write-Output "Exporting proto..."
./tools/bin/buf.exe export $REPO -o .

Write-Output "Generating gRPC code..."
./tools/bin/buf.exe generate

# 変数を定義
$directoryPath = "Generated"  # 対象のディレクトリパスを指定

# 指定されたディレクトリ内のすべてのファイルを取得
Get-ChildItem -Path $directoryPath -Recurse -File | ForEach-Object {
    $file = $_.FullName
    # ファイルの内容を読み込む
    $content = Get-Content -Raw -Path $file

    # 改行コードを CRLF に変換
    $newContent = $content -replace "`r?`n", "`r`n"

    # 新しい内容をファイルに書き込む
    Set-Content -Path $file -Value $newContent -NoNewline
}

Write-Output "All files in the directory have been updated to use CRLF line endings."
