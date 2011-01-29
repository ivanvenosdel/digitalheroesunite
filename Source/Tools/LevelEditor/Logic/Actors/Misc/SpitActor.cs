#region Using Statements
using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonsterEscape.Logic.Actors;
using MonsterEscape.Logic.ClassComponents;
using MonsterEscape.Worlds;
using MonsterEscape.Utility;
#endregion

namespace MonsterEscape.Logic.Actors.Misc
{
    /// <summary>
    /// Authors: James Kirk
    /// Creation: 10.25.2010
    /// Description: Spit Actor
    /// </summary>
    public class SpitActor : Actor
    {
        #region Fields
        private const float MOVEMENT_RATE = TerrainKey.TILE_SIZE * (1.0f / 30.0f); //A tile a second
        private Vector2 delta = Vector2.Zero;
        #endregion

        #region Properties
        public Vector2 Delta { get { return this.delta; } set { this.delta = value; } }
        public bool Die { get; set; }
        #endregion

        #region Constructors
        public SpitActor(Guid id)
            : base(id, ActorType.SPIT)
        {
            SetPosition(new PositionComponent(this));

            //Sprite is set in Initialize
        }
        #endregion

        #region Public Methods
        public void Initialize(bool greenSpit)
        {
        }

        /// <summary>
        /// Adds this actor to the currentlevel
        /// </summary>
        /// <param name="position">The world position</param>
        public override void AddToWorld(Vector2 position)
        {
            GetPosition().Position = position;
        }

        /// <summary>
        /// Removes the actor from the level and from memory
        /// </summary>
        public override void RemoveFromWorld()
        {
        }

        /// <summary>
        /// Float the object up and down by modifying its sprite component offset
        /// </summary>
        /// <param name="gameTime">The current game time</param>
        public override void Update(GameTime gameTime)
        {
            //Nothing
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 offset, bool drawShadow)
        {
            //Nothing
        }

        public void onCollision(Actor actor)
        {
            Die = true;

            RemoveFromWorld();
        }
        #endregion
    }
}
