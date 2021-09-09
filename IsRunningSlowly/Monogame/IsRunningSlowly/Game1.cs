using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Microsoft.Xna.Framework.Audio;

namespace MonogameIssues
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        SpriteFont font;

        int targetFrameRate;
        double lastDrawTime;
        double? simulateLagEndTime;
        double? runningSmoothlyTime;
        int? runningSmoothlyFrameCount;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            targetFrameRate = 60;

            IsFixedTimeStep = true;
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
                System.Threading.Tasks.Task.Delay(50).Wait();
                simulateLagEndTime = runningSmoothlyTime = runningSmoothlyFrameCount = null;
            }
            else
            {
                if (simulateLagEndTime == null)
                    simulateLagEndTime = gameTime.TotalGameTime.TotalSeconds;
                if ((runningSmoothlyTime == null) && (gameTime.IsRunningSlowly == false))
                    runningSmoothlyTime = gameTime.TotalGameTime.TotalSeconds;
                if ((simulateLagEndTime != null) && (runningSmoothlyTime == null))
                    if (runningSmoothlyFrameCount != null)
                        runningSmoothlyFrameCount++;
                    else
                        runningSmoothlyFrameCount = 0;
            }

            if (keyboardState.IsKeyDown(Keys.F2))
                targetFrameRate = 60;

            if (keyboardState.IsKeyDown(Keys.F3))
                targetFrameRate = 30;
            
            TargetElapsedTime = System.TimeSpan.FromSeconds(1d / targetFrameRate);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            Vector2 position = new Vector2(25, 25);
            spriteBatch.DrawString(font, "GameTime.IsRunningSlowly Reset Condition", position, Color.Green);

            spriteBatch.DrawString(font, "GameTime.IsRunningSlowly should be true when frames are being dropped and reset to false when the target framerate is resumed.",
                position += new Vector2(0, 35), Color.White);
            spriteBatch.DrawString(font, "However the reset condition seems to be implemented incorrectly and can take an excessive amount of time to reset.",
                position += new Vector2(0, 25), Color.White);
            spriteBatch.DrawString(font, "It seems the time taken to reset is connected to how long the game was lagging for.", position += new Vector2(0, 25), Color.White);

            spriteBatch.DrawString(font, "Controls:   Hold F1 = Simulate Lag,   F2 = 60 Target FPS,   F3 = Target 30 FPS", position += new Vector2(0, 50), Color.Green);

            spriteBatch.DrawString(font, "Target Frame Rate", position += new Vector2(0, 50), Color.White);
            spriteBatch.DrawString(font, targetFrameRate.ToString(), position + new Vector2(250, 0), Color.Yellow);

            spriteBatch.DrawString(font, "FPS", position += new Vector2(0, 50), Color.White);
            spriteBatch.DrawString(font, ((int)(1d / (gameTime.TotalGameTime.TotalSeconds - lastDrawTime))).ToString(), position + new Vector2(250, 0), Color.Yellow);
            lastDrawTime = gameTime.TotalGameTime.TotalSeconds;

            spriteBatch.DrawString(font, "gameTime.IsRunningSlowly", position += new Vector2(0, 50), Color.White);
            spriteBatch.DrawString(font, gameTime.IsRunningSlowly.ToString(), position + new Vector2(250, 0), Color.Yellow);

            spriteBatch.DrawString(font, "Time taken to reset IsRunningSlowly", position += new Vector2(0, 50), Color.White);
            if (simulateLagEndTime != null)
                spriteBatch.DrawString(font, (((runningSmoothlyTime != null) ? runningSmoothlyTime : gameTime.TotalGameTime.TotalSeconds) - simulateLagEndTime).ToString() + " Second(s)",
                    position + new Vector2(250, 0), Color.Yellow);

            spriteBatch.DrawString(font, "Frames taken to reset IsRunningSlowly", position += new Vector2(0, 50), Color.White);
            if (runningSmoothlyFrameCount != null)
                spriteBatch.DrawString(font, runningSmoothlyFrameCount.ToString() + " Frames", position + new Vector2(250, 0), Color.Yellow);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}