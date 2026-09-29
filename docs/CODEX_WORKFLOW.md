# Codex開発環境 v1

## 目的と役割分担

この環境は、小さな C# コンソールアプリを使い、人間が判断し、Codex が実装と検証を進める開発運用を練習するためのものです。

人間は Issue で目的・期待する動作・完了条件を定義し、PR をレビューし、変更内容と merge を承認します。Codex は Issue と既存実装を読み、branch 作成、実装、テスト、commit / push、PR 作成、CI の確認と修正を担当します。仕様の判断が必要な点は人間に確認し、合意済みの範囲内の実装方法は Codex が判断します。

本書は各仕組みの配置と使い方を説明します。行動ルールの正本は [AGENTS.md](../AGENTS.md)、具体的な作業手順の正本は [issue-to-pr Skill](../.codex/skills/issue-to-pr/SKILL.md)です。

## 各仕組みの責務

| 仕組み | ここに置く内容・役割 |
| --- | --- |
| [AGENTS.md](../AGENTS.md) | 作業全般に共通する方針。変更範囲、承認、検証、報告、レビュー対応の原則 |
| [Issue Form](../.github/ISSUE_TEMPLATE/feature.yml) | 個々の機能の仕様。概要・目的、期待する動作、境界条件、必要なテスト、完了条件 |
| [issue-to-pr Skill](../.codex/skills/issue-to-pr/SKILL.md) | Issue 実装依頼をレビュー可能な PR まで進める、特定タスク用の手順 |
| [PR template](../.github/pull_request_template.md) | 関連 Issue、Summary、Validation、CI、Notes を記録する形式 |
| [GitHub Actions CI](../.github/workflows/ci.yml) | 実行可能な build / test の定義。変更が既存の検証を通るか判定 |
| [Ruleset](https://github.com/mkunori/codex-agent-practice/rules/24104295) | GitHub 側で PR 経由の変更・必須チェック・ブランチ保護を強制 |
| Draft PR | 途中の作業を同じ PR で共有し、レビュー準備が整ったかを示す状態 |
| `Closes #...` | PR と解決する Issue を関連付け、既定ブランチへの merge 時に Issue を自動 close する記述 |

Issue Form は仕様入力の補助であり、実装手順ではありません。必要なテスト欄は任意で、空欄なら Codex が仕様から補います。それ以外の仕様欄は必須です。[config.yml](../.github/ISSUE_TEMPLATE/config.yml) で空の Issue 作成を無効にし、フォーム利用を促しています。

PR template はレビュー材料の形式を揃えます。記入しただけで検証が済むわけではなく、実際の結果と最新の CI run を記載します。

### GitHub 側のガードレール

2026-09-29 に確認した `Protect main` Ruleset は有効で、既定ブランチ（現在は main）に次を適用しています。

- Pull Request を必須とする。
- required status check として `build-and-test` を必須とする。
- force push とブランチ削除を禁止する。

承認レビューの必須人数は 0 人です。人間の最終承認と merge 指示は [AGENTS.md](../AGENTS.md) に基づく運用上の責務です。CI 成功だけで merge の許可にはなりません。squash merge はこのリポジトリの標準運用であり、Ruleset が唯一の方式として強制しているわけではありません。

設定は GitHub 側にあり、ファイルをコピーしても移植されません。変更時は実際の Ruleset と CI のチェック名の整合を確認します。

## 標準フロー

1. 人間が Feature Issue を作成し、仕様と完了条件を定義します。
2. `Issue #XX を実装してください` と Codex に依頼します。commit / push / PR 作成までの依頼範囲も明確にします。
3. Codex が Issue、AGENTS.md、既存コード・テストを確認し、main と origin/main の同期、作業ツリーを確認して作業ブランチを作成します。
4. Issue の範囲内で実装と必要な自動テストを追加・更新し、build / test と差分確認を行います。
5. commit / push し、PR template に沿って main 向け PR を作成します。本文に `Closes #XX` を記載します。
6. 最新 head commit の CI が完了するまで確認し、仕様と完了条件を満たしたレビュー可能な状態で停止します。
7. 人間がレビューします。指摘は原則として同じブランチ・PR で対応し、再検証と CI 確認を行います。
8. 人間が変更内容を承認し、merge を指示した後、PR の状態と最新 CI を再確認して squash merge します。
9. PR の MERGED、Issue の自動 close、main への push で起動した CI の成功を確認します。ローカル main を origin/main と同期し、不要になった作業ブランチをローカル・リモートから削除します。

`Closes #XX` は PR 作成時や CI 成功時に Issue を閉じるものではありません。解決対象ではなく参考として関連付けるだけなら、PR template の案内に沿って `Refs #XX` を使います。

## ローカル検証と CI

リポジトリルートで .NET 10 SDK を使います。

```sh
dotnet build
dotnet run --project Tests/CodexAgentPractice.Tests.csproj
```

テストは `dotnet test` 形式ではありません。独自のコンソールランナーが全テスト成功時に終了コード 0、失敗時に 1 を返します。テスト項目の詳細は [Tests/README.md](../Tests/README.md) に委ねます。

CI は main 向け PR の作成・更新・再オープンと main への push で動きます。`build-and-test` job が checkout、.NET 10 SDK のセットアップ後、次を実行します。

```sh
dotnet restore Tests/CodexAgentPractice.Tests.csproj
dotnet build Tests/CodexAgentPractice.Tests.csproj --configuration Release --no-restore
dotnet run --project Tests/CodexAgentPractice.Tests.csproj --configuration Release --no-build --no-restore
```

テストプロジェクトはアプリを参照しているため、この build で両方を検証します。実際の runner、Action、コマンドの定義は [ci.yml](../.github/workflows/ci.yml) を参照してください。

### CI が失敗した場合

最新 head commit の workflow run を特定し、失敗した job / step / log を読みます。ログ、PR の差分、Issue の仕様、既存テストを照合して原因を特定し、同じ branch / PR で最小修正します。ローカルで再テストし、commit / push 後の新しい run が完了するまで確認します。

テストの削除・無効化や、誤った実装に期待値を合わせることで CI を通さない原則は [AGENTS.md](../AGENTS.md) に従います。権限や外部要因で検証できない場合も、成功扱いせず理由と未確認事項を報告します。

## Draft PR を使う場合

大きめの作業では Draft PR の利用を依頼できます。最初の意味のある実装コミットを push した段階で Draft PR を作成し、残作業を本文に記載します。同じ PR で追加実装・テスト・修正を続け、完了条件と最新 CI の成功を確認して Ready for review に変更します。

流れは「実装途中 → Draft PR → 同じ PR で追加作業 → CI 成功 → Ready for review」です。Ready for review は人間のレビューを受けられる状態を示し、merge の承認とは別です。

## issue-to-pr Skill の使い方

この Skill は Issue からレビュー可能な PR までのワークフロー用です。常時の行動ルールは AGENTS.md に置き、Skill には Issue 読解から CI 確認までのタスク固有の順序を置いています。Skill の選択自体が commit / push などの許可を追加するわけではありません。

Codex に認識されている場合は、`$issue-to-pr を使って Issue #XX を実装してください` と明示できます。Issue 実装依頼と description が一致すると自動選択されることも期待できますが、依頼文だけから実際の利用を断定はできません。明示・自動選択の仕組みは [OpenAI の Skill ドキュメント](https://developers.openai.com/codex/skills/)を参照してください。

このリポジトリの保存先は `.codex/skills/issue-to-pr/SKILL.md` です。一方、上記公式ドキュメントの現在の repo Skill 探索先は `.agents/skills` です。使用する Codex 環境で Skill 一覧や読み込み状況を確認してください。認識されない場合は既存ファイルのパスを指定して読むよう依頼できますが、それを自動選択の確認とは扱いません。配置の変更は別途判断し、本書では現在の保存先を説明しています。

## 新しいリポジトリへ持ち込む場合

- 人間の仕様承認・レビュー・merge 指示と Codex の作業範囲を決め、共通ルールを AGENTS.md に絞って記載する。
- Issue Form と PR template を導入し、仕様入力と結果報告に必要な項目をプロジェクトに合わせる。
- issue-to-pr Skill の手順を調整し、利用する Codex の探索先へ配置して、明示利用・認識状況を確認する。
- SDK、build、テストのコマンドと成功・失敗の終了コードを確認する。このリポジトリ固有の .NET コマンドと独自テストランナーは、新しいプロジェクトに合わせて変更する。
- CI を用意し、PR と既定ブランチへの push で実際に build / test が実行されることを確認する。
- GitHub 側で Ruleset を設定する。対象ブランチ、PR 必須、実際の job 名に対応する required status check、force push・削除保護を確認する。
- Git / GitHub の認証・権限、既定ブランチ、merge 方式を確認し、小さな Issue で PR、CI、承認後の merge、Issue 自動 close、後片付けまで試す。

共通方針は AGENTS.md、タスク手順は Skill、記入項目はテンプレート、実行処理は CI、強制する保護は Ruleset に置きます。本書と README はそれらへの入口と責務の説明に留め、設定やルールを変更するときは各正本を更新します。
