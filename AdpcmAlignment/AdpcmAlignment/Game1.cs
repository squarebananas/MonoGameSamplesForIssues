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
        Cue cueUncompressed;
        Cue cueCompressed;

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
            new WaveBank(audioEngine, "Content/myWaveBankUncompressed.xwb");
            new WaveBank(audioEngine, "Content/myWaveBankCompressed.xwb");
            soundBank = new SoundBank(audioEngine, "Content/mySoundBank.xsb");
            cueUncompressed = soundBank.GetCue("sfxUncompressed");
            cueCompressed = soundBank.GetCue("sfxCompressed");
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.F1))
                cueUncompressed.Play();
            if (keyboardState.IsKeyDown(Keys.F2))
                cueCompressed.Play();

            audioEngine.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            Vector2 position = new Vector2(25, 25);
            spriteBatch.DrawString(font, "Adpcm Alignment", position, Color.Green);
            
            spriteBatch.DrawString(font, "Both the uncompressed and compressed versions of the sound effect should sound the same.", position += new Vector2(0, 35), Color.White);
            spriteBatch.DrawString(font, "However the compressed version is distorted (or crashes for XAudio) due to the incorrect block alignment.",
                position += new Vector2(0, 25), Color.White);
            spriteBatch.DrawString(font, "(Please note this issue would also effect compressed non Xact sounds, however these cannot currently be built",
                position += new Vector2(0, 25), Color.White);
            spriteBatch.DrawString(font, "due to the separate FFMPEG issue #5662).", position += new Vector2(0, 25), Color.White);

            spriteBatch.DrawString(font, "Controls:   F1 = Play Uncompressed Sound Effect,   F2 = Play Compressed Sound Effect", position += new Vector2(0, 50), Color.Green);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}