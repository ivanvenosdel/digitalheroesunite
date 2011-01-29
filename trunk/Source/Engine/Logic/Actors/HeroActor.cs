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
        private Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public bool Jumping { get; set; }
        #endregion

        #region Fields
        private float jumpYPeakHeight;
        private bool movingUp = true;
        private float jumpYCurrentHeight;
        #endregion

        #region Private Methods
        private void ContinueJumpHandler(object sender, EventArgs e)
        {
           
        }
        #endregion

        #region Public Methods
        public void Initialize(Vector2 pos, Point dim)
        {
//TEMP
            SetPosition(new PositionComponent(this, pos));
            SetBounding(new BoundingComponent(this, dim));

            //Order of animations must match the order of the animation package
            string filepath = @"Actors\Tangy\";
            string[] animations = { filepath+"walk_left",      //1
                                    filepath+"walk_left",      //2 right
                                    filepath+"walk_up",        //3
                                    filepath+"walk_down",      //4
                                    filepath+"idle" };         //5
            Point[] frameSizes = { new Point(100, 152),         //1
                                   new Point(100, 152),         //2
                                   new Point(92, 153),          //3
                                   new Point(90, 152),          //4
                                   new Point(86, 152) };        //5
            Vector2[] origins = { new Vector2((frameSizes[0].X - 1) / 2 - 14, frameSizes[0].Y - 1),     //1
                                  new Vector2((frameSizes[0].X - 1) / 2 + 12, frameSizes[0].Y - 1),     //2
                                  new Vector2((frameSizes[1].X - 1) / 2, frameSizes[1].Y - 1),          //3
                                  new Vector2((frameSizes[2].X - 1) / 2, frameSizes[2].Y - 1),          //4
                                  new Vector2((frameSizes[3].X - 1) / 2, frameSizes[3].Y - 1) };        //5
            AnimStyle[] styles = { AnimStyle.FORWARD,           //1
                                   AnimStyle.FORWARD,           //2
                                   AnimStyle.FORWARD,           //3
                                   AnimStyle.FORWARD,           //4
                                   AnimStyle.FORWARD };         //5
            int[] frames = { 31,            //1
                             31,            //2
                             31,            //3
                             31,            //4
                             20 };          //5
            SpriteEffects[] effects = { SpriteEffects.None,             //1
                                        SpriteEffects.FlipHorizontally, //2
                                        SpriteEffects.None,             //3
                                        SpriteEffects.None,             //4
                                        SpriteEffects.None };           //5
            SetSprite(new SpriteComponent(this, animations, origins, frameSizes, styles, frames, effects, 1000.0f / 30.0f)); //20 fps
            PlayAnimation(AnimPackageHero.IDLE, true);
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

        public override void Update(GameTime gameTime)
        {
            if (this.Jumping)
            {
                if (this.movingUp)
                {
                    if (this.jumpYCurrentHeight < jumpYPeakHeight)
                    {
                        //We have to go up some more.
                        this.position.Y -= 8;
                        this.jumpYCurrentHeight += 8;
                    }
                    else
                    {
                        this.movingUp = false;
                    }
                }
                else
                {
                    if (this.jumpYCurrentHeight >= 0)
                    {
                        //We have to go back down some more
                        this.position.Y += 8;
                        this.jumpYCurrentHeight -= 8;
                    }
                    else
                    {
                        //Were done! Stop jumping
                        this.Jumping = false;
                        this.movingUp = true;
                    }
                }
                this.SetPosition(new PositionComponent(this, this.position));
            }
            base.Update(gameTime);
        }

        public void Walk(float dX)
        {
            this.position.X += dX;

            this.SetPosition(new PositionComponent(this, this.position));
        }

        public void BeginJump(float height)
        {
            //Set constraints
            this.jumpYCurrentHeight = 0;
            this.jumpYPeakHeight = height;
            //Go into jump mode
            this.Jumping = true;
            this.movingUp = true;
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
