#region Using Statements
using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonsterEscape.Graphics.Animations;
using MonsterEscape.Logic.Actors;
using MonsterEscape.Logic.Actors.Monsters;
using MonsterEscape.Logic.ClassComponents;
using MonsterEscape.Logic.Events;
using MonsterEscape.Worlds;
using MonsterEscape.Utility;
#endregion

namespace MonsterEscape.Logic.Actors.Misc
{
    /// <summary>
    /// Authors: James Kirk
    /// Creation: 11.01.2010
    /// Description: Egg Actor
    /// </summary>
    public class EggActor : Actor
    {
        #region Fields

        #endregion

        #region Properties
        #endregion

        #region Constructors
        public EggActor(Guid id)
            : base(id, ActorType.EGG)
        {
            SetPosition(new PositionComponent(this));
            SetBounding(new BoundingComponent(this, new Point(45, 60), new Point(45, 60)));

            SetSprite(new SpriteComponent(this, @"Powerups\powerups", new Vector2(26, 54), new Rectangle(201, 0, 50, 54), SpriteEffects.None));
            Update(null);
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

        public void onCollision(Monster actor)
        {
            RemoveFromWorld();
        }
        #endregion
    }
}
