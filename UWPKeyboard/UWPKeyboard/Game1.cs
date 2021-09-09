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

        Keys[] keys;
        bool gamePadConnected;

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
            keys = Keyboard.GetState().GetPressedKeys();

            gamePadConnected = GamePad.GetState(PlayerIndex.One).IsConnected;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            Vector2 position = new Vector2(25, 25);
            spriteBatch.DrawString(font, "UWP Keyboard Issues", position, Color.Green);

            spriteBatch.DrawString(font, "1) Ctrl, Shift and Alt do not work correctly (especially if Right Alt is pressed). ",
                position += new Vector2(0, 35), Color.White);
            spriteBatch.DrawString(font, "Sometimes these keys are not registered as pressed and other times they are latched as pressed while released.",
                position += new Vector2(0, 25), Color.White);
            spriteBatch.DrawString(font, "2) When using a Xbox gamepad the buttons are registered as key presses but most do not have enum values assigned.",
                position += new Vector2(0, 35), Color.White);
            spriteBatch.DrawString(font, "3) When using a Xbox gamepad pressing Dpad Up results in the ChatpadOrange key being incorrectly registered as pressed.",
                position += new Vector2(0, 35), Color.White);
            spriteBatch.DrawString(font, "4) When using a Xbox gamepad pressing RT results in the ChatpadGreen key being incorrectly registered as pressed.",
                position += new Vector2(0, 35), Color.White);

            spriteBatch.DrawString(font, "Controls:   Press any keyboard key / gamepad button", position += new Vector2(0, 50), Color.Green);
            position += new Vector2(0, 25);

            spriteBatch.DrawString(font, "GamePad.IsConnected", position += new Vector2(0, 25), Color.White);
            spriteBatch.DrawString(font, gamePadConnected.ToString(), position + new Vector2(150, 0), Color.Yellow);
            position += new Vector2(0, 25);

            for (int i = 0; i < keys.Length; i++)
                spriteBatch.DrawString(font, keys[i].ToString(), position += new Vector2(0, 25), Color.Yellow);
            if (keys.Length == 0)
                spriteBatch.DrawString(font, "No keys pressed", position += new Vector2(0, 25), Color.Yellow);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}