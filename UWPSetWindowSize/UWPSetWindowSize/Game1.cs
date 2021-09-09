using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonogameIssues
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        SpriteFont font;

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
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.F1))
            {
                graphics.PreferredBackBufferWidth = 640;
                graphics.PreferredBackBufferHeight = 360;
                graphics.ApplyChanges();
            }
            if (keyboardState.IsKeyDown(Keys.F2))
            {
                graphics.PreferredBackBufferWidth = 1280;
                graphics.PreferredBackBufferHeight = 720;
                graphics.ApplyChanges();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            Vector2 position = new Vector2(25, 25);
            spriteBatch.DrawString(font, "UWP Set Window Size", position, Color.Green);

            spriteBatch.DrawString(font, "It should be possible to change the window size during Update by setting the preferred back buffer size and using ApplyChanges.",
                position += new Vector2(0, 35), Color.White);
            spriteBatch.DrawString(font, "However this results in a thread access error.", position += new Vector2(0, 25), Color.White);

            spriteBatch.DrawString(font, "Controls:   F1 = Set Window Size To 640 x 360,   F2 = Set Window Size To 1280 x 720", position += new Vector2(0, 50), Color.Green);
            position += new Vector2(0, 25);

            spriteBatch.DrawString(font, "Window Size", position += new Vector2(0, 50), Color.White);
            spriteBatch.DrawString(font, Window.ClientBounds.Width + " x " + Window.ClientBounds.Height, position + new Vector2(350, 0), Color.Yellow);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}