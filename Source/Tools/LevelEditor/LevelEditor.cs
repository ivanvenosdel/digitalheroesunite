#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using MonsterEscape;
using MonsterEscape.Graphics.Cameras;
using MonsterEscape.Graphics.UI;
using MonsterEscape.Logic.Actors;
using MonsterEscape.Logic.Input;
using MonsterEscape.Utility;
using MonsterEscape.Worlds;
#endregion

namespace MonsterEscape.LevelEditor
{
    /// <summary>
    /// Level Editor
    /// </summary>
    public class LevelEditor : Microsoft.Xna.Framework.Game
    {
        #region Fields
        private DeviceManager deviceManager;
        private SpriteBatch spriteBatch;
        #endregion

        #region Constructors
        public LevelEditor()
        {
            deviceManager = new DeviceManager(this);
            this.deviceManager.PreferredBackBufferWidth = 800;
            this.deviceManager.PreferredBackBufferHeight = 520;
            this.IsMouseVisible = true;
            this.IsFixedTimeStep = true;
        }
        #endregion

        #region Protected Methods
        protected override void Initialize()
        {
            base.Initialize();

            InputManager.Instance.Initialize();
            ActorFactory.Instance.Initialize();
            TerrainKey.Initialize();
            ActorKey.Initialize();

            UIManager.Instance.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            UIManager.Instance.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            InputManager.Instance.Update(gameTime);
            CurrentLevel.Instance.Update(gameTime);

            Camera.Instance.Update(gameTime);
            UIManager.Instance.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            CurrentLevel.Instance.Draw(gameTime);
            UIManager.Instance.Draw(gameTime);
            base.Draw(gameTime);
        }
        #endregion
    }
}
