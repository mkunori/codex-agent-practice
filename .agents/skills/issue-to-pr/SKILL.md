---
name: issue-to-pr
description: GitHub Issueを実装し、検証・commit・push・Pull Request作成・CI確認まで進め、レビュー可能なPRに仕上げるときに使う。Issueの調査だけやmerge作業には使わない。
---

# Issue to PR

このリポジトリで、指定されたIssueをレビュー可能なPRまで進める。共通の開発・承認・報告ルールはルートの `AGENTS.md` に従い、このSkillでは重複定義しない。ユーザーが指定した作業範囲と既存の許可を引き継ぎ、Skillの選択だけでcommit・push・PR作成の許可を追加しない。

## ワークフロー

1. GitHub CLIで対象リポジトリとIssue番号を確認し、本文・コメントから期待動作、境界条件、完了条件を読む。`AGENTS.md`、関連実装・テスト、`.github/workflows/ci.yml`、`.github/pull_request_template.md` を確認する。仕様上の不足が実装判断を妨げる場合だけ確認し、範囲内の実装方法は自分で判断する。
2. 作業ツリーとブランチ、remoteを確認し、最新の `origin/main` と同期したmainから内容に合う作業ブランチを作る。継続依頼で対象ブランチ・PRが既にある場合はそれを使う。
3. Issueの範囲で実装し、完了条件に対応する自動テストを追加・更新する。CLIの正常系・異常系では、出力内容だけでなく標準出力／標準エラーと終了コードも検証する。
4. リポジトリルートで `dotnet build` と `dotnet run --project Tests/CodexAgentPractice.Tests.csproj` を実行する。テストはコンソール形式なので `dotnet test` で代用しない。必要に応じて実アプリでも確認し、差分をレビューして依頼外の変更がないことを確かめる。
5. 許可された範囲で変更をcommitし、作業ブランチをpushしてupstreamを設定する。既存PRの有無を確認し、新規ならmain向けに作成する。本文はPRテンプレートに沿い、解決するIssueを `Closes #番号` で関連付け、実際の検証結果を記載する。
6. Draft運用が指定された場合は、最初の意味のある実装コミットでDraft PRを作成し、残作業を明示する。その後の実装・テストも同じPRで続ける。
7. 最新head commitに対応するActions runの完了と必須チェック `build-and-test` を確認する。失敗したjob・step・ログを読み、Issueの仕様と差分を照合して原因を特定し、同じブランチ・PRで最小修正、再検証、追加commit・pushを行う。権限や外部サービスの問題、仕様判断が必要な状態では、無変更の再実行を繰り返さず、確認できた事実と必要な対応を報告する。
8. 全完了条件と最新CIの成功を確認し、PR本文のCI欄を結果とrunリンクで更新する。指定されたDraftはReady for reviewへ変更し、状態を確認する。Issue番号、実装概要、検証結果、PR URL、CI結果、残る制約を報告して停止する。このワークフローではmergeしない。
