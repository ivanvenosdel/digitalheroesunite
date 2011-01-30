﻿#region File Description
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
using Engine.World;
using System.Collections.Generic;
using Engine.Logic.Audio;
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
        Texture2D Intro1;
        string cineTexture;
        int level;
        int timeCount = 0;
        Engine.World.GameWorld.OnLevelEnd levelEndHandler;

        #endregion


        #region Initialization

        /// <summary>
        /// Constructor
        /// </summary>
        /// 

  
        public CinematicScreen(string cineTexture)
            : base()
            {
            this.cineTexture = cineTexture;
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            this.levelEndHandler = null;
            SoundManager.Instance.LoadSong("Music/VirusTheme");
            SoundManager.Instance.PlaySong("Music/VirusTheme");

            }

        public CinematicScreen(string cineTexture, int level, Engine.World.GameWorld.OnLevelEnd levelEndHandler)
        {
            this.cineTexture = cineTexture;
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            this.level = level;
            this.levelEndHandler = levelEndHandler;
           
        }

        /// <summary>
        /// Load Content
        /// </summary>
        public override void LoadContent()
        {   
           
            if (content == null)
            { 
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            }

            string texturePath = String.Format(@"UI\Intro\{0}", this.cineTexture);
            Intro1 = content.Load<Texture2D>(texturePath);
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
           if (timeCount % 300 == 0)
            {
                if(this.levelEndHandler != null)
                {
                  GameWorld.Instance.Initialize(this.level, this.levelEndHandler);
                  /*SoundManager.Instance.StopSong();
                  SoundManager.Instance.UnloadContent();*/
                }
                
                ExitScreen();
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
            timeCount++;

            spriteBatch.Begin();

            spriteBatch.Draw(Intro1, fullscreen,
                             new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));

            spriteBatch.End();
        }

        #endregion

    }
}