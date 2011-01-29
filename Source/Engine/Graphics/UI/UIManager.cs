#region Using Statements
using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Engine.Graphics.Cameras;
using Engine.Graphics.UI.Screens;
using Engine.Logic.Input;
#endregion

namespace Engine.Graphics.UI
{
    #region Enums
    /// <summary>Font Types</summary>
    public enum FontType
    {
        DEFAULT = 0
    };

    /// <summary>Possible UI Screens</summary>
    public enum ScreenType
    {
        GAMEPLAY
    };
    #endregion

    /// <summary>
    /// Authors: James Kirk
    /// Creation: 11.27.2010
    /// Description: User Interface Manager
    /// </summary>
    public class UIManager
    {
        #region Fields
        private const int SHIFT_TRANSITION = 500;

        private static readonly UIManager instance = new UIManager();

        private SpriteBatch spriteBatch;
        private List<SpriteFont> fonts = new List<SpriteFont>();
        private ScreenType currentScreenType;
        private Screen currentScreen;
        private Screen lastScreen;
        private int timer;
        #endregion

        #region Properties
        /// <summary>Singelton</summary>
        public static UIManager Instance { get { return instance; } }
        /// <summary>The Current UI Screen</summary>
        public Screen CurrentScreen { get { return this.currentScreen; } }
        /// <summary>Obtains a font</summary>
        public SpriteFont getFont(FontType fnt) { return this.fonts[(int)fnt]; }
        #endregion

        #region Constructors
        protected UIManager()
        {
            this.lastScreen = null;
            this.timer = 0;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Initialize the UIManager
        /// </summary>
        public void Initialize()
        {
            ChangeScreen(ScreenType.GAMEPLAY);

            InputManager.Instance.OnMouseEvent += new MouseEvent(this.OnMouseEvent);
            InputManager.Instance.OnKeyEvent += new KeyEvent(this.OnKeyEvent);
        }

        /// <summary>
        /// Load all cross screen content such as fonts
        /// </summary>
        public void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(DeviceManager.Instance.GraphicsDevice);
            this.fonts.Add(DeviceManager.Instance.Content.Load<SpriteFont>(@"UI/Fonts/debugfont")); //FontType.DEFAULT
        }

        /// <summary>
        /// Unloads all content
        /// </summary>
        public void UnloadContent()
        {
            //Dump the 'lastscreen' if it still exists
            if (this.lastScreen != null)
            {
                this.lastScreen.UnloadContent();
            }

            //Dump currentscreen
            if (this.currentScreen != null)
            {
                this.currentScreen.UnloadContent();
            }
        }

        /// <summary>
        /// Update the current UI
        /// </summary>
        /// <param name="gameTime">Current game time</param>
        public void Update(GameTime gameTime)
        {
            if (this.currentScreen != null)
                this.currentScreen.Update(gameTime);

            //Update Transition timer
            if (this.timer > 0)
            {
                this.timer -= gameTime.ElapsedGameTime.Milliseconds;
                if (this.timer <= 0)
                {
                    this.timer = 0;
                    if (this.lastScreen != null)
                    {
                        this.lastScreen.UnloadContent();
                        this.lastScreen = null;
                    }
                }
            }

#if DEBUG
            if (Debug.RenderFPS)
                FramesPerSecond.Update(gameTime);
#endif
        }
        
        /// <summary>
        /// Render the current UI onto the screen
        /// </summary>
        /// <param name="gameTime">Current game time</param>
        public void Draw(GameTime gameTime)
        {
            //Draw the fading out lastscreen if it exists
            if (this.lastScreen != null)
                this.lastScreen.Draw(gameTime, this.spriteBatch, this.timer / ((float)SHIFT_TRANSITION * 2.0f));

            //Draw the fading in, or solid currentscreen if it exists
            if (this.currentScreen != null)
                this.currentScreen.Draw(gameTime, this.spriteBatch, (SHIFT_TRANSITION - this.timer) / ((float)SHIFT_TRANSITION / 2.0f));
#if DEBUG
            if (Debug.RenderFPS)
            {
                SpriteFont font = getFont(FontType.DEFAULT);
                this.spriteBatch.Begin();
                this.spriteBatch.DrawString(font, string.Format("FPS: {0}", FramesPerSecond.FPS), new Vector2(5, 5), Color.White);
                this.spriteBatch.End();
            }
#endif
        }

        /// <summary>
        /// Updates to the new UI
        /// Cleans up any existing one
        /// </summary>
        /// <param name="type">The screen type (scene)</param>
        public void ChangeScreen(ScreenType type)
        {
            this.currentScreenType = type;

            //Remove the 'lastscreen' if it still exists
            //because we have a new one to take it's place
            if (this.lastScreen != null)
            {
                this.lastScreen.UnloadContent();
                this.lastScreen = null;
            }

            this.lastScreen = this.currentScreen;
            this.timer = SHIFT_TRANSITION;

            switch (type)
            {
                case ScreenType.GAMEPLAY:
                    this.currentScreen = new GamePlayScreen();
                break;
            };

            this.currentScreen.Initialize();
            this.currentScreen.LoadContent();
        }

        /// <summary>Handles mouse events from InputManager</summary>
        /// <param name="buttons">List of mouse button states</param>
        /// <param name="mouseDelta">The distance moved by the mouse since last handled</param>
        /// <param name="scrollDelta">The distance scrolled by the scroll wheel since last handled</param>
        /// <param name="mousePos">The mouse cursor location on screen</param>
        public void OnMouseEvent(MouseState current, MouseState previous, float scrollDelta)
        {
            if (currentScreen != null)
                currentScreen.OnMouseEvent(current, previous, scrollDelta);
        }

        /// <summary>Handles key events from InputManager</summary>
        /// <param name="keyboardState">Keyboard State</param>
        public void OnKeyEvent(KeyboardState keyboardState)
        {
            if (currentScreen != null)
                currentScreen.OnKeyEvent(keyboardState);
        }
        #endregion
    }
}
