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
            : base("Main Menu")
        {
            // Create our menu entries.
            MenuEntry playGameMenuEntry = new MenuEntry("Start");
            MenuEntry creditsMenuEntry = new MenuEntry("Credits");
            MenuEntry exitMenuEntry = new MenuEntry("Exit");


            // Hook up menu event handlers.
            playGameMenuEntry.Selected += PlayGameMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;
            creditsMenuEntry.Selected += CreditsGameMenuEntrySelected;

            // Add entries to the menu.
            MenuEntries.Add(playGameMenuEntry);
            MenuEntries.Add(exitMenuEntry);
            MenuEntries.Add(creditsMenuEntry);

            SoundManager.Instance.LoadSong("Music/TitleScreen");
            SoundManager.Instance.PlaySong("Music/TitleScreen");
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

            ScreenManager.AddScreen(new CinematicScreen(), PlayerIndex.One);
           // LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
           //                    new GameplayScreen());
            //ScreenManager.AddScreen(new MessageBoxScreen("Press Space to continue."), PlayerIndex.One);

            SoundManager.Instance.UnloadContent();
        }


        /// <summary>
        /// Event Handler for when the credits option is selected
        /// </summary>
        void CreditsGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
       {
        ScreenManager.AddScreen (new CreditsScreen(), PlayerIndex.One);
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
