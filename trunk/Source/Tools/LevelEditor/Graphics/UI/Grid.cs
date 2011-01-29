#region Using Statements
using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonsterEscape.Graphics.Cameras;
using MonsterEscape.Worlds;
#endregion

namespace MonsterEscape.Graphics.UI
{
    public class Grid
    {
        #region Fields
        private SpriteBatch spriteBatch;
        private Texture2D gridTexture;
        #endregion

        #region Constructor
        public Grid()
        {
            spriteBatch = new SpriteBatch(DeviceManager.Instance.GraphicsDevice);
            gridTexture = DeviceManager.Instance.ContentManager.Load<Texture2D>("grid");
        }
        #endregion

        #region Public Methods
        public void Draw()
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, Camera.Instance.View);
            //Tiles
            for (int x = 0; x < CurrentLevel.Instance.Width; ++x)
            {
                for (int y = 0; y < CurrentLevel.Instance.Height; ++y)
                {
                    Vector2 pos = new Vector2(x * TerrainKey.TILE_SIZE, y * TerrainKey.TILE_SIZE);
                    this.spriteBatch.Draw(gridTexture, pos, Color.White);
                }
            }
            this.spriteBatch.End();
        }
        #endregion
    }
}
