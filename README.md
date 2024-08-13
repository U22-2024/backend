# Backend

U22-2024プロジェクトのバックエンドリポジトリ

## 開発環境

Riderを想定して環境を作ってますが、VSCode用の設定も一応用意しています。
もし、動かなかったら言ってください。

### Rider

Riderで開くと、Required Pluginsが表示されるので、それをインストールしてください。

### VSCode

VSCodeで開くと、Recommened Extensionsが表示されるので、それをインストールしてください。

## 初めに実行するコマンド

```shell
dotnet tool restore
dotnet workload install aspire
dotnet restore
```

## プロジェクトの実行

![img.png](Docs/imgs/rider_run_config.png)

Riderの右上を見ると、このような表示があるので、それをクリックしてください。
すると、下のような画面が出てくるので、三角に斜線がついているボタンをクリックして、実行ボタン（緑色の三角形）をおしてください。

![run_configs.png](Docs/imgs/run_configs.png)

## 予想されるエラーと対処方法

###
