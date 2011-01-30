#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Engine.Graphics.Animations;
using Engine.Logic.ClassComponents;
using System.Timers;
using Engine.Logic.Input;
#endregion

namespace Engine.Logic.Actors
{
    public class HeroActor : Actor
    {
        #region Constructors
        public HeroActor(Guid id)
            : base(id, ActorType.HERO)
        {
        }
        #endregion

        #region Properties
        public bool Jumping { get; set; }
        #endregion

        #region Fields
        private float jumpYPeakHeight;
        private float jumpYCurrentHeight;
        #endregion

        #region Public Methods
        public void Initialize(Vector2 pos, Point dim)
        {
            SetPosition(new PositionComponent(this, pos));
            SetBounding(new BoundingComponent(this, dim));
            SetGravity(new GravityComponent(this));

            //Order of animations must match the order of the animation package
            string filepath = @"Actors\Hero\";
            string[] animations = { filepath+"HeroRun",      //1
                                    filepath+"HeroRun",      //2 left
                                    filepath+"HeroJump",        //3
                                    filepath+"HeroJump",        //4 left
                                    filepath+"HeroStanding",      //5
                                    filepath+"HeroWhip" };         //6
            Point[] frameSizes = { new Point(160, 144),         //1
                                   new Point(160, 144),         //2
                                   new Point(160, 144),          //3
                                   new Point(160, 144),          //4
                                   new Point(160, 144),          //4
                                   new Point(160, 144) };        //5
            Vector2[] origins = { new Vector2((frameSizes[0].X - 1) / 2, frameSizes[0].Y - 1),     //1
                                  new Vector2((frameSizes[0].X - 1) / 2, frameSizes[0].Y - 1),     //2
                                  new Vector2((frameSizes[1].X - 1) / 2, frameSizes[1].Y - 1),          //3
                                  new Vector2((frameSizes[2].X - 1) / 2, frameSizes[2].Y - 1),          //4
                                  new Vector2((frameSizes[2].X - 1) / 2, frameSizes[2].Y - 1),          //5
                                  new Vector2((frameSizes[3].X - 1) / 2, frameSizes[3].Y - 1) };        //6
            AnimStyle[] styles = { AnimStyle.FORWARD,           //1
                                   AnimStyle.FORWARD,           //2
                                   AnimStyle.FORWARD,           //3
                                   AnimStyle.FORWARD,           //4
                                   AnimStyle.FORWARD,           //5
                                   AnimStyle.FORWARD };         //6
            int[] frames = { 3,            //1
                             3,            //2
                             1,            //3
                             1,            //4
                             2,            //5
                             3 };          //6
            SpriteEffects[] effects = { SpriteEffects.None,             //1
                                        SpriteEffects.FlipHorizontally, //2
                                        SpriteEffects.None,             //3
                                        SpriteEffects.FlipHorizontally, //4
                                        SpriteEffects.None,             //5
                                        SpriteEffects.None };           //6
            SetSprite(new SpriteComponent(this, animations, origins, frameSizes, styles, frames, effects, 1000.0f / 6.0f)); //2 fps
            PlayAnimation(AnimPackageHero.STAND, true);
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
            if (GetSprite() != null)
            {
                GetSprite().Draw(gameTime, spriteBatch, GetPosition().Position);
            }
        }

        public void WarpTo(Vector2 start)
        {
            this.GetPosition().Position = start;
        }

        public override void Update(GameTime gameTime)
        {
            float positionAdjust = 0;
            if (this.Jumping)
            {
                //if (this.movingUp)
                //{
                    if (this.jumpYCurrentHeight < jumpYPeakHeight)
                    {
                        //We have to go up some more.
                        positionAdjust -= Heromanager.JUMP_RATE;
                        this.jumpYCurrentHeight += Heromanager.JUMP_RATE;
                    }
                    //else
                    //{
                    //    this.movingUp = false;
                    //}
                //}
                //else
                //{
                //    if (this.jumpYCurrentHeight > 0)
                //    {
                //        //We have to go back down some more
                //        positionAdjust += 8;
                //        this.jumpYCurrentHeight -= 8;
                //    }
                //    else
                //    {
                //        //Were done! Stop jumping
                //        this.Jumping = false;
                //        this.movingUp = true;
                //    }
                //}
                this.GetPosition().Position.Y += positionAdjust;
            }

            Vector2 tempPosition = this.GetPosition().Position;
            base.Update(gameTime);
        }

        public void Walk(float dX)
        {
            this.GetPosition().Position.X += dX;
        }

        public void BeginJump(float height)
        {
            //Set constraints
            this.jumpYCurrentHeight = 0;
            this.jumpYPeakHeight = height;
            //Go into jump mode
            this.Jumping = true;

            if (GetSprite().CurrentAnimation == AnimPackageHero.RUN_LEFT)
                PlayAnimation(AnimPackageHero.JUMP_LEFT, true);
            else
                PlayAnimation(AnimPackageHero.JUMP_RIGHT, true);
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
