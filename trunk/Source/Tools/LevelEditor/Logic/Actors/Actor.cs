#region Using Statements
using System;
using System.Collections;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonsterEscape.Logic.ClassComponents;
#endregion

namespace MonsterEscape.Logic.Actors
{
    /// <summary>
    /// Authors: James Kirk
    /// Creation: 9.9.2007
    /// Description: Base Actor Type
    /// </summary>
    #region Enums
    public enum ActorType
    {
        /// <summary>Unknown Type</summary>
        UNKNOWN = 0,
        /// <summary>Tangy</summary>
        MONSTER_TANGY,
        /// <summary>Paunch</summary>
        MONSTER_PAUNCH,
        /// <summary>Nimbleton</summary>
        MONSTER_NIMBLETON,
        /// <summary>Cy</summary>
        MONSTER_CY,
        /// <summary>Enemy City1</summary>
        CITY1,
        /// <summary>Enemy City2</summary>
        CITY2,
        /// <summary>Enemy City3</summary>
        CITY3,
        /// <summary>Enemy Sewer1</summary>
        SEWER1,
        /// <summary>Enemy Sewer2</summary>
        SEWER2,
        /// <summary>Enemy Sewer3</summary>
        SEWER3,
        /// <summary>Enemy Lava1</summary>
        LAVA1,
        /// <summary>Enemy Lava2</summary>
        LAVA2,
        /// <summary>Enemy Lava3</summary>
        LAVA3,
        /// <summary>Enemy Forest1</summary>
        FOREST1,
        /// <summary>Enemy Forest2</summary>
        FOREST2,
        /// <summary>Enemy Forest3</summary>
        FOREST3,
        /// <summary>Speed Boost</summary>
        POWERUP_SPEED,
        /// <summary>Extra Life</summary>
        POWERUP_LIFE,
        /// <summary>Invincible</summary>
        POWERUP_INVINCIBLE,
        /// <summary>Spit</summary>
        SPIT,
        /// <summary>Egg</summary>
        EGG,
        /// <summary>Obstacle</summary>
        OBSTACLE,
        /// <summary>End</summary>
        END
    };
    #endregion

    /// <summary>
    /// Authors: James Kirk, David Konz
    /// Creation: 5.6.2007
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
        /// <summary>Class Components</summary>
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
            this.components = new Dictionary<ComponentType, ClassComponent>(1);
        }
        #endregion

        #region Public Methods
        public abstract void AddToWorld(Vector2 position);

        public abstract void RemoveFromWorld();

        public virtual void Update(GameTime gameTime)
        {
            foreach (ClassComponent component in this.components.Values)
                component.Update(gameTime);
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 offset, bool drawShadow)
        {
            PositionComponent position = this.GetPosition();
            SpriteComponent sprite = this.GetSprite();
            if (position != null && sprite != null)
                sprite.Draw(gameTime, spriteBatch, position.Position + offset, drawShadow, Vector2.Zero);
        }

        public ClassComponent GetComponent(ComponentType t)
        {
            return (ClassComponent)this.components[t];
        }

        /// <summary>Gets the AI Component</summary>
        public AIComponent GetAI()
        {
            if (this.components.ContainsKey(ComponentType.AI))
                return this.components[ComponentType.AI] as AIComponent;
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

        /// <summary>Gets the Effect Component</summary>
        public EffectComponent GetEffect()
        {
            if (this.components.ContainsKey(ComponentType.EFFECT))
                return this.components[ComponentType.EFFECT] as EffectComponent;
            else
                return null;
        }

        /// <summary>Gets the Attribute Component</summary>
        public AttributeComponent GetAttribute()
        {
            if (this.components.ContainsKey(ComponentType.ATTRIBUTE))
                return this.components[ComponentType.ATTRIBUTE] as AttributeComponent;
            else
                return null;
        }

        /// <summary>Gets the Pathfind Component</summary>
        public PathfindComponent GetPathfind()
        {
            if (this.components.ContainsKey(ComponentType.PATHFIND))
                return this.components[ComponentType.PATHFIND] as PathfindComponent;
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

        /// <summary>Sets the Sprite Component</summary>
        public SpriteComponent GetSprite()
        {
            if (this.components.ContainsKey(ComponentType.SPRITE))
                return this.components[ComponentType.SPRITE] as SpriteComponent;
            else
                return null;
        }
        #endregion

        #region Protected Methods
        /// <summary>Gets the AI Component</summary>
        protected void SetAI(AIComponent ai)
        {
            this.components[ComponentType.AI] = ai;
        }

        /// <summary>Sets the Bounding Component</summary>
        protected void SetBounding(BoundingComponent bounding)
        {
            this.components[ComponentType.BOUNDING] = bounding;
        }

        /// <summary>Sets the Effect Component</summary>
        protected void SetEffect(EffectComponent effect)
        {
            this.components[ComponentType.EFFECT] = effect;
        }

        /// <summary>Sets the Attribute Component</summary>
        protected void SetAttribute(AttributeComponent attribute)
        {
            this.components[ComponentType.ATTRIBUTE] = attribute;
        }

        /// <summary>Sets the Pathfind Component</summary>
        protected void SetPathfind(PathfindComponent pathfind)
        {
            this.components[ComponentType.PATHFIND] = pathfind;
        }

        /// <summary>Sets the Position Component</summary>
        protected void SetPosition(PositionComponent position)
        {
            this.components[ComponentType.POSITION] = position;
        }

        /// <summary>Sets the Sprite Component</summary>
        protected void SetSprite(SpriteComponent sprite)
        {
            this.components[ComponentType.SPRITE] = sprite;
        }
        #endregion
    }
}
