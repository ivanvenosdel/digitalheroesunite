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
    /// Creation: 10.13.2010
    /// Description: Monster Cy Actor
    /// </summary>
    public class MonsterCyActor : Monster
    {
        #region Constructors
        public MonsterCyActor(Guid id)
            : base(id, ActorType.MONSTER_CY)
        {
            SetPosition(new PositionComponent(this));

            SetPathfind(new PathfindComponent(this, WalkerType.Ground));

            AttributeComponent attribute = new AttributeComponent(this);
            SetAttribute(attribute);
            attribute.Speed = (float)TerrainKey.TILE_SIZE / 1.5f / 30.0f;
            attribute.Health = 1;

            SetEffect(new EffectComponent(this));

            SetBounding(new BoundingComponent(this, new Point(70, 200), new Point(100, 200)));

            //Order of animations must match the order of the animation package
            //Order of animations must match the order of the animation package
            string filepath = @"Monsters\Cy\";
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
                                    filepath+"idle_egg",        //12
                                    filepath+"happy",           //13
                                    filepath+"smash_left",        //14
                                    filepath+"smash_left",        //15 right
                                    filepath+"smash_up",          //16
                                    filepath+"smash_down",        //17
                                    filepath+"smash_left_egg",    //18
                                    filepath+"smash_left_egg",    //19 right
                                    filepath+"smash_up_egg",      //20
                                    filepath+"smash_down_egg" };  //21
            Point[] frameSizes = { new Point(129, 185),         //1
                                   new Point(129, 185),         //2
                                   new Point(142, 179),         //3
                                   new Point(138, 177),         //4
                                   new Point(135, 177),         //5
                                   new Point(151, 183),         //6
                                   new Point(243, 218),         //7
                                   new Point(125, 183),         //8
                                   new Point(125, 183),         //9
                                   new Point(151, 178),         //10
                                   new Point(151, 178),         //11
                                   new Point(139, 177),         //12
                                   new Point(243, 208),         //13
                                   new Point(221, 201),         //14
                                   new Point(221, 201),         //15
                                   new Point(236, 195),         //16
                                   new Point(236, 195),         //17
                                   new Point(168, 201),         //18
                                   new Point(168, 201),         //19
                                   new Point(190, 195),         //20
                                   new Point(196, 195) };       //21
            Vector2[] origins = { new Vector2((frameSizes[0].X - 1) / 2, frameSizes[0].Y - 1),              //1
                                  new Vector2((frameSizes[1].X - 1) / 2, frameSizes[1].Y - 1),              //2
                                  new Vector2((frameSizes[2].X - 1) / 2, frameSizes[2].Y - 1),              //3
                                  new Vector2((frameSizes[3].X - 1) / 2, frameSizes[3].Y - 1),              //4
                                  new Vector2((frameSizes[4].X - 1) / 2, frameSizes[4].Y - 1),              //5
                                  new Vector2((frameSizes[5].X - 1) / 2 + 10, frameSizes[5].Y - 1 + 4),     //6
                                  new Vector2((frameSizes[6].X - 1) / 2 + 4, frameSizes[6].Y - 1 - 35),     //7
                                  new Vector2((frameSizes[7].X - 1) / 2, frameSizes[7].Y - 1),              //8
                                  new Vector2((frameSizes[8].X - 1) / 2, frameSizes[8].Y - 1),              //9
                                  new Vector2((frameSizes[9].X - 1) / 2, frameSizes[9].Y - 1),              //10
                                  new Vector2((frameSizes[10].X - 1) / 2, frameSizes[10].Y - 1),            //11
                                  new Vector2((frameSizes[11].X - 1) / 2, frameSizes[11].Y - 1),            //12
                                  new Vector2((frameSizes[12].X - 1) / 2 - 8, frameSizes[12].Y - 2),        //13
                                  new Vector2((frameSizes[13].X - 1) / 2 + 8, frameSizes[13].Y - 1 - 15),   //14
                                  new Vector2((frameSizes[14].X - 1) / 2 - 8, frameSizes[14].Y - 1 - 15),   //15
                                  new Vector2((frameSizes[15].X - 1) / 2 - 8, frameSizes[15].Y - 1 - 15),   //16
                                  new Vector2((frameSizes[16].X - 1) / 2 - 8, frameSizes[16].Y - 1 - 15),   //17
                                  new Vector2((frameSizes[17].X - 1) / 2 - 8, frameSizes[17].Y - 1 - 15),   //18
                                  new Vector2((frameSizes[18].X - 1) / 2 + 12, frameSizes[18].Y - 1 - 15),  //19
                                  new Vector2((frameSizes[19].X - 1) / 2 + 12, frameSizes[19].Y - 1 - 15),  //20
                                  new Vector2((frameSizes[20].X - 1) / 2 + 12, frameSizes[20].Y - 1 - 15) };//21
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
                                   AnimStyle.FORWARD,           //20
                                   AnimStyle.FORWARD };         //21
            int[] frames = { 22,            //1
                             22,            //2
                             22,            //3
                             22,            //4
                             22,            //5 idle
                             14,            //6 hurt
                             22,            //7 die
                             22,            //8
                             22,            //9
                             22,            //10
                             22,            //11
                             22,            //12
                             20,            //13 happy
                             22,            //14
                             22,            //15
                             22,            //16
                             22,            //17
                             22,            //18
                             22,            //19
                             22,            //20
                             22 };          //21
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
                                        SpriteEffects.None,             //14
                                        SpriteEffects.FlipHorizontally, //15
                                        SpriteEffects.None,             //16
                                        SpriteEffects.None,             //17
                                        SpriteEffects.None,             //18
                                        SpriteEffects.FlipHorizontally, //19
                                        SpriteEffects.None,             //20
                                        SpriteEffects.None };           //21
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