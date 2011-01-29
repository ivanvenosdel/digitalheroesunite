#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace MonsterEscape.Worlds
{
    /// <summary>
    /// Authors: James Kirk
    /// Creation: 7.15.2010
    /// Description: Scenery Sprite
    /// </summary>
    public class SceneryTile
    {
        #region Fields
        private int typeID;
        private Vector2 position;
        #endregion

        #region Properties
        public int SceneryTypeID { get { return this.typeID; } set { this.typeID = value; } }
        public SceneryType SceneryType { get { return TerrainKey.SceneryTypes[this.typeID]; } }
        public Vector2 Position { get { return this.position; } set { this.position = value; } }
        #endregion

        #region Constructors
        /// <summary>Constructor</summary>
        public SceneryTile() { }

        public SceneryTile(int sceneryTypeID, Vector2 pos)
        {
            this.typeID = sceneryTypeID;
            this.position = pos;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Special scaling draw method for HUD
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch, Vector2 scale)
        {
            if (this.typeID < 0)
                return;

            spriteBatch.Draw(TerrainKey.SceneryTextures[SceneryType.TextureID], this.position, this.SceneryType.Frame, Color.White, 0, this.SceneryType.Origin, scale, SpriteEffects.None, 1.0f);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (this.typeID < 0)
                return;

            spriteBatch.Draw(TerrainKey.SceneryTextures[SceneryType.TextureID], this.position, this.SceneryType.Frame, Color.White, 0, this.SceneryType.Origin, 1, SpriteEffects.None, 1.0f);
        }
        #endregion
    }
}
