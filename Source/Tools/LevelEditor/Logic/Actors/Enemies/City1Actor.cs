#region Using Statements
using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonsterEscape.Graphics.Animations;
using MonsterEscape.Logic.Actors;
using MonsterEscape.Logic.Actors.Monsters;
using MonsterEscape.Logic.AI;
using MonsterEscape.Logic.ClassComponents;
using MonsterEscape.Logic.Events;
using MonsterEscape.Worlds;
#endregion

namespace MonsterEscape.Logic.Actors.Enemies
{
    /// <summary>
    /// Authors: James Kirk
    /// Creation: 9.07.2010
    /// Description: City1 Actor
    /// </summary>
    public class City1Actor : Enemy
    {
        #region Constructors
        public City1Actor(Guid id)
            : base(id, ActorType.CITY1)
        {
            SetPosition(new PositionComponent(this));

            this.AIConfig = new AIConfiguration();
            this.AIConfig.Frequency = 3.5f;
            this.AIConfig.Behavior = AIBehavior.NONE;
            this.AIConfig.SpitAim = false;
            this.AIConfig.SpitGreen = false;
            this.AIConfig.Direction = AIDirection.DIRECTIONAL;
            this.AIConfig.DirectionPreference = PathDirection.Left;
            this.AIConfig.Walker = WalkerType.Ground;
            this.AIConfig.MovementDuration = 10.0f;
            SetAI(new AIComponent(this, this.AIConfig));

            SetPathfind(new PathfindComponent(this, this.AIConfig.Walker));

            //Order of animations must match the order of the animation package
            string filepath = @"Enemies\City1\";
            string[] animations = { filepath+"walk_left",       //1
                                    filepath+"walk_left",       //2 right
                                    filepath+"walk_up",         //3
                                    filepath+"walk_down",       //4
                                    filepath+"idle",            //5
                                    filepath+"hurt",            //6
                                    filepath+"die" };           //7
            Point[] frameSizes = { new Point(168, 109),         //1 
                                   new Point(168, 109),         //2 
                                   new Point(94, 125),          //3
                                   new Point(108, 116),         //4
                                   new Point(109, 101),         //5
                                   new Point(108, 116),         //6
                                   new Point(108, 115) };       //7
            Vector2[] origins = { new Vector2((frameSizes[0].X - 1) / 2 - 30, frameSizes[0].Y - 1),      //1
                                  new Vector2((frameSizes[0].X - 1) / 2 + 30, frameSizes[0].Y - 1),      //2
                                  new Vector2((frameSizes[1].X - 1) / 2 - 35, frameSizes[1].Y - 1 + 15), //3
                                  new Vector2((frameSizes[2].X) / 2, frameSizes[2].Y - 1 - 5),           //4
                                  new Vector2((frameSizes[3].X - 1) / 2 - 6, frameSizes[3].Y - 1 - 10),  //5
                                  new Vector2((frameSizes[4].X - 1) / 2 - 7 , frameSizes[4].Y - 1 + 19), //6
                                  new Vector2((frameSizes[5].X - 1) / 2, frameSizes[5].Y - 1) };         //7
            AnimStyle[] styles = { AnimStyle.FORWARD,           //1
                                   AnimStyle.FORWARD,           //2
                                   AnimStyle.FORWARD,           //3
                                   AnimStyle.FORWARD,           //4
                                   AnimStyle.FORWARD,           //5
                                   AnimStyle.FORWARD,           //6
                                   AnimStyle.FORWARD };         //7
            int[] frames = { 20,            //1
                             20,            //2
                             20,            //3
                             20,            //4
                             20,            //5 idle
                             20,            //6 hurt
                             15 };          //7 die
            SpriteEffects[] effects = { SpriteEffects.None,             //1
                                        SpriteEffects.FlipHorizontally, //2
                                        SpriteEffects.None,             //3
                                        SpriteEffects.None,             //4
                                        SpriteEffects.None,             //5
                                        SpriteEffects.None,             //6
                                        SpriteEffects.None };           //7
            SetSprite(new SpriteComponent(this, animations, origins, frameSizes, styles, frames, effects, 20.0f / 60.0f));
            PlayAnimation(BasicAnimPackage.IDLE, true);
        }

        /// <summary>
        /// Adds this actor to the currentlevel
        /// </summary>
        /// <param name="position">The world position</param>
        public override void AddToWorld(Vector2 position)
        {
            GetPosition().Position = position;
            CurrentLevel.Instance.Enemies.Add(this);
        }

        /// <summary>
        /// Removes the actor from the level and from memory
        /// </summary>
        public override void RemoveFromWorld()
        {
            EventManager.Instance.QueueEvent(new RemoveActorEvent(this.ActorID, this.ActorType));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        /// <summary>
        /// Sets the current animation and clears anything queued
        /// </summary>
        /// <param name="anim">The animation index</param>
        /// <param name="repeat">Repeat when done?</param>
        public override void PlayAnimation(int anim, bool repeat)
        {
            GetSprite().PlayAnimation(anim, repeat);
        }

        /// <summary>
        /// Queue an animation to play when the current one is complete
        /// </summary>
        /// <param name="anim">The animation index</param>
        /// <param name="repeat">Repeat when done?</param>
        public override void QueueAnimation(int anim, bool repeat)
        {
            GetSprite().QueueAnimation(anim, repeat);
        }
        #endregion
    }
}