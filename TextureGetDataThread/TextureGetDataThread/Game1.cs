using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System.Threading;

namespace MonogameIssues
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        SpriteFont font;

        Thread thread;
        Texture2D texture;
        Color[] colorArray;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("font");
            texture = Content.Load<Texture2D>("MonoGame");
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (thread == null)
            {
                if (keyboardState.IsKeyDown(Keys.F1))
                    GetDataMethod();
                if (keyboardState.IsKeyDown(Keys.F2))
                {
                    thread = new Thread(GetDataMethod);
                    thread.Start();
                }
                if (keyboardState.IsKeyDown(Keys.F3))
                    colorArray = null;
            }
            else
            {
                if (thread.Join(0))
                    thread = null;
            }

            base.Update(gameTime);
        }

        public void GetDataMethod()
        {
            colorArray = new Color[texture.Width * texture.Height];
            texture.GetData<Color>(colorArray);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            Vector2 position = new Vector2(25, 25);
            spriteBatch.DrawString(font, "Texture2D.GetData From Thread", position, Color.Green);
            
            spriteBatch.DrawString(font, "Currently Texture2D.GetData can only be run from the UI thread for DesktopGL projects.", position += new Vector2(0, 35), Color.White);
            spriteBatch.DrawString(font, "WindowsDX / UWP projects allow Texture2D.GetData from other threads.", position += new Vector2(0, 25), Color.White);
            spriteBatch.DrawString(font, "This function could be added by using the BlockOnUIThread approach (already used for Texture construction, Buffer GetData, etc).",
                position += new Vector2(0, 25), Color.White);

            spriteBatch.DrawString(font, "Controls:   F1 = Run GetData From UI Thread,   F2 = Run GetData From Another Thread,   F3 = Empty Color Array",
                position += new Vector2(0, 50), Color.Green);

            spriteBatch.Draw(texture, position += new Vector2(0, 50), null, Color.White, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);

            spriteBatch.DrawString(font, "Color Array from GetData", position += new Vector2(0, 150), Color.White);
            spriteBatch.DrawString(font, (colorArray != null) ? "Length = " + colorArray.Length : "null", position + new Vector2(200, 0), Color.Yellow);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}