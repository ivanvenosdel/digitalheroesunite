#region File Description
/* --------------------
 * CinematicScreen.cs
 * 
 * Displays scene after 'Start' is selected.
 * --------------------
 * */
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

namespace GameStateManagement
{
    /// <summary>
    /// First scene for the introduction.
    /// </summary>
    
    class CinematicScreen : GameScreen
    {
        #region Fields

        ContentManager content;
        Texture2D cinematicTexture[2];

        #endregion


        #region Initialization

        /// <summary>
        /// Constructor
        /// </summary>

        public CinematicScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        /// <summary>
        /// Load Content
        /// </summary>
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            cinematicTexture[0] = content.Load<Texture2D>(@"UI\cinematic1");
            cinematicTexture[1] = content.Load<Texture2D>(@"UI\cinematic2");

            GameplayScreen gameplayScreen = new GameplayScreen();
            ScreenManager.AddScreen(gameplayScreen, PlayerIndex.One);
        }

        /// <summary>
        /// Unloads graphics content for this screen.
        /// </summary>
        public override void UnloadContent()
        {
            content.Unload();
        }

        #endregion

        #region Handle Input
       /// <summary>
       /// End Cutscene
       /// </summary>
       /// <param name="input"></param>
        public override void HandleInput(InputState input)
            {

            if (input.IsSpace(PlayerIndex.One))
            {
                ExitScreen();
                ScreenManager.AddScreen(new MessageBoxScreen("Press Space to continue."), PlayerIndex.One);
            }
             }
        #endregion

        #region Update and Draw

        /// <summary>
        /// Updates the background screen. Unlike most screens, this should not
        /// transition off even if it has been covered by another screen: it is
        /// supposed to be covered, after all! This overload forces the
        /// coveredByOtherScreen parameter to false in order to stop the base
        /// Update method wanting to transition off.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);
        }

        /// <summary>
        /// Draws the background screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);

            spriteBatch.Begin();

            spriteBatch.Draw(cinematicTexture[i], fullscreen,
                             new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));

            spriteBatch.End();
        }

        #endregion

    }
}