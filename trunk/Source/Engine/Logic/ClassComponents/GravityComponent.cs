using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using Engine.Logic.Actors;
using Engine.Graphics.Cameras;

namespace Engine.Logic.ClassComponents
{
    public class GravityComponent : ClassComponent
    {
        public const float GRAVITY = 0f;

        #region Constructors
        /// <summary>
        /// Apply gravity
        /// </summary>
        /// <param name="owner"></param>
        public GravityComponent(Actor owner)
            : base(owner)
        {

        }
        #endregion

        public override void Update(GameTime gametime)
        {
            if (Camera.Instance.OnScreen(new Point((int)Owner.GetPosition().Position.X, (int)Owner.GetPosition().Position.Y)))
                Owner.GetPosition().Position.Y += GRAVITY;
        }
    }
}
