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

            exitMenuEntry.Position = new Vector2(400, 500);
            // Hook up menu event handlers.
            playGameMenuEntry.Selected += PlayGameMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(playGameMenuEntry);
            MenuEntries.Add(exitMenuEntry);

            SoundManager.Instance.LoadSong("Music/TitleScreen");
            SoundManager.Instance.PlaySong("Music/TitleScreen");
        }


        #endregion

        #region Event Methods
        private void LevelEndHandler(GameWorld world, int level)
        {
            level++;
            //TODO: Destroy World

            this.ScreenManager.AddScreen(new CinematicScreen(level, LevelEndHandler), PlayerIndex.One);
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

            ScreenManager.AddScreen(new CinematicScreen(1, this.LevelEndHandler), PlayerIndex.One);
           // LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
           //                    new GameplayScreen());
            //ScreenManager.AddScreen(new MessageBoxScreen("Press Space to continue."), PlayerIndex.One);

            SoundManager.Instance.UnloadContent();
        }




        /// <summary>
        /// When the user cancels the main menu, ask if they want to exit the sample.
        /// </summary>
        protected override void OnCancel(PlayerIndex playerIndex)
        {
         
            ScreenManager.Game.Exit();
        }


        #endregion
    }
}
