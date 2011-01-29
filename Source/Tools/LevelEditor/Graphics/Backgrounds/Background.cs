using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonsterEscape.Worlds;

namespace MonsterEscape.Graphics.Backgrounds
{
    public abstract class Background
    {
        public static Rectangle FullLevelRectangle;

        public Texture2D Base { get; set; }

        public Background() { }

        public virtual void Initialize()
        {
            int w = CurrentLevel.Instance.Width * TerrainKey.TILE_SIZE + 1200;
            int h = CurrentLevel.Instance.Height * TerrainKey.TILE_SIZE + 800;
            FullLevelRectangle = new Rectangle(-600, -400, w, h);
        }

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}
