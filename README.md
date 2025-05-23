# Hunt Logic

## 概要
- 1人称視点のシューティングゲーム（FPS)です。プレイヤーは、時間経過で減少する体力と、敵からの攻撃で減少するHPの２つを管理しながら、できる限り長く生き延びるサバイバル要素も含めています。
- 登場キャラクターはプレイヤーの他に、パートナーのキツネ、見つけると逃げるウサギ、攻撃してくるハチ、こちらから攻撃したら反撃してくる熊、がいます。
- https://play.unity.com/en/games/adc3a287-e58a-4630-bb71-c21114e501e7/webgl-builds
  こちらのUnityPlayroomでプレイができます。
- https://unityroom.com/games/hiroakeen_fps_1
  こちらのUnityRoomでは生存時間によるランキングにも対応しています。
- https://hiroakeen.itch.io/hunt-logic
  こちらのitch.ioではWindowsダウンロード版も配信しています。

## 操作方法
- プレイヤーの移動：WASDキー　　視点の移動：マウス移動

- 弓を構える：右クリック長押し　　矢を放つ：弓を構えた状態で左クリック

- カメラ感度の調整：スペースキーでメニュー画面から設定

- タイトルシーン、ゲームオーバーシーンはマウスクリックでボタンを選択

- ゲーム終了：エスケープキー

## 工夫した点 
- エネミーがプレイヤーに自動で近づく実装を応用し、パートナーが案内してくれるナビゲーション機能へ改変した。
- 矢は重力をかけたかったためRaycastHitではなくInstantiateした。
- 弓をひく角度などリアルさが出やすいように調節、連射できない仕様やアニメーションを長めに設定した。
- それぞれの動物ごとに挙動を変化。熊のみBodyとHeadで与えるダメージ量が変化。
- マップを一定地点まで進むと反対側へ繋がり、無限に広がっているように見せることでマップ容量負荷を軽減。
- FogとCameraのClipping Planes Farを小さくし、視界をあえて狭めることで探索感を得られるようにした。
- ダメージによるマスク描画により緊迫感を上げた。
  
## 苦労した点
- 矢が正面へ飛ばない問題
  - Instantiate位置からCanvasをScreenSpace-Cameraに設定(プレイヤーの視点前方にカメラCanvasが表示される）
  - WorldToScreenPointやScreenPointToRayを使い、目の前にあるCameraCanvasのレティクルイメージ画像に向かって打つ方式へ。
  - 矢の方向確認はDebug.DrawRayで目視しながら調整した。
- プレイヤーの影（全身）とアニメーションが連動しているが、視界上の腕（腕のみ）とキャラクターの腕が被る問題。
  - キャラクターモデルを影のみ描画（Cast Shadow）で解決。
 
## プログラミング技術（機能・デザイン） 
- SOLIDの原則に基づいた設計を意識。
  - 各クラスを単一責任になるよう分離。
  - 各動物ごとの挙動はIStateを用いたSTATEパターンを導入し、継承した個別スクリプトに分割した。
  - シングルトンによりExit処理、どのシーンでもエスケープで終了可。
- VisualStudioの分析ツールを用いてコードメトリクスを計算、保守容易性など可視化しながら適宜スクリプトを調整した。
- DoTweenでUIアニメーションを作成。
- オクルージョンカリングをオンにして描画負荷を軽減。
- 動物ごとのオーディオを３Dで設定し、プレイヤーとの距離により音量が変化。
- terrainによる樹木の自動配置、影の描画。

## 制作経緯（開発スケジュール） 
- 初日：企画
  - 自分が好きなジャンル：FPS＋狩り
  - ミニゲームサイズ：登場キャラクター少
  - 初見でもわかりやすく：ナビゲーション
  - 繰り返し遊べる：サバイバル要素
  - ターゲットプラットフォーム：WebGL
  - Webにした理由：テストプレイが容易、配布しやすいなど、ポートフォリオに向いていると思ったから。
- 参考にしたゲーム（採用したこと）
  - Apex Legends（FPS、弓矢）
  - Minecraft（サバイバル要素）
  - Monster Hunter: World（おともキャラ、狩り）
  - ゼルダの伝説 時のオカリナ（ナビゲーション機能）
  - スターフォックス64（無限マップ、Fog）
  - CallOfDuty BlackOps（ダメージマスク描画、Head Bob設定）
- ２日目：基礎構成
  - プレイヤー、カメラ、ナビゲーションなどのスクリプト
  - アニメーション、アセットや素材集めなど
  - terrain制作
- ３日目：衝突判定、トリガー判定、ダメージ処理などの接続関係、矢の射出関係
- ４日目：オーディオ、テストプレイ、バグ修正
- ５日目：最終調整、UnityRoom（ランキング対応）やitch.ioに投稿
- ６日目以降：修正＆アップデートを継続

## 課題点
- プロファイラーを活用して負荷の高いところをさらに改善
- より疎結合なスクリプト構成を目指す
- オリジナルというより既存のアイデアを組み合わせたので、オリジナル要素を増やす
- オブジェクト指向やデザインパターンをより効率よく、拡張性を高く維持できるよう訓練していく
- よりリアルな射撃感を出せるよう位置などを調整する

## 今後対応したいこと
- 現状はあまり長時間プレイを想定していないため、もしInstantiate負荷が増えるならオブジェクトプールの採用を検討したい。
- ユーザーのコメント等があれば、要望やバグ報告など対応していきたい。
- アセット購入ではなく自作してみたい（３Dモデリング等）

## まとめ
- 人生初の自作ゲームをリリースまで出来た。
- ひととおりの流れを経験し、すべてを自分で実装する大変さや、スケジュール管理も想定外のことばかりだった。
- 今後はターゲットプラットフォームをスマホにも拡大したり、ゲームジャムに参加したり、スキル向上にも努めていきたい。
  
## 使用素材
- Game Weapon Icons Package
- Blood splatter decal package
- Ten Power-Ups
- Fantasy Bee
- White Rabbit
- FREE Stylized Bear - RPG Forest Animal
- LowPoly Survival Character Rio
- Toon Fox
- Arms&HandsHumanoidRig
- RPG_Animations - Bow
- Anime Natural Environment
- Simple Gems and Items Ultimate Animated Customizable Pack
- DOTween (HOTween v2)
- Sleek essential UI pack
- Free Meat and Skin Icons

## 参考図書
- Unity公式eBook
- 作って学べる　Unity本格入門　［Unity 6対応版］賀好 昭仁 著
- Unityの教科書 Unity 6完全対応版 北村愛実 著
- Unityゲーム プログラミング・バイブル 2nd Generation 著者：森 哲哉、布留川 英一、西森 丈俊、車谷 勇人、一條 貴彰、打田 恭平、轟 昂、室星 亮太、井本 大登、細田 翔、西岡 陽、平井 佑樹、コポコポ、すいみん、Maruton、karukaru、ハダシA、notargs、EIKI`、おれんじりりぃ、黒河 優介、中村 優一、藤岡 裕吾
- Unity 3Dゲーム開発ではじめるC#プログラミング Harrison Ferrone　著/吉川 邦夫　訳
- 新・標準プログラマーズライブラリ なるほどなっとく C#入門 出井秀行　著
- 確かな力が身につく C#「超」入門 第3版 北村愛実 著
- 独習C# 第5版 山田 祥寛 著
- Game Programming Patterns ソフトウェア開発の問題解決メニュー Robert Nystrom　著/武舎 広幸　監修/阿部 和也　訳/上西 昌弘　訳
- オブジェクト設計スタイルガイド　Matthias Noback 著/田中 裕一 訳
