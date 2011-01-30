#region File Description
//-----------------------------------------------------------------------------
// MainMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
using Engine.Logic.Events;
using Engine.Logic.Audio;
using Engine.World;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
#endregion

namespace GameStateManagement
{
    /// <summary>
    /// The main menu screen is the first thing displayed when the game starts up.
    /// </summary>
    class MainMenuScreen : MenuScreen
    {
       
       
        #region Initialization

        /// <summary>
        /// Constructor fills in the menu contents.
        /// </summary>
        public MainMenuScreen()
            : base("")
        {
            string start = "Start";
            string exit = "Exit";
            
            MenuEntry playGameMenuEntry = new MenuEntry(start);
            MenuEntry exitMenuEntry = new MenuEntry(exit);

            Start = new Vector2(0, 400);
           
            // Hook up menu event handlers.
            playGameMenuEntry.Selected += PlayGameMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(playGameMenuEntry);
            MenuEntries.Add(exitMenuEntry);

            SoundManager.Instance.LoadSong("Music/TitleScreen");
            SoundManager.Instance.PlaySong("Music/TitleScreen");
        }

      /*  public override void UnloadContent()
        {
            SoundManager.Instance.StopSong();
            base.UnloadContent();
        }*/
        #endregion

        #region Event Methods
        private void LevelEndHandler(GameWorld world, int level)
        {
            switch (level)
            {
                case 1:
                    //Load lvl 2
                    world.Initialize(level + 1, this.LevelEndHandler);
                    break;
                case 2:
                    //Load lvl 3
                    world.Initialize(level + 1, this.LevelEndHandler);
                    break;
                default:
                    break;
            }
        }
        #endregion


        #region Handle Input

        /// <summary>
        /// Event handler for when the Play Game menu entry is selected.
        /// </summary>
        void PlayGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            // Tell all the current screens to transition off.
            foreach (GameScreen screen in ScreenManager.GetScreens())
                screen.ExitScreen();

            GameplayScreen gameplayScreen = new GameplayScreen();
            ScreenManager.AddScreen(gameplayScreen, PlayerIndex.One);
            
            ScreenManager.AddScreen(new MessageBoxScreen("Press Space to continue."), PlayerIndex.One);
            CinematicScreen cinematic3 = new CinematicScreen("Intro3", 1, this.LevelEndHandler, "Platformer");
            CinematicScreen cinematic2 = new CinematicScreen("Intro2");
            CinematicScreen cinematic1 = new CinematicScreen("Intro1", "VirusTheme");

            ScreenManager.AddScreen(cinematic3, PlayerIndex.One);
            ScreenManager.AddScreen(cinematic2, PlayerIndex.One);
            ScreenManager.AddScreen(cinematic1, PlayerIndex.One);
        }

        #endregion
        
    }
}
