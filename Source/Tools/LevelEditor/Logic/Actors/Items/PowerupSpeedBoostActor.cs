#region Using Statements
using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonsterEscape.Graphics.Animations;
using MonsterEscape.Logic.Actors;
using MonsterEscape.Logic.ClassComponents;
using MonsterEscape.Logic.Events;
using MonsterEscape.Worlds;
using MonsterEscape.Utility;
#endregion

namespace MonsterEscape.Logic.Actors.Items
{
    /// <summary>
    /// Authors: James Kirk
    /// Creation: 9.07.2010 ~ 9.27.2010
    /// Description: PowerupSpeedBoostActor Actor
    /// </summary>
    public class PowerupSpeedBoostActor : Powerup
    {
        #region Fields
        public const float SPEED_BONUS = 3.0f;
        #endregion

        #region Constructors
        public PowerupSpeedBoostActor(Guid id)
            : base(id, ActorType.POWERUP_SPEED)
        {
            SetPosition(new PositionComponent(this));
            SetBounding(new BoundingComponent(this, new Point(40, 81), new Point(40, 81)));

            SetSprite(new SpriteComponent(this, @"Powerups\powerups", new Vector2(36, 81), new Rectangle(0, 0, 71, 81), SpriteEffects.None));
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Adds this actor to the currentlevel
        /// </summary>
        /// <param name="position">The world position</param>
        public override void AddToWorld(Vector2 position)
        {
            GetPosition().Position = position;
            CurrentLevel.Instance.Items.Add(this);
        }

        /// <summary>
        /// Removes the actor from the level and from memory
        /// </summary>
        public override void RemoveFromWorld()
        {
            EventManager.Instance.QueueEvent(new RemoveActorEvent(this.ActorID, this.ActorType));
        }

        /// <summary>
        /// Float the object up and down by modifying its sprite component offset
        /// </summary>
        /// <param name="gameTime">The current game time</param>
        public override void Update(GameTime gameTime)
        {
            //Nothing
        }

        /// <summary>
        /// Apply the effects of this item to the user
        /// </summary>
        /// <param name="user">The actor using this item</param>
        public override void Use(Actor user)
        {
            //Nothing
        }
        #endregion
    }
}
