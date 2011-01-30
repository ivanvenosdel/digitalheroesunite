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
    public class BossActor : Actor
    {
        #region Constructors
        public BossActor(Guid id)
            : base(id, ActorType.BOSS)
        {
        }
        #endregion

        #region Properties
        #endregion

        #region Fields
        #endregion

        #region Public Methods
        public void Initialize(Vector2 pos)
        {
            SetPosition(new PositionComponent(this, pos));

            //Order of animations must match the order of the animation package
            string filepath = @"Actors\Boss\";
            string[] animations = { filepath + "BigBoss" };

            Point[] frameSizes = { new Point(160, 144) };

            Vector2[] origins = { new Vector2((frameSizes[0].X - 1) / 2, frameSizes[0].Y - 1) }
                ;
            AnimStyle[] styles = { AnimStyle.FORWARD };

            int[] frames = { 3 };

            SpriteEffects[] effects = { SpriteEffects.None };

            SetSprite(new SpriteComponent(this, animations, origins, frameSizes, styles, frames, effects, 1000.0f / 6.0f)); //2 fps
            PlayAnimation(AnimPackageVortex.SWIRL, true);
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

