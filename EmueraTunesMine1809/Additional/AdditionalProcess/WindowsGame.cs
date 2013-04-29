using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WindowsGame
{
    /// <summary>
    /// 基底 Game クラスから派生した、ゲームのメイン クラスです。
    /// </summary>
    public class WinGame : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics = null;
        private SpriteBatch spriteBatch=null;
        private Texture2D texture = null;

        public WinGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// ゲームが実行を開始する前に必要な初期化を行います。
        /// ここで、必要なサービスを照会して、関連するグラフィック以外のコンテンツを
        /// 読み込むことができます。base.Initialize を呼び出すと、使用するすべての
        /// コンポーネントが列挙されるとともに、初期化されます。
        /// </summary>
        protected override void Initialize()
        {
            // TODO: ここに初期化ロジックを追加します。

            base.Initialize();
        }

        /// <summary>
        /// LoadContent はゲームごとに 1 回呼び出され、ここですべてのコンテンツを
        /// 読み込みます。
        /// </summary>
        protected override void LoadContent()
        {
            // 新規の SpriteBatch を作成します。これはテクスチャーの描画に使用できます。
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
            this.texture = this.Content.Load<Texture2D>("Harukakka");
            // TODO: this.Content クラスを使用して、ゲームのコンテンツを読み込みます。
        }

        /// <summary>
        /// UnloadContent はゲームごとに 1 回呼び出され、ここですべてのコンテンツを
        /// アンロードします。
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: ここで ContentManager 以外のすべてのコンテンツをアンロードします。
        }

        /// <summary>
        /// ワールドの更新、衝突判定、入力値の取得、オーディオの再生などの
        /// ゲーム ロジックを、実行します。
        /// </summary>
        /// <param name="gameTime">ゲームの瞬間的なタイミング情報</param>
        protected override void Update(GameTime gameTime)
        {
            // ゲームの終了条件をチェックします。
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: ここにゲームのアップデート ロジックを追加します。

            base.Update(gameTime);
        }

        /// <summary>
        /// ゲームが自身を描画するためのメソッドです。
        /// </summary>
        /// <param name="gameTime">ゲームの瞬間的なタイミング情報</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: ここに描画コードを追加します。

            base.Draw(gameTime);
        }
        public void GameDraw() 
        {
            //graphics = new GraphicsDeviceManager(this);
            GraphicsDevice.Clear(Color.White);
            Content.RootDirectory = "Content";

            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            // テクスチャーを描画するためのスプライトバッチクラスを作成します
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);

            // テクスチャーをコンテンツパイプラインから読み込む
            this.texture = this.Content.Load<Texture2D>("Texture");

            // スプライトの描画準備
            this.spriteBatch.Begin();

            // スプライトを描画する
            this.spriteBatch.Draw(this.texture, Vector2.Zero, Color.White);

            // スプライトの一括描画
            this.spriteBatch.End();

            //base.Draw(gameTime);
        }
    }
}
