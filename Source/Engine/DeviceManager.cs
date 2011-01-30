#region Using Statements
using System;
using System.Threading;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Engine.Graphics;
using Engine.Logic;
using Engine.Physics;
#endregion

namespace Engine
{
    /// <summary>
    /// Authors: James Kirk
    /// Creation: 3.08.2009
    /// Description:
    ///      Engine Cores, GraphicsDevice, ContentManager
    /// </summary>
    public class DeviceManager : GraphicsDeviceManager
    {
        #region Fields
        private static DeviceManager instance;

        private ContentManager content;
        private bool paused;

        public static float PixelsAMeter = 100.0f;

        private LogicCore logic;
        private GraphicsCore graphics;
        private PhysicsCore physics;
        #endregion

        #region Properties
        /// <summary>Singleton</summary>
        public static DeviceManager Instance { get { return instance; } }
        /// <summary>Custom Content Loader</summary>
        public ContentManager Content { get { return this.content; } }
        /// <summary>Game Paused</summary>
        public bool Paused { get { return this.paused; } set { this.paused = value; } }
        /// <summary>Logic Core</summary>
        public LogicCore Logic { get { return this.logic; } }
        /// <summary>Physics Core</summary>
        public PhysicsCore Physics { get { return this.physics; } }
        /// <summary>Graphics Core</summary>
        public GraphicsCore Graphics { get { return this.graphics; } }
        #endregion

        #region Constructor
        /// <summary>
        /// Do not call this function, use Instance
        /// </summary>
        public DeviceManager(Game game) : base(game)
        {
            instance = this;
           
            this.content = game.Content;
            this.content.RootDirectory = "Content";
            this.paused = false;

            Initialize(game);
        }
        #endregion

        #region Private Methods
        private void Initialize(Game game)
        {
            this.logic = new LogicCore(game);
            this.physics = new PhysicsCore(game);
            this.graphics = new GraphicsCore(game);
        }
        #endregion
    }
}
