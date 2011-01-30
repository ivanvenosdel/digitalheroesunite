#region Using Statements
using System;
using System.Collections.Generic;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Engine.Logic.Input;
using Engine.Logic.Actors;
using Engine.Logic.ClassComponents;
using Engine.Utilities;
#endregion

namespace Engine.Graphics.Cameras
{
    /// <summary>
    /// Authors: James Kirk
    /// Creation: 4.20.2010
    /// Description: Main Camera
    /// </summary>
    public class CameraControlActor : Actor
    {
        #region Fields
        
        #endregion

        #region Properties
        public Vector2 Direction { get; set; }
        #endregion

        #region Constructors
        public CameraControlActor(Guid id)
            : base(id, ActorType.CAMERACONTROL)
        {
        }
        #endregion

        #region Public Methods
        /// <summary>Initializes</summary>
        public void Initialize(Vector2 position, Vector2 direction)
        {
            this.Direction = direction;
            SetPosition(new PositionComponent(this, position));
            SetBounding(new BoundingComponent(this, new Point(60,60)));
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 scaledPoint = UtilityGame.PhysicsToGame(this.GetPosition().Position);
            Point point = new Point(Convert.ToInt32(scaledPoint.X), Convert.ToInt32(scaledPoint.Y));
            if (Camera.Instance.OnScreen(point))
            {
                Camera.Instance.Direction = Direction;
            }
            base.Update(gameTime);
        }

        public override void AddToWorld()
        {

        }

        public override void RemoveFromWorld()
        {

            base.RemoveFromWorld();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
#if DEBUG
            //If there is a bounding component and debug draw is on, draw the bounding box
            if (Debug.DrawBounding)
                this.GetBounding().Draw(gameTime, spriteBatch);
#endif
        }

        public override void PlayAnimation(int anim, bool repeat)
        {
        }

        public override void QueueAnimation(int anim, bool repeat)
        {
        }
        #endregion

        #region Private Methods
        #endregion
    }
}