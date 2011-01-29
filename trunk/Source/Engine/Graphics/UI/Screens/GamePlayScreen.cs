#region Using Statements
using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Engine.Logic.Input;
using Engine.Logic.Events;
#endregion

namespace Engine.Graphics.UI.Screens
{
    /// <summary>
    /// Authors: James Kirk
    /// Creation: 12.13.2010
    /// Description: Game Play Screen
    /// </summary>
    public class GamePlayScreen : Screen
    {
        #region Fields
        public enum SubScreen
        {
            GAME = 0,
            PAUSE
        };

        private SubScreen subScreen;
        #endregion

        #region Properties
        /// <summary>Current Subscreen</summary>
        public SubScreen CurrentSubScreen { get { return this.subScreen; } set { this.subScreen = value; } }
        #endregion

        #region Constructor
        public GamePlayScreen()
        {
            this.subScreen = SubScreen.GAME;
        }
        
        public override void Initialize()
        {
            base.Initialize();
        }
        #endregion

        #region Public Methods
        public override void LoadContent()
        {
            //Load Textures
        }
        
        public override void Update(GameTime gameTime)
        {

        }

        /// <summary>
        /// Render Gameplay UI elements
        /// </summary>
        /// <param name="spriteBatch">A spriteBatch</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, float masterAlpha)
        {
            spriteBatch.Begin();

            Color alpha = new Color(masterAlpha, masterAlpha, masterAlpha, masterAlpha);

            //Constant GAME ui
            //spriteBatch.Draw(this.texture, new Vector2(x, y), new Rectangle(u, v, w, h), alpha);

            if (this.subScreen == SubScreen.PAUSE)
            {

            }
           
            spriteBatch.End();
        }

        /// <summary>Handles mouse events from InputManager</summary>
        /// <param name="buttons">List of mouse button states</param>
        /// <param name="mouseDelta">The distance moved by the mouse since last handled</param>
        /// <param name="scrollDelta">The distance scrolled by the scroll wheel since last handled</param>
        /// <param name="mousePos">The mouse cursor location on screen</param>
        public override void OnMouseEvent(MouseState current, MouseState previous, float scrollDelta)
        {

        }

        /// <summary>Handles key events from InputManager</summary>
        /// <param name="keyboardState">Keyboard State</param>
        public override void OnKeyEvent(KeyboardState keyboardState)
        {
            //Pause Toggle
            if (keyboardState.IsKeyDown(Keys.Pause))
            {
                lastKey = Keys.Pause;
            }
            else if (lastKey == Keys.Pause)
            {
                lastKey = Keys.None;

                if (this.subScreen == SubScreen.PAUSE)
                {
                    EventManager.Instance.QueueEvent(new UnpauseGameEvent());
                    this.subScreen = SubScreen.GAME;
                }
                else
                {
                    EventManager.Instance.QueueEvent(new PauseGameEvent());
                    this.subScreen = SubScreen.PAUSE;
                }
            }
        }
        #endregion

        #region Private Methods

        #endregion
    }
}
