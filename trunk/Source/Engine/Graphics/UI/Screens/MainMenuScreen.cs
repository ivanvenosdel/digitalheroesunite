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
            MenuEntry exitMenuEntry = new MenuEntry("Exit");

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
        /// When the user cancels the main menu, ask if they want to exit the sample.
        /// </summary>
        protected override void OnCancel(PlayerIndex playerIndex)
        {
         /*  //const string message = "Press Space to continue.";

            //MessageBoxScreen confirmExitMessageBox = new MessageBoxScreen(message);

            //confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

            //ScreenManager.AddScreen(confirmExitMessageBox, playerIndex);
        }


        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to exit" message box.
        /// </summary>
        void ConfirmExitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            EventManager.Instance.QueueEvent(new KillSwitchEvent());*/
            ScreenManager.Game.Exit();
        }


        #endregion
    }
}
