#region Using Statements
using System;
using System.Threading;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace MonsterEscape
{
    /// <summary>
    /// Device Manager
    /// 
    /// Authors: James Kirk
    /// Creation: 3.08.2009
    /// Description:
    ///      Xna Core Devices, GraphicsDevice, AssetManager
    /// </summary>
    public class DeviceManager : GraphicsDeviceManager
    {
        #region Fields
        public const int SCREEN_WIDTH = 760;
        public const int SCREEN_HEIGHT = 480;
        public const int HALF_SCREEN_WIDTH = SCREEN_WIDTH / 2;
        public const int HALF_SCREEN_HEIGHT = SCREEN_HEIGHT / 2;
        private static DeviceManager instance;

        private ContentManager contentManager;
        #endregion

        #region Properties
        /// <summary>Singleton</summary>
        public static DeviceManager Instance { get { return instance; } }
        /// <summary>Custom Content Loader</summary>
        public ContentManager ContentManager { get { return this.contentManager; } }
        #endregion

        #region Constructor
        /// <summary>
        /// Do not call this function, use Instance
        /// </summary>
        public DeviceManager(Game game) : base(game)
        {
            instance = this;

            this.contentManager = game.Content;
            this.contentManager.RootDirectory = "Content";
        }
        #endregion

        #region Public Methods
        #endregion
    }
}
