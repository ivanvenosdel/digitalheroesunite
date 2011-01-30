#region Using Statements
using System;
using System.Collections;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using FarseerPhysics;
using FarseerPhysics.Common;
using FarseerPhysics.DebugViewXNA;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Collision;
using FarseerPhysics.Factories;

using Engine.Graphics.Cameras;
using Engine.Logic.Events;
using Engine.Logic.Input;
using Engine.Logic.Logger;
using Engine.World;
#endregion

namespace Engine.Physics
{
    /// <summary>
    /// Authors: James Kirk
    /// Creation: 1.30.2010
    /// Description: The Physics Core
    /// </summary>
    public class PhysicsCore : DrawableGameComponent
    {
        #region Fields
        private FarseerPhysics.Dynamics.World worldSimulation;

#if DEBUG
        DebugViewXNA debugView;
#endif
        #endregion

        #region Properties
        /// <summary>The Physics Simulator</summary>
        public FarseerPhysics.Dynamics.World WorldSimulation { get { return this.worldSimulation; } }
        #endregion

        #region Constructors
        /// <summary>Constructor</summary>
        /// <param name="game">The XNA game object</param>
        public PhysicsCore(Game game)
            : base(game)
        {
            this.worldSimulation = new FarseerPhysics.Dynamics.World(new Vector2(0, 3.5f));
            Settings.VelocityIterations = 5;
            Settings.PositionIterations = 3;

#if DEBUG
            Settings.EnableDiagnostics = Debug.DrawBounding;

            this.debugView = new DebugViewXNA(WorldSimulation);
            this.debugView.DefaultShapeColor = Color.White;
            this.debugView.SleepingShapeColor = Color.LightGray;

            this.debugView.AppendFlags(DebugViewFlags.AABB);
            this.debugView.AppendFlags(DebugViewFlags.CenterOfMass);
            //this.debugView.AppendFlags(DebugViewFlags.ContactNormals);
            this.debugView.AppendFlags(DebugViewFlags.ContactPoints);
            this.debugView.AppendFlags(DebugViewFlags.DebugPanel);
            this.debugView.AppendFlags(DebugViewFlags.Joint);
            //this.debugView.AppendFlags(DebugViewFlags.Pair);
            //this.debugView.AppendFlags(DebugViewFlags.PolygonPoints);
            this.debugView.AppendFlags(DebugViewFlags.Shape);
#endif
            //Events
            EventManager.Instance.AddListener(new EventListener(HandleEvents), EventType.KILLSWITCH);
        }
        #endregion

        #region Protected Methods
        /// <summary>Loads Graphics content</summary>
        protected override void LoadContent()
        {

        }
        #endregion

        #region Public Methods
        /// <summary>Initializes Physics Core</summary>
        public override void Initialize()
        {
#if DEBUG
            DebugViewXNA.LoadContent(DeviceManager.Instance.GraphicsDevice, DeviceManager.Instance.Content);
#endif
            base.Initialize();
        }

        /// <summary>Updates Physics and its components</summary>
        /// <param name="gameTime">The current update time</param>
        /// <devdoc>Called by the game engine</devdoc>
        public override void Update(GameTime gameTime)
        {
            if (GameWorld.Instance.Enabled)
            {
                try
                {
                    // variable time step but never less then 30 Hz
                    this.worldSimulation.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f, (1f / 30f)));
                }
                catch (Exception e)
                {
                    LogManager.Instance.Alert("Physics Exception", "Engine.Physics.Update", 0, e);
                }
            }
        }

        /// <summary>Draws the physics debug info if enabled</summary>
        /// <param name="gameTime">The current update time</param>
        public override void Draw(GameTime gameTime)
        {
#if DEBUG
            if (Debug.DrawBounding)
            {
                Matrix v = Matrix.CreateTranslation(Camera.Instance.Position.X / -DeviceManager.PixelsAMeter, Camera.Instance.Position.Y / -DeviceManager.PixelsAMeter, 0);
                Vector2 size = (((new Vector2(DeviceManager.Instance.GraphicsDevice.Viewport.Width, DeviceManager.Instance.GraphicsDevice.Viewport.Height)) / (DeviceManager.PixelsAMeter * 2)) / Camera.Instance.Zoom);
                Matrix p = Matrix.CreateOrthographicOffCenter(-size.X, size.X, size.Y, -size.Y, 0, 1);

                this.debugView.RenderDebugData(ref p, ref v);
            }
#endif
        }
        #endregion

        #region Private Methods
        /// <summary>Physics event handler</summary>
        /// <param name="evt">The event being passed in</param>
        private void HandleEvents(Event evt)
        {
            switch (evt.EventType)
            {
                case EventType.KILLSWITCH:
                    //Clear volatile Data
                    break;

                case EventType.UNKNOWN:
                default:
                    LogManager.Instance.Alert("Unknown Event Type", "Engine.AI.AiEventListener.HandleEvent", 0);
                    break;
            }
        }
        #endregion
    }
}