#region Using Statements
using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonsterEscape;
using MonsterEscape.Graphics.Cameras;
using MonsterEscape.Logic.Actors;
using MonsterEscape.Logic.Actors.Enemies;
using MonsterEscape.Logic.Actors.Monsters;
using MonsterEscape.Logic.Events;
using MonsterEscape.Utility;
#endregion

namespace MonsterEscape.Logic.ClassComponents
{
    /// <summary>
    /// Authors: James Kirk
    /// Creation: 5.08.2010
    /// Description: Common Component
    /// </summary>
    #region Enum
    public enum AnimStyle
    {
        /// <summary>LEFT TO RIGHT</summary>
        FORWARD = 0,
        /// <summary>LEFT TO RIGHT THEN BACK TO LEFT</summary>
        PING_PONG,
        /// <summary>TOTAL RANDOM</summary>
        RANDOM,
        /// <summary>NO ANIMATION</summary>
        STILL
    };
    #endregion

    /// <summary>
    /// Authors: James Kirk
    /// Creation: 5.1.2010
    /// Description: Sprite Component
    /// </summary>
    public class SpriteComponent : ClassComponent
    {
        #region Fields
        public const int BOUNCE_FRAMES = 16;//~0.5 seconds at 30 fps
        public const int HALF_BOUNCE_FRAMES = BOUNCE_FRAMES / 2;
        public const int SHRINK_FRAMES = 30;//~1.0 seconds at 30 fps
        private List<Texture2D> textures = new List<Texture2D>();
        private List<Vector2> origins = new List<Vector2>();
        private List<Point> frameSizes = new List<Point>();
        private List<int> frames = new List<int>();
        private List<AnimStyle> animationStyles = new List<AnimStyle>();
        private List<SpriteEffects> effects = new List<SpriteEffects>();

        private float scale = 1.0f;
        private Texture2D shadow;

        private bool animationEnabled;
        private AnimPackage current;
        private Queue<AnimPackage> queue = new Queue<AnimPackage>();

        private int currentFrame;
        private float animationSpeed;
        private float animationUpdateTime;
        private bool forwardAnim;

        private Rectangle singleFrame;
        private int bounceFrames = 0;
        private int shrinkFrames = 0;
        private Vector2 bounceDelta = Vector2.Zero;
        private Vector2 shrinkTarget = Vector2.Zero;
        private Vector2 shrinkOrigin = Vector2.Zero;
        public Vector2 Offset;
        #endregion

        #region Properties
        public int CurrentAnimation { get { return this.current.Animation; } }

        public bool AnimationEnabled { get { return this.animationEnabled; } set { this.animationEnabled = value; } }

        public int AnimationCount { get { return this.textures.Count; } }
        #endregion

        #region Structs
        protected class AnimPackage
        {
            public int Animation;
            public bool Repeat;

            public AnimPackage()
            {
                Animation = -1;
                Repeat = false;
            }

            public AnimPackage(int animation, bool repeat)
            {
                Animation = animation;
                Repeat = repeat;
            }
        }
        #endregion

        #region Constructors
        /// <summary>The component used to render our characters to the screen</summary>
        /// <param name="actorRef">The actor the object belongs to</param>
        public SpriteComponent(Actor actorRef, string[] assets, Vector2[] origins, Point[] frameSizes, AnimStyle[] styles, int[] frames, SpriteEffects[] effects, float animSpeed)
            : base(actorRef)
        {
            int animationCount = assets.Length;
            for (int i = 0; i < animationCount; ++i)
            {
                this.textures.Add(DeviceManager.Instance.ContentManager.Load<Texture2D>(assets[i]));
                this.origins.Add(origins[i]);

                this.frameSizes.Add(frameSizes[i]); ;
                this.frames.Add(frames[i]);

                this.animationStyles.Add(styles[i]);
                this.effects.Add(effects[i]);
            }

            this.animationSpeed = animSpeed;
            this.animationEnabled = true;

            this.current = new AnimPackage();
            this.queue.Clear();

            this.Offset = Vector2.Zero;

            this.shadow = DeviceManager.Instance.ContentManager.Load<Texture2D>(@"Powerups\powerups"); //FIXME temp shadow for all things
        }

        public SpriteComponent(Actor actorRef, string asset, Vector2 origin, Rectangle frame, SpriteEffects effect)
            : base(actorRef)
        {
            this.textures.Add(DeviceManager.Instance.ContentManager.Load<Texture2D>(asset));
            this.origins.Add(origin);

            this.singleFrame = frame;
            this.frameSizes.Add(new Point(frame.Width, frame.Height));
            this.frames.Add(1);

            this.animationEnabled = false;

            this.animationStyles.Add(AnimStyle.STILL);
            this.effects.Add(effect);

            this.animationSpeed = 0;

            this.current = new AnimPackage(0, false);
            this.queue.Clear();

            this.Offset = Vector2.Zero;

            this.shadow = DeviceManager.Instance.ContentManager.Load<Texture2D>(@"Powerups\powerups"); //FIXME temp shadow for all things
        }
        #endregion

        #region Public Methods
        public override void Update(GameTime gameTime)
        {
            if (this.shrinkFrames > 0)
            {
                float ratio = shrinkFrames / (float)SHRINK_FRAMES;
                float x = MathHelper.Lerp(this.shrinkTarget.X, this.shrinkOrigin.X, ratio);
                float y = MathHelper.Lerp(this.shrinkTarget.Y, this.shrinkOrigin.Y, ratio);
                this.Offset = new Vector2(x, y);

                this.scale = MathHelper.Lerp(0.0f, 1.0f, ratio);

                --this.shrinkFrames;

                //If this was an enemy being eaten, remove it from the world and reset the monster eating it
                if (shrinkFrames == 0 && ((this.ActorRef is Enemy) && (this.ActorRef as Enemy).BeingEatenByMonster != null))
                {
                    (this.ActorRef as Enemy).BeingEatenByMonster.Eating = false;
                    (this.ActorRef as Enemy).BeingEatenByMonster.GetEffect().Enabled = false;
                    EventManager.Instance.QueueEvent(new RemoveActorEvent(this.ActorRef.ActorID, this.ActorRef.ActorType));
                }
            }
            else if (this.bounceFrames > 0)
            {
                if (this.bounceFrames >= HALF_BOUNCE_FRAMES)
                    this.Offset += bounceDelta;
                else
                    this.Offset.Y -= bounceDelta.Y;
                --this.bounceFrames;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position, bool drawShadow, Vector2 scale) 
        {
            if (this.current.Animation == -1)
                return;

            if (scale == Vector2.Zero)
                scale = new Vector2(this.scale, this.scale);

            //Draw a Shadow if necessary
            if (drawShadow)
                spriteBatch.Draw(this.shadow, position + new Vector2(0, 5), new Rectangle(0, 95, 66, 115), Color.White, 0.0f, new Vector2(24, 8), scale, SpriteEffects.None, 0.0f);

            Color effectColor = Color.White;
            if (this.ActorRef.GetEffect() != null)
                effectColor = this.ActorRef.GetEffect().GetEffectColor();

            //Draw the sprite
            spriteBatch.Draw(this.textures[this.current.Animation], this.Offset + position, GetFrame(), effectColor, 0.0f, this.origins[this.current.Animation], scale, this.effects[this.current.Animation], 0.0f);

            //Update the animation if its time
            if (frames[this.current.Animation] == 1 ||
                this.animationStyles[this.current.Animation] == AnimStyle.STILL ||
                gameTime.TotalGameTime.TotalMilliseconds < this.animationUpdateTime)
                return;

            //Set a new animation update time
            this.animationUpdateTime = (float)gameTime.TotalGameTime.TotalMilliseconds + this.animationSpeed;

            //Update the current frame
            bool completed = false;
            switch (this.animationStyles[this.current.Animation])
            {
                case AnimStyle.FORWARD:
                    ++currentFrame;
                    if (currentFrame >= frames[this.current.Animation])
                    {
                        if (!this.current.Repeat)
                            completed = true;
                        currentFrame = 0;
                    }
                    break;
                case AnimStyle.RANDOM:
                    currentFrame = WinphoneMath.RandomInt(0, frames[this.current.Animation]);
                    if (!this.current.Repeat)
                        completed = true;
                    break;
                case AnimStyle.PING_PONG:
                default:
                    if (this.forwardAnim)
                        ++currentFrame;
                    else
                        --currentFrame;
                    if (currentFrame == 0 || currentFrame == frames[this.current.Animation] - 1)
                    {
                        if (!this.current.Repeat)
                            completed = true;
                        this.forwardAnim = !this.forwardAnim;
                    }
                    break;
            }

            //All done, time to move on to the next animation
            if (completed)
            {
                if (!this.animationEnabled)
                    this.queue.Clear();

                if (this.queue.Count > 0)
                {

                    AnimPackage next = this.queue.Dequeue();

                    this.current.Animation = next.Animation;
                    this.current.Repeat = next.Repeat;
                }
                else
                {
                    //We're done with all animations
                    EventManager.Instance.QueueEvent(new AnimationDoneEvent(this.ActorRef, this.current.Animation));

                    this.current.Animation = -1;
                    this.current.Repeat = false;

                }
            }
        }

        /// <summary>
        /// Sets the current animation and clears anything queued
        /// </summary>
        /// <param name="anim">The animation index</param>
        /// <param name="repeat">Repeat when done?</param>
        public void PlayAnimation(int anim, bool repeat)
        {
            if (!this.animationEnabled)
                return;

            this.current.Animation = anim;
            this.current.Repeat = repeat;

            //Setup for a new animation
            this.currentFrame = 0;
            this.animationUpdateTime = 0.0f;
            this.forwardAnim = true;

            //Clear the queued animation
            this.queue.Clear();
        }

        /// <summary>
        /// Queue an animation to play when the current one is complete
        /// </summary>
        /// <param name="anim">The animation index</param>
        /// <param name="repeat">Repeat when done?</param>
        public void QueueAnimation(int anim, bool repeat)
        {
            if (!this.animationEnabled)
                return;

            if (this.current.Animation == -1)
            {
                PlayAnimation(anim, repeat);
                return;
            }

            this.queue.Enqueue(new AnimPackage(anim, repeat));
        }

        /// <summary>
        /// Query if the sprite has the passed animation
        /// </summary>
        /// <param name="anim">The animation</param>
        /// <returns>True if it does have it</returns>
        public bool HasAnimation(int anim)
        {
            if (anim < this.frames.Count && anim >= 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Causes the sprite to bounce into the air and then fall back down
        /// </summary>
        public void Bounce(Vector2 delta)
        {
            bounceFrames = BOUNCE_FRAMES;
            bounceDelta = delta;
        }

        /// <summary>
        /// Causes the sprite to scale down to zero toward a target
        /// </summary>
        public void Shrink(Vector2 target)
        {
            this.shrinkFrames = SHRINK_FRAMES;
            this.shrinkTarget = target;
            this.shrinkOrigin = this.Offset;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// If the sprite is animating determine where in the sprite sheet we are 
        /// 
        /// </summary>
        /// <returns></returns>
        public Rectangle GetFrame()
        {
            //Do we have a defined single frame?
            if (this.singleFrame.Width != 0)
            {
                return this.singleFrame;
            }
            else
            {
                int framesPerRow = this.textures[this.current.Animation].Width / frameSizes[this.current.Animation].X;
                int xPos = (this.currentFrame % framesPerRow) * frameSizes[this.current.Animation].X;
                int yPos = (this.currentFrame / framesPerRow) * frameSizes[this.current.Animation].Y;

                return new Rectangle(xPos, yPos, this.frameSizes[this.current.Animation].X, this.frameSizes[this.current.Animation].Y);
            }
        }
        #endregion
    }
}
