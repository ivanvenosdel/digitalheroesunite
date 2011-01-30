#region Using Statements
using System;
using System.Collections;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Engine.Logic.ClassComponents;
#endregion

namespace Engine.Logic.Actors
{
    /// <summary>
    /// Authors: James Kirk
    /// Creation: 7.25.2010
    /// Description: Base Actor Types
    /// </summary>
    #region Enums
    public enum ActorType
    {
        /// <summary>Unknown Type</summary>
        UNKNOWN = 0,
        /// <summary>Hero</summary>
        HERO,
        /// <summary>End</summary>
        END
    };
    #endregion

    /// <summary>
    /// Authors: James Kirk
    /// Creation: 7.25.2010
    /// Description: Base Actor Object
    /// </summary>
    public abstract class Actor
    {
        #region Fields
        private Guid actorID;
        private ActorType actorType;
        private Dictionary<ComponentType, ClassComponent> components;

        #endregion

        #region Properties
        /// <summary>The unique id</summary>
        public Guid ActorID { get { return this.actorID; } }
        /// <summary>The actor type</summary>
        public ActorType ActorType { get { return this.actorType; } }
        /// <summary>Components</summary>
        public Dictionary<ComponentType, ClassComponent> Components { get { return this.components; } }
        #endregion

        #region Constructors
        /// <summary>
        /// The basic actor constructor
        /// </summary>
        /// <param name="ID">A unique id</param>
        /// <param name="type">The actor type</param>
        public Actor(Guid ID, ActorType type)
        {
            this.actorID = ID;
            this.actorType = type;
            this.components = new Dictionary<ComponentType, ClassComponent>();
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach (ClassComponent component in this.components.Values)
                component.Update(gameTime);
        }

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        /// <summary>
        /// Sets the current animation and clears anything queued
        /// </summary>
        /// <param name="anim">The animation index</param>
        /// <param name="repeat">Repeat when done?</param>
        public abstract void PlayAnimation(int anim, bool repeat);

        /// <summary>
        /// Queue an animation to play when the current one is complete
        /// </summary>
        /// <param name="anim">The animation index</param>
        /// <param name="repeat">Repeat when done?</param>
        public abstract void QueueAnimation(int anim, bool repeat);

        public abstract void AddToWorld();
        public virtual void RemoveFromWorld()
        {
            this.Dispose();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Clean up any components
        /// </summary>
        public void Dispose()
        {
            foreach (ClassComponent component in this.components.Values)
            {
                component.Dispose();
            }
        }

        /// <summary>Gets the Gravity Component</summary>
        public GravityComponent GetGravity()
        {
            if (this.components.ContainsKey(ComponentType.GRAVITY))
                return this.components[ComponentType.GRAVITY] as GravityComponent;
            else
                return null;
        }

        /// <summary>Gets the Bounding Component</summary>
        public BoundingComponent GetBounding()
        {
            if (this.components.ContainsKey(ComponentType.BOUNDING))
                return this.components[ComponentType.BOUNDING] as BoundingComponent;
            else
                return null;
        }

        /// <summary>Gets the Position Component</summary>
        public PositionComponent GetPosition()
        {
            if (this.components.ContainsKey(ComponentType.POSITION))
                return this.components[ComponentType.POSITION] as PositionComponent;
            else
                return null;
        }

        /// <summary>Gets the Sprite Component</summary>
        public SpriteComponent GetSprite()
        {
            if (this.components.ContainsKey(ComponentType.SPRITE))
                return this.components[ComponentType.SPRITE] as SpriteComponent;
            else
                return null;
        }
        #endregion

        #region Protected Methods
        /// <summary>Sets the Bounding Component</summary>
        protected BoundingComponent SetBounding(BoundingComponent component)
        {
            this.components[ComponentType.BOUNDING] = component;
            return component;
        }

        protected GravityComponent SetGravity(GravityComponent component)
        {
            this.components[ComponentType.GRAVITY] = component;
            return component;
        }

        /// <summary>Sets the Position Component</summary>
        protected PositionComponent SetPosition(PositionComponent component)
        {
            this.components[ComponentType.POSITION] = component;
            return component;
        }

        /// <summary>Sets the Sprite Component</summary>
        protected SpriteComponent SetSprite(SpriteComponent component)
        {
            this.components[ComponentType.SPRITE] = component;
            return component;
        }
        #endregion
    }
}
