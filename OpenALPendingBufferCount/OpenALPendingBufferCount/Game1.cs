using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Microsoft.Xna.Framework.Audio;
using System;

namespace MonogameIssues
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        SpriteFont font;

        DynamicSoundEffectInstance dynamicSoundEffect;
        byte[] soundData;
        int streamPosition;
        int pendingBufferCountAtSubmitBuffer;

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

            System.IO.Stream stream = System.IO.File.OpenRead("Content/music.wav");
            soundData = new byte[stream.Length];
            stream.Read(soundData);

            dynamicSoundEffect = new DynamicSoundEffectInstance(44100, AudioChannels.Stereo);            
            dynamicSoundEffect.BufferNeeded += (s, e) =>
            {
                if (streamPosition < soundData.Length)
                {
                    pendingBufferCountAtSubmitBuffer = dynamicSoundEffect.PendingBufferCount;
                    int byteLengthToSubmit = Math.Min(65536, soundData.Length - streamPosition);
                    dynamicSoundEffect.SubmitBuffer(soundData, streamPosition, byteLengthToSubmit);
                    streamPosition += byteLengthToSubmit;
                }
            };
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.F1))
                dynamicSoundEffect.Play();

            if (keyboardState.IsKeyDown(Keys.F2))
            {
                dynamicSoundEffect.Stop();
                streamPosition = 0;
                pendingBufferCountAtSubmitBuffer = 0;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            Vector2 position = new Vector2(25, 25);
            spriteBatch.DrawString(font, "OpenAL Streaming Pending Buffer Count", position, Color.Green);
            
            spriteBatch.DrawString(font, "The TargetPendingBufferCount is hardcoded to 3 and the PendingBufferCount should generally match this.",
                position += new Vector2(0, 35), Color.White);
            spriteBatch.DrawString(font, "However the PendingBufferCount is never seen to be above 1.", position += new Vector2(0, 25), Color.White);
            spriteBatch.DrawString(font, "If the pendingBufferCountAtSubmitBuffer is 0 then all buffers were played before more data was submitted.",
                position += new Vector2(0, 25), Color.White);
            spriteBatch.DrawString(font, "This results in a choppy audio glitch.", position += new Vector2(0, 25), Color.White);

            spriteBatch.DrawString(font, "Controls:   F1 = Start Stream,   F2 = Stop Stream", position += new Vector2(0, 50), Color.Green);

            spriteBatch.DrawString(font, "dynamicSoundEffect.PendingBufferCount", position += new Vector2(0, 50), Color.White);
            spriteBatch.DrawString(font, dynamicSoundEffect.PendingBufferCount.ToString(), position + new Vector2(350, 0), Color.Yellow);

            spriteBatch.DrawString(font, "pendingBufferCountAtSubmitBuffer", position += new Vector2(0, 50), Color.White);
            spriteBatch.DrawString(font, pendingBufferCountAtSubmitBuffer.ToString(), position + new Vector2(350, 0), Color.Yellow);

            spriteBatch.DrawString(font, "Stream Position", position += new Vector2(0, 50), Color.White);
            spriteBatch.DrawString(font, streamPosition.ToString() + "  /  " + soundData.Length + " Bytes", position + new Vector2(350, 0), Color.Yellow);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}