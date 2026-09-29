# Codex Agent Practice

小さな C#/.NET コンソールアプリを題材に、Codex によるエージェント開発を練習するリポジトリです。人間が仕様と最終レビューを担当し、Codex が実装から検証・Pull Request（PR）対応まで進める「Codex開発環境 v1」を構成しています。

## Build / Test

.NET 10 SDK を用意し、リポジトリルートで実行します。

```sh
dotnet build
dotnet run --project Tests/CodexAgentPractice.Tests.csproj
```

自動テストはコンソール形式の独自ランナーです。`dotnet test` ではなく、上記のプロジェクトを実行します。詳しくは [テストの説明](Tests/README.md)を参照してください。

## 日常の開発

Feature Issue に仕様と完了条件を書き、Codex に `Issue #XX を実装し、レビュー可能なPRまで進めてください` と依頼します。このように commit / push / PR 作成までを依頼範囲に含めた場合、Codex が作業ブランチで実装・検証し、commit / push、PR 作成、CI 確認まで進めます。人間がレビューし、変更内容を承認して merge を指示した後に squash merge、Issue の自動 close、ブランチの後片付けを行います。

## 仕組みの入口

| 仕組み | 役割 |
| --- | --- |
| [AGENTS.md](AGENTS.md) | Codex が守る共通の開発・承認・検証ルール |
| [Issue Form](.github/ISSUE_TEMPLATE/feature.yml) | 実装可能な仕様と完了条件を入力する入口 |
| [issue-to-pr Skill](.codex/skills/issue-to-pr/SKILL.md) | Issue からレビュー可能な PR までの再利用可能な手順 |
| [PR template](.github/pull_request_template.md) | 変更内容・検証結果・関連 Issue をレビュー相手へ伝える形式 |
| [GitHub Actions CI](.github/workflows/ci.yml) | main 向け PR と main への push の build / test |
| [Ruleset](https://github.com/mkunori/codex-agent-practice/rules/24104295) | PR・必須チェック・main 保護を GitHub 側で強制する設定 |

責務の分け方、CI 失敗時の対応、Draft PR、Skill の利用・認識確認、新しいリポジトリへの導入方法は [Codex開発ワークフロー](docs/CODEX_WORKFLOW.md)を参照してください。
