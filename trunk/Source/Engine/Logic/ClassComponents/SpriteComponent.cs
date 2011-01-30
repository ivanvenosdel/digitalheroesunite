#region Using Statements
using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Engine.Graphics.Cameras;
using Engine.Logic.Actors;
using Engine.Logic.Events;
using Engine.Utilities;
#endregion

namespace Engine.Logic.ClassComponents
{
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
        private List<Texture2D> textures = new List<Texture2D>();
        private List<Vector2> origins = new List<Vector2>();
        private List<Point> frameSizes = new List<Point>();
        private List<int> frames = new List<int>();
        private List<AnimStyle> animationStyles = new List<AnimStyle>();
        private List<SpriteEffects> effects = new List<SpriteEffects>();

        private float scale = 1.0f;

        private bool animationEnabled;
        private AnimPackage current;
        private Queue<AnimPackage> queue = new Queue<AnimPackage>();

        private int currentFrame;
        private float animationSpeed;
        private float animationUpdateTime;
        private bool forwardAnim;

        private Rectangle singleFrame;
        private Vector2 offset;
        #endregion

        #region Properties
        /// <summary> The current playing animation </summary>
        public int CurrentAnimation { get { return this.current.Animation; } }
        /// <summary> Stop the animation from progressing </summary>
        public bool AnimationEnabled { get { return this.animationEnabled; } set { this.animationEnabled = value; } }
        /// <summary> The number of animation options this sprite contains </summary>
        public int AnimationCount { get { return this.textures.Count; } }
        /// <summary> The scale </summary>
        public float Scale { get { return this.scale; } set { this.scale = value; } }
        /// <summary> The rendering offset </summary>
        public Vector2 Offset { get { return this.offset; } set { this.offset = value; } }
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
        /// <param name="owner">The actor the object belongs to</param>
        /// <param name="assets">File location for each spritesheet texture</param>
        /// <param name="origins">The origins of each spritesheet</param>
        /// <param name="frameSizes">The framesizes of each spritesheet</param>
        /// <param name="styles">The animation style of each spritesheet</param>
        /// <param name="frames">The number of frames in each spritesheet</param>
        /// <param name="effects">The xna sprite effect of each spritesheet</param>
        /// <param name="animSpeed">The fps of all spritesheets</param>
        public SpriteComponent(Actor owner, string[] assets, Vector2[] origins, Point[] frameSizes, AnimStyle[] styles, int[]frames, SpriteEffects[] effects, float animSpeed)
            : base(owner)
        {
            int animationCount = assets.Length;
            for (int i = 0; i < animationCount; ++i)
            {
                this.textures.Add(DeviceManager.Instance.Content.Load<Texture2D>(assets[i]));
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

            this.offset = Vector2.Zero;
        }

        public SpriteComponent(Actor actorRef, string asset, Vector2 origin, Rectangle frame, SpriteEffects effect)
            : base(actorRef)
        {
            this.textures.Add(DeviceManager.Instance.Content.Load<Texture2D>(asset));
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

            this.offset = Vector2.Zero;
        }
        #endregion

        #region Public Methods
        public override void Update(GameTime gameTime)
        {
            //Update the animation if its time
            if (this.current.Animation == -1 ||
                frames[this.current.Animation] == 1 ||
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
                    currentFrame = UtilityMath.RandomInt(0, frames[this.current.Animation]);
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
                    EventManager.Instance.QueueEvent(new AnimationDoneEvent(this.Owner, this.current.Animation));

                    this.current.Animation = -1;
                    this.current.Repeat = false;
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position)
        {
            if (this.current.Animation == -1)
                return;

            if (!Camera.Instance.OnScreen(new Rectangle((int)(this.Offset.X + position.X), (int)(this.Offset.Y + position.Y), this.frameSizes[this.current.Animation].X, this.frameSizes[this.current.Animation].Y)))
                return;

            //Draw the sprite
            spriteBatch.Draw(this.textures[this.current.Animation], this.Offset + position, GetFrame(), Color.White, 0.0f, this.origins[this.current.Animation], this.scale, this.effects[this.current.Animation], 0.0f);

#if DEBUG
            //If there is a bounding component and debug draw is on, draw the bounding box
            if (Debug.DrawBounding &&  this.Owner.GetBounding() != null)
                this.Owner.GetBounding().Draw(gameTime, spriteBatch);
#endif
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
        #endregion

        #region Private Methods
        /// <summary>
        /// If the sprite is animating determine where in the sprite sheet we are 
        /// 
        /// </summary>
        /// <returns></returns>
        private Rectangle GetFrame()
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
