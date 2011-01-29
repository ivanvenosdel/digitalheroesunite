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
    /// Description: Obstacle Actor
    /// </summary>
    public class ObstacleActor : Actor
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public ObstacleActor(Guid id)
            : base(id, ActorType.OBSTACLE)
        {
            SetPosition(new PositionComponent(this));
            SetBounding(new BoundingComponent(this, new Point(60, 60), new Point(60, 60)));

            string[] animations = { @"Misc\obstacle",          //1 Normal
                                    @"Misc\obstacle_break" };  //2 Break
            Point[] frameSizes = { new Point(131, 87),         //1
                                   new Point(131, 87) };       //2
            Vector2[] origins = { new Vector2((frameSizes[0].X) / 2, frameSizes[0].Y / 2),    //1
                                  new Vector2((frameSizes[0].X) / 2, frameSizes[0].Y / 2) };  //2
            AnimStyle[] styles = { AnimStyle.FORWARD,       //1
                                   AnimStyle.FORWARD };     //2
            int[] frames = { 1,            //1
                             24 };         //2
            SpriteEffects[] effects = { SpriteEffects.None,               //1
                                        SpriteEffects.FlipHorizontally }; //2
            SetSprite(new SpriteComponent(this, animations, origins, frameSizes, styles, frames, effects, 20.0f / 60.0f));
            GetSprite().PlayAnimation(ObstacleAnimPackage.NORMAL, false);
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
        #endregion
    }
}
