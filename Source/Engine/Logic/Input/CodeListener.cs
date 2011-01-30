using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Engine.Logic.Audio;
using GameStateManagement;
using Engine.World;

namespace Engine.Logic.Input
{
    public sealed class CodeListener
    {
        private static readonly CodeListener instance = new CodeListener();
        private int position;
        private bool enabled;
        private Keys lastKey;
        private int countDownToDeath;
        private Texture2D aTexture;
        private Texture2D bTexture;
        private Texture2D downTexture;
        private Texture2D leftTexture;
        private Texture2D rightTexture;
        private Texture2D upTexture;
        private GameWorld.OnLevelEnd levelEndHandler;

        /// <summary>Singleton</summary>
        public static CodeListener Instance { get { return instance; } }

        public CodeListener()
        {
            aTexture = DeviceManager.Instance.Content.Load<Texture2D>("Ui/WinCode/A1");
            bTexture = DeviceManager.Instance.Content.Load<Texture2D>("Ui/WinCode/B1");
            downTexture = DeviceManager.Instance.Content.Load<Texture2D>("Ui/WinCode/down1");
            leftTexture = DeviceManager.Instance.Content.Load<Texture2D>("Ui/WinCode/left3");
            rightTexture = DeviceManager.Instance.Content.Load<Texture2D>("Ui/WinCode/right3");
            upTexture = DeviceManager.Instance.Content.Load<Texture2D>("Ui/WinCode/up3");
        }

        public void Initialize(GameWorld.OnLevelEnd lvlandler)
        {
            levelEndHandler = lvlandler;
            position = 0;
            countDownToDeath = 0;
            enabled = true;
            lastKey = Keys.None;
            InputManager.Instance.OnKeyEvent += new KeyEvent(this.OnKeyEvent);
        }

        public void Reset()
        {
            DeviceManager.Instance.Paused = false;
            enabled = false;
            InputManager.Instance.OnKeyEvent -= new KeyEvent(this.OnKeyEvent);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (countDownToDeath > 0)
            {
                countDownToDeath -= gameTime.ElapsedGameTime.Milliseconds;
                if (countDownToDeath <= 0)
                {
                    SoundManager.Instance.PlaySound("Sound/Blunt");

                    //Done
                    levelEndHandler(null, 4);
                }
            }

            Vector2 origin = new Vector2(350, -125);
            if (position >= 1)
                spriteBatch.Draw(this.upTexture, new Vector2(origin.X + 15, origin.Y), Color.White);
            if (position >= 2)
                spriteBatch.Draw(this.upTexture, new Vector2(origin.X + 95, origin.Y), Color.White);
            if (position >= 3)
                spriteBatch.Draw(this.downTexture, new Vector2(origin.X + 165, origin.Y - 10), Color.White);
            if (position >= 4)
                spriteBatch.Draw(this.downTexture, new Vector2(origin.X + 230, origin.Y - 10), Color.White);
            if (position >= 5)
                spriteBatch.Draw(this.leftTexture, new Vector2(origin.X + 295, origin.Y + 10), Color.White);
            if (position >= 6)
                spriteBatch.Draw(this.rightTexture, new Vector2(origin.X + 360, origin.Y + 10), Color.White);
            if (position >= 7)
                spriteBatch.Draw(this.leftTexture, new Vector2(origin.X + 440, origin.Y + 10), Color.White);
            if (position >= 8)
                spriteBatch.Draw(this.rightTexture, new Vector2(origin.X + 500, origin.Y + 10), Color.White);
            if (position >= 9)
                spriteBatch.Draw(this.bTexture, new Vector2(origin.X + 560, origin.Y - 32), Color.White);
            if (position >= 10)
                spriteBatch.Draw(this.aTexture, new Vector2(origin.X + 640, origin.Y - 40), Color.White);
        }

        /// <summary>Handles key events from InputManager</summary>
        /// <param name="keyboardState">Keyboard State</param>
        public void OnKeyEvent(KeyboardState keyboardState)
        {
            DeviceManager.Instance.Paused = true;

            if (enabled && position < 10)
            {
                if (keyboardState.IsKeyDown(Keys.Up))
                {
                    lastKey = Keys.Up;
                }
                else if (lastKey == Keys.Up)
                {
                    if (position == 0 || position == 1)
                        position++;
                    else
                        position = 1;//start over

                    lastKey = Keys.None;
                }

                if (keyboardState.IsKeyDown(Keys.Down))
                {
                    lastKey = Keys.Down;
                }
                else if (lastKey == Keys.Down)
                {
                    if (position == 2 || position == 3)
                        position++;
                    else
                        position = 0;

                    lastKey = Keys.None;
                }

                if (keyboardState.IsKeyDown(Keys.Left))
                {
                    lastKey = Keys.Left;
                }
                else if (lastKey == Keys.Left)
                {
                    if (position == 4 || position == 6)
                        position++;
                    else
                        position = 0;

                    lastKey = Keys.None;
                }

                if (keyboardState.IsKeyDown(Keys.Right))
                {
                    lastKey = Keys.Right;
                }
                else if (lastKey == Keys.Right)
                {
                    if (position == 5 || position == 7)
                        position++;
                    else
                        position = 0;

                    lastKey = Keys.None;
                }

                if (keyboardState.IsKeyDown(Keys.B))
                {
                    lastKey = Keys.B;
                }
                else if (lastKey == Keys.B)
                {
                    if (position == 8)
                        position++;
                    else
                        position = 0;

                    lastKey = Keys.None;
                }

                if (keyboardState.IsKeyDown(Keys.A))
                {
                    lastKey = Keys.A;
                }
                else if (lastKey == Keys.A)
                {
                    if (position == 9)
                    {
                        position++;

                        //Done
                        SoundManager.Instance.PlaySound("Sound/Bonus");
                        countDownToDeath = 2000;
                    }
                    else
                        position = 0;

                    lastKey = Keys.None;
                }
            }
        }
    }
}
