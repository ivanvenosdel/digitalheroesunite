#region Using Statements
using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonsterEscape.Graphics.Animations;
using MonsterEscape.Logic.Actors;
using MonsterEscape.Logic.Actors.Enemies;
using MonsterEscape.Logic.AI;
using MonsterEscape.Logic.ClassComponents;
using MonsterEscape.Logic.Events;
using MonsterEscape.Worlds;
#endregion

namespace MonsterEscape.Logic.Actors.Monsters
{
    /// <summary>
    /// Authors: James Kirk
    /// Creation: 5.17.2010
    /// Description: Monster Tangy Actor
    /// </summary>
    public class MonsterTangy : Monster
    {
        #region Constructors
        public MonsterTangy(Guid id)
            : base(id, ActorType.MONSTER_TANGY)
        {
            SetPosition(new PositionComponent(this));

            SetPathfind(new PathfindComponent(this, WalkerType.Ground));

            AttributeComponent attribute = new AttributeComponent(this);
            SetAttribute(attribute);
            attribute.Speed = (float)TerrainKey.TILE_SIZE / 1.5f / 30.0f;
            attribute.Health = 1;

            SetEffect(new EffectComponent(this));

            SetBounding(new BoundingComponent(this, new Point(70, 150), new Point(100, 150)));

            //Order of animations must match the order of the animation package
            string filepath = @"Monsters\Tangy\";
            string[] animations = { filepath+"walk_left",       //1
                                    filepath+"walk_left",       //2 right
                                    filepath+"walk_up",         //3
                                    filepath+"walk_down",       //4
                                    filepath+"idle",            //5
                                    filepath+"hurt",            //6
                                    filepath+"die",             //7
                                    filepath+"walk_left_egg",   //8
                                    filepath+"walk_left_egg",   //9 right
                                    filepath+"walk_up_egg",     //10
                                    filepath+"walk_down_egg",   //11
                                    filepath+"happy" };         //12
            Point[] frameSizes = { new Point(100, 152),         //1
                                   new Point(100, 152),         //2
                                   new Point(92, 153),          //3
                                   new Point(90, 152),          //4
                                   new Point(86, 152),          //5
                                   new Point(82, 150),          //6
                                   new Point(100, 163),         //7
                                   new Point(100, 192),         //8
                                   new Point(100, 192),         //9
                                   new Point(97, 183),          //10
                                   new Point(98, 194),          //11
                                   new Point(167, 190) };       //12
            Vector2[] origins = { new Vector2((frameSizes[0].X - 1) / 2 - 14, frameSizes[0].Y - 1),     //1
                                  new Vector2((frameSizes[0].X - 1) / 2 + 12, frameSizes[0].Y - 1),      //2
                                  new Vector2((frameSizes[1].X - 1) / 2, frameSizes[1].Y - 1),          //3
                                  new Vector2((frameSizes[2].X - 1) / 2, frameSizes[2].Y - 1),          //4
                                  new Vector2((frameSizes[3].X - 1) / 2, frameSizes[3].Y - 1),          //5
                                  new Vector2((frameSizes[4].X - 1) / 2 - 4, frameSizes[4].Y - 1 - 4),  //6
                                  new Vector2((frameSizes[5].X - 1) / 2 + 12, frameSizes[5].Y - 1 + 4), //7
                                  new Vector2((frameSizes[6].X - 1) / 2 - 20, frameSizes[6].Y - 1 + 30),//8
                                  new Vector2((frameSizes[6].X - 1) / 2 + 12, frameSizes[6].Y - 1 + 30), //9
                                  new Vector2((frameSizes[7].X - 1) / 2, frameSizes[7].Y - 1 - 10),     //10
                                  new Vector2((frameSizes[8].X - 1) / 2, frameSizes[8].Y - 1 + 4),      //11
                                  new Vector2((frameSizes[9].X - 1) / 2 + 35, frameSizes[9].Y - 1) };   //12
            AnimStyle[] styles = { AnimStyle.FORWARD,           //1
                                   AnimStyle.FORWARD,           //2
                                   AnimStyle.FORWARD,           //3
                                   AnimStyle.FORWARD,           //4
                                   AnimStyle.FORWARD,           //5
                                   AnimStyle.FORWARD,           //6
                                   AnimStyle.FORWARD,           //7
                                   AnimStyle.FORWARD,           //8
                                   AnimStyle.FORWARD,           //9
                                   AnimStyle.FORWARD,           //10
                                   AnimStyle.FORWARD,           //11
                                   AnimStyle.FORWARD };         //12
            int[] frames = { 31,            //1
                             31,            //2
                             31,            //3
                             31,            //4
                             20,            //5 idle
                             15,            //6 hurt
                             24,            //7 die
                             31,            //8
                             31,            //9
                             31,            //10
                             31,            //11
                             23 };          //12 happy
            SpriteEffects[] effects = { SpriteEffects.None,             //1
                                        SpriteEffects.FlipHorizontally, //2
                                        SpriteEffects.None,             //3
                                        SpriteEffects.None,             //4
                                        SpriteEffects.None,             //5
                                        SpriteEffects.None,             //6
                                        SpriteEffects.None,             //7
                                        SpriteEffects.None,             //8
                                        SpriteEffects.FlipHorizontally, //9
                                        SpriteEffects.None,             //10
                                        SpriteEffects.None,             //11
                                        SpriteEffects.None };           //12
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
            CurrentLevel.Instance.Monsters.Add(this);
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