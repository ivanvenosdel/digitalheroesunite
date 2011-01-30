#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using Engine.Logic.Audio;
#endregion

namespace Engine
{
    /// <summary>
    /// Authors: James Kirk
    /// Creation: 1.23.2011
    /// Description: The game engine
    /// </summary>
    public class Base : Microsoft.Xna.Framework.Game
    {
        public Base()
        {
            DeviceManager deviceManager = new DeviceManager(this);
            DeviceManager.Instance.PreferredBackBufferWidth = 800;
            DeviceManager.Instance.PreferredBackBufferHeight = 600;
            DeviceManager.Instance.IsFullScreen = false;

            this.IsMouseVisible = false;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// </summary>
        protected override void Initialize()
        {
            this.Components.Add(DeviceManager.Instance.Logic);
            DeviceManager.Instance.Logic.UpdateOrder = 0;

            this.Components.Add(DeviceManager.Instance.Physics);
            DeviceManager.Instance.Physics.UpdateOrder = 1;
            DeviceManager.Instance.Physics.DrawOrder = 1;

            this.Components.Add(DeviceManager.Instance.Graphics);
            DeviceManager.Instance.Graphics.UpdateOrder = 2;
            DeviceManager.Instance.Graphics.DrawOrder = 0;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            //Unload SoundManger here because Logic is just a normal GameComponent
            SoundManager.Instance.UnloadContent();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //If Logic has flipped the switch,
            //we shutdown the game.
            if (DeviceManager.Instance.Logic.KillSwitch)
                this.Exit();

            base.Update(gameTime);
        }
    }

#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Base game = new Base())
            {
                game.Run();
            }
        }
    }
#endif
}
