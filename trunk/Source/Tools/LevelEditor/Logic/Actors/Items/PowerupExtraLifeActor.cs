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
    /// Creation: 9.28.2010
    /// Description: PowerupExtraLife Actor
    /// </summary>
    public class PowerupExtraLifeActor : Powerup
    {
        #region Fields
        #endregion

        #region Constructors
        public PowerupExtraLifeActor(Guid id)
            : base(id, ActorType.POWERUP_LIFE)
        {
            SetPosition(new PositionComponent(this));
            SetBounding(new BoundingComponent(this, new Point(40, 67), new Point(40, 67)));

            SetSprite(new SpriteComponent(this, @"Powerups\powerups", new Vector2(33, 67), new Rectangle(134, 0, 66, 67), SpriteEffects.None));
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
