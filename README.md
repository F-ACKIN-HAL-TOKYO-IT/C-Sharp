# ~ C-SHARP REPO ~
#
# 使い方(⋈◍＞◡＜◍)。✧♡
- 優しい誰かが面倒な課題を終わらせ、gitに上げてくれます
- あなたはそれをダウンロードして使います
- あなたは本来するべき仕事に専念できるようになり、世界はより良くなります！

# .NET Windowsフォームアプリケーション
ソリューションをまるごとアップロードすると名前などの個人情報を世界中に晒してしまう恐れがあるので、コードのみをアップロードするようにしましょう。ただし、むしろ晒したいという方はこの限りではありません。

## .NET Windowsフォームアプリケーションのデザインとプログラム本体について
フォームアプリのプロジェクトを作成すると、以下のファイルが自動生成されます。
- Program.cs         → Form1を呼び出すコードが書かれているファイル
- From1.cs           → Form1の処理を書くファイル
- From1.Designer.cs  → Form1のデザインを書くファイル

### Program.cs
![screenshot 102](https://user-images.githubusercontent.com/45355489/49055388-bc8a3800-f23a-11e8-8b0d-3e91f9817175.png)

重要なのは
- line.7     : namespace WindowsFormsApp4
- line.19    : Application.Run(new Form1());
このProgramクラスのMainメソッドは同じ名前空間上（WindowsFormsApp4）にいるForm1クラスをインスタンス化し、Application.Run()メソッドに渡します。

### Form1.cs
![screenshot 103](https://user-images.githubusercontent.com/45355489/49055576-6d90d280-f23b-11e8-8c23-75e4251be3b9.png)

コードはファイルを右クリック、もしくはF7キーで表示することが出来ます。
こちらも同じ名前空間です。
- line.15    : public Form1()
- line.17    : InitializeComponent();
Form1クラスのコンストラクタがInitializeComponent()を呼び出しています。

### From1.Designer.cs
![screenshot 104](https://user-images.githubusercontent.com/45355489/49055818-5ef6eb00-f23c-11e8-9fd5-9da7c8a84c78.png)

- line.29    : private void InitializeComponent()

InitializeComponent()本体が書かれています。

#### ボタンを追加するとどうなるのか
button1という名前のボタンを追加してクリック処理も追加します。
![screenshot 105](https://user-images.githubusercontent.com/45355489/49055957-eb091280-f23c-11e8-9bc1-06f908b752b0.png)

Form1.cs
![screenshot 106](https://user-images.githubusercontent.com/45355489/49056026-2b689080-f23d-11e8-8476-516ef874d5a4.png)
ボタンが押された時の処理はForm1.csに追加されます。
- line.20    : private void button1_Click(object sender, EventArgs e)

From1.Designer.cs
![screenshot 107](https://user-images.githubusercontent.com/45355489/49056141-b9447b80-f23d-11e8-9c69-841d14fa5328.png)
こちらにもコードが追加されています。
ボタンオブジェクトに対して名前や位置、デザインなどが設定されています。
- line49     : this.Controls.Add(this.button1);
- line58     : private System.Windows.Forms.Button button1;
this.Controls.Add()フォームにオブジェクトを追加します。
フィールド変数を定義し、Form1.csから利用できるようにします。


## デコンパイルされたコードの使い方
.NET Windowsフォームアプリケーションのデザインとプログラム本体についてよく読んでください。
### MyInstallerのコードを分割するには
https://github.com/F-ACKIN-HAL-TOKYO-IT/C-Sharp/blob/master/MyInstaller/FormMain.cs
このサンプルコードは、FormMainクラスのコンストラクタでInitializeComponent()を実行せずに、同コード内にあるMyInitializeComponent()を呼び出しています。
- line.195   : private void MyInitializeComponent()
この状態ではVisualStudioはデザインをレンダリング出来ないので、デザインを変更したい場合はFromMain.Designer.csというファイルを作成するかForm1をリネームするなりして、そこに張り付ける必要があります。
さらに、コードの一番下にあるフィールド変数もFromMain.Designer.csに張り付ける必要があります。
## 名前の変更
ファイル名を変更した場合、必ず参照先（名前空間やクラス名、メソッド名）も更新するようにしてください。

# 谷まさるとの関連について
偶然にも一部名前が一致していますが、私たちは専門学校HAL-TOKYOとはなんの関係もありません。