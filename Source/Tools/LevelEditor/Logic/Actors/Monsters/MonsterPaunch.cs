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
using MonsterEscape.Utility;
#endregion

namespace MonsterEscape.Logic.Actors.Monsters
{
    /// <summary>
    /// Authors: James Kirk
    /// Creation: 10.13.2010
    /// Description: Monster Paunch Actor
    /// </summary>
    public class MonsterPaunch : Monster
    {
        private bool eating;
        public bool Eating { get { return this.eating; } set { this.eating = value; } }

        #region Constructors
        public MonsterPaunch(Guid id)
            : base(id, ActorType.MONSTER_PAUNCH)
        {
            SetPosition(new PositionComponent(this));

            SetPathfind(new PathfindComponent(this, WalkerType.Ground));

            AttributeComponent attribute = new AttributeComponent(this);
            SetAttribute(attribute);
            attribute.Speed = (float)TerrainKey.TILE_SIZE / 1.5f / 30.0f;
            attribute.Health = 1;

            SetEffect(new EffectComponent(this));

            SetBounding(new BoundingComponent(this, new Point(80, 140), new Point(150, 150)));

            //Order of animations must match the order of the animation package
            string filepath = @"Monsters\Paunch\";
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
                                    filepath+"happy",           //12
                                    filepath+"eat_left",        //13
                                    filepath+"eat_left",        //14 right
                                    filepath+"eat_up",          //15
                                    filepath+"eat_down",        //16
                                    filepath+"eat_left_egg",    //17
                                    filepath+"eat_left_egg",    //18 right
                                    filepath+"eat_up_egg",      //19
                                    filepath+"eat_down_egg" };  //20
            Point[] frameSizes = { new Point(149, 160),         //1
                                   new Point(149, 160),         //2
                                   new Point(184, 157),         //3
                                   new Point(171, 156),         //4
                                   new Point(166, 157),         //5
                                   new Point(213, 155),         //6
                                   new Point(207, 201),         //7
                                   new Point(155, 159),         //8
                                   new Point(155, 159),         //9
                                   new Point(157, 157),         //10
                                   new Point(156, 156),         //11
                                   new Point(180, 183),         //12
                                   new Point(150, 188),         //13
                                   new Point(150, 188),         //14
                                   new Point(157, 182),         //15
                                   new Point(158, 182),         //16
                                   new Point(151, 187),         //17
                                   new Point(151, 187),         //18
                                   new Point(158, 182),         //19
                                   new Point(158, 182) };       //20
            Vector2[] origins = { new Vector2((frameSizes[0].X - 1) / 2 - 2, frameSizes[0].Y - 1 + 1),  //1
                                  new Vector2((frameSizes[0].X - 1) / 2 - 8, frameSizes[0].Y - 1 + 1),  //2
                                  new Vector2((frameSizes[1].X - 1) / 2 + 20, frameSizes[1].Y - 1 - 6), //3
                                  new Vector2((frameSizes[2].X - 1) / 2 - 6, frameSizes[2].Y - 1 - 4),  //4
                                  new Vector2((frameSizes[3].X - 1) / 2, frameSizes[3].Y - 1),          //5
                                  new Vector2((frameSizes[4].X - 1) / 2 + 20, frameSizes[4].Y - 1 - 6), //6
                                  new Vector2((frameSizes[5].X - 1 - 4) / 2, frameSizes[5].Y - 1 - 2),  //7
                                  new Vector2((frameSizes[6].X - 1) / 2 - 20, frameSizes[6].Y - 1 - 45),//8
                                  new Vector2((frameSizes[6].X - 1) / 2 - 20, frameSizes[6].Y - 1 - 45),//9
                                  new Vector2((frameSizes[7].X - 1) / 2, frameSizes[7].Y - 1 - 4),      //10
                                  new Vector2((frameSizes[8].X - 1) / 2, frameSizes[8].Y - 1 - 4),      //11
                                  new Vector2((frameSizes[8].X - 1) / 2 + 12, frameSizes[8].Y - 1 + 25),//12
                                  new Vector2((frameSizes[8].X - 1) / 2, frameSizes[8].Y - 1 + 28),     //13
                                  new Vector2((frameSizes[8].X - 1) / 2, frameSizes[8].Y - 1 + 28),     //14
                                  new Vector2((frameSizes[8].X - 1) / 2, frameSizes[8].Y - 1 + 23),     //15
                                  new Vector2((frameSizes[8].X - 1) / 2, frameSizes[8].Y - 1 + 23),     //16
                                  new Vector2((frameSizes[8].X - 1) / 2, frameSizes[8].Y - 1 + 28),     //17
                                  new Vector2((frameSizes[8].X - 1) / 2, frameSizes[8].Y - 1 + 28),     //18
                                  new Vector2((frameSizes[8].X - 1) / 2, frameSizes[8].Y - 1 + 23),     //19
                                  new Vector2((frameSizes[9].X - 1) / 2, frameSizes[9].Y - 1 + 23) };   //20
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
                                   AnimStyle.FORWARD,           //12
                                   AnimStyle.FORWARD,           //13
                                   AnimStyle.FORWARD,           //14
                                   AnimStyle.FORWARD,           //15
                                   AnimStyle.FORWARD,           //16
                                   AnimStyle.FORWARD,           //17
                                   AnimStyle.FORWARD,           //18
                                   AnimStyle.FORWARD,           //19
                                   AnimStyle.FORWARD };         //20
            int[] frames = { 36,            //1
                             36,            //2
                             36,            //3
                             36,            //4
                             20,            //5 idle
                             20,            //6 hurt
                             25,            //7 die
                             36,            //8
                             36,            //9
                             36,            //10
                             36,            //11
                             20,            //12 happy
                             36,            //13
                             36,            //14
                             36,            //15
                             36,            //16
                             36,            //17
                             36,            //18
                             36,            //19
                             36 };          //20

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
                                        SpriteEffects.None,             //12
                                        SpriteEffects.None,             //13
                                        SpriteEffects.FlipHorizontally, //14
                                        SpriteEffects.None,             //15
                                        SpriteEffects.None,             //16
                                        SpriteEffects.None,             //17
                                        SpriteEffects.FlipHorizontally, //18
                                        SpriteEffects.None,             //19
                                        SpriteEffects.None };           //20
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

        public override void onCollision(Enemy enemy)
        {
            //Nothing
        }

        /// <summary>
        /// Sets the current animation and clears anything queued
        /// </summary>
        /// <param name="anim">The animation index</param>
        /// <param name="repeat">Repeat when done?</param>
        public override  void PlayAnimation(int anim, bool repeat)
        {
            GetSprite().PlayAnimation(anim, repeat);
        }

        /// <summary>
        /// Queue an animation to play when the current one is complete
        /// </summary>
        /// <param name="anim">The animation index</param>
        /// <param name="repeat">Repeat when done?</param>
        public override  void QueueAnimation(int anim, bool repeat)
        {
            GetSprite().QueueAnimation(anim, repeat);
        }
        #endregion
    }
}