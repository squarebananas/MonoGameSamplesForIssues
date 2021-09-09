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

        AudioEngine audioEngine;
        SoundBank soundBank;
        Cue cue;

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

            audioEngine = new AudioEngine("Content/xact.xgs");
            new WaveBank(audioEngine, "Content/myWaveBank.xwb");
            soundBank = new SoundBank(audioEngine, "Content/mySoundBank.xsb");
            cue = soundBank.GetCue("music");
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.F1))
            {
                if (cue.IsPlaying == false)
                {
                    cue.Dispose();
                    cue = soundBank.GetCue("music");
                    cue.Play();
                }
            }
            if (keyboardState.IsKeyDown(Keys.F2))
                if (cue.IsStopped == false)
                    cue.Pause();
            if (keyboardState.IsKeyDown(Keys.F3))
                if (cue.IsStopped == false)
                    cue.Resume();
            if (keyboardState.IsKeyDown(Keys.F4))
                if ((cue.IsPlaying == true) || (cue.IsPaused == true))
                    cue.Stop(AudioStopOptions.Immediate);

            audioEngine.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            Vector2 position = new Vector2(25, 25);
            spriteBatch.DrawString(font, "Cue.IsPlaying Issue", position, Color.Green);

            spriteBatch.DrawString(font, "For XNA projects when a cue is paused both IsPlaying and IsPaused are set to true.", position += new Vector2(0, 35), Color.White);
            spriteBatch.DrawString(font, "Monogame currently doesn't match this behaviour and sets IsPlaying to false.", position += new Vector2(0, 25), Color.White);
            spriteBatch.DrawString(font, "This can cause audio issues when porting XNA games to Monogame.", position += new Vector2(0, 25), Color.White);
            spriteBatch.DrawString(font, "(Please note reusing pause after stop won't work on OpenAL unless #7372 is applied. This is a separate resolved issue).",
                position += new Vector2(0, 25), Color.White);

            spriteBatch.DrawString(font, "Controls:   F1 = Play Music,   F2 = Pause Music,   F3 = Resume Music,   F4 = Stop Music", position += new Vector2(0, 50), Color.Green);

            spriteBatch.DrawString(font, "cue.IsPlaying", position += new Vector2(0, 50), Color.White);
            spriteBatch.DrawString(font, cue.IsPlaying.ToString(), position + new Vector2(150, 0), Color.Yellow);

            spriteBatch.DrawString(font, "cue.IsPaused", position += new Vector2(0, 25), Color.White);
            spriteBatch.DrawString(font, cue.IsPaused.ToString(), position + new Vector2(150, 0), Color.Yellow);

            spriteBatch.DrawString(font, "cue.IsStopped", position += new Vector2(0, 25), Color.White);
            spriteBatch.DrawString(font, cue.IsStopped.ToString(), position + new Vector2(150, 0), Color.Yellow);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}