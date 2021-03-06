#region Using Statements
using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Engine.Graphics.Cameras;
using Engine.Graphics.UI;
using Engine.Logic.ClassComponents;
using Engine.Logic.Events;
using Engine.Logic.Logger;
using Engine.World;
using GameStateManagement;

#endregion

namespace Engine.Graphics
{
    /// <summary>
    /// Authors: James Kirk
    /// Creation: 7.25.2010
    /// Description: The Graphics Core
    /// </summary>
    public class GraphicsCore : DrawableGameComponent
    {
        #region Fields
        private SpriteBatch spriteBatch;
        private Game game;
        private ScreenManager screenManager;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>Constructor</summary>
        /// <param name="game">The XNA game object</param>
        public GraphicsCore(Game game)
            : base(game)
        {
            this.game = game;

            //Events
            EventManager.Instance.AddListener(new EventListener(HandleEvents), EventType.ACTOR_ADD);
            EventManager.Instance.AddListener(new EventListener(HandleEvents), EventType.ACTOR_REMOVE);
            EventManager.Instance.AddListener(new EventListener(HandleEvents), EventType.KILLSWITCH);
        }
        #endregion

        #region Protected Methods
        /// <summary>Loads Graphics content</summary>
        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(DeviceManager.Instance.GraphicsDevice);

#if DEBUG
            if (BoundingComponent.BoundingTexture == null)
            {
                //Set Data needs an array, so we do this stupid dance
                Color[] final = new Color[1];
                final[0] = Color.White;
                BoundingComponent.BoundingTexture = new Texture2D(DeviceManager.Instance.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
                BoundingComponent.BoundingTexture.SetData<Color>(final);
            }
#endif

            WorldTypes.LoadContent();
        }
        #endregion

        #region Public Methods
        /// <summary>Initializes Graphics Core</summary>
        public override void Initialize()
        {
            Camera.Instance.Initialize();

            screenManager = new ScreenManager(this.game);
            screenManager.DrawOrder = 2;

            this.game.Components.Add(screenManager);
            // Activate the first screens.
            screenManager.AddScreen(new BackgroundScreen(), null);
            screenManager.AddScreen(new MainMenuScreen(), null);

            base.Initialize();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        /// <summary>Updates Graphics and its components</summary>
        /// <param name="gameTime">The current update time</param>
        public override void Update(GameTime gameTime)
        {
            //Portions that shouldn't run when the game is paused
            if (!DeviceManager.Instance.Paused)
            {
                Camera.Instance.Update(gameTime);
            }

#if DEBUG
            if (Debug.RenderFPS)
                FramesPerSecond.Update(gameTime);
#endif
        }

        /// <summary>Draws game objects to the screen</summary>
        /// <param name="gameTime">The current update time</param>
        public override void Draw(GameTime gameTime)
        {
            PreDraw();

            DeviceManager.Instance.GraphicsDevice.Clear(Color.CornflowerBlue);

            GameWorld.Instance.Draw(gameTime);
#if DEBUG
            if (Debug.RenderFPS)
            {
                this.spriteBatch.Begin();
                this.spriteBatch.DrawString(this.screenManager.Font, string.Format("FPS: {0}", FramesPerSecond.FPS), new Vector2(5, 5), Color.White);
                if (GameWorld.Instance.Enabled && GameWorld.Instance.Hero.GetBounding() != null)
                 this.spriteBatch.DrawString(this.screenManager.Font, string.Format("LV: {0}", GameWorld.Instance.Hero.GetBounding().Fixture.Body.LinearVelocity.ToString()), new Vector2(5, 35), Color.White);
                this.spriteBatch.End();
            }
#endif
            PostDraw();
        }

        /// <summary>Performs pre-drawing setup</summary>
        public void PreDraw()
        {

        }

        /// <summary>Performs post-drawing setup</summary>
        public void PostDraw()
        {

        }
        #endregion

        #region Private Methods
        /// <summary>Graphics event handler</summary>
        /// <param name="evt">The event being passed in</param>
        private void HandleEvents(Event evt)
        {
            switch (evt.EventType)
            {
                case EventType.ACTOR_ADD:

                    break;

                case EventType.ACTOR_REMOVE:

                    break;

                case EventType.KILLSWITCH:
                    //Clear volatile Data
                    break;

                case EventType.UNKNOWN:
                default:
                    LogManager.Instance.Alert("Unknown Event Type", "Engine.Graphics.HandleEvent", 0);
                    break; ;
            }

        }
        #endregion
    }
}
