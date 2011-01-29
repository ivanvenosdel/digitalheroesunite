using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Engine.World
{
    public class TileType
    {
        #region Properties
        public int ID;
        public Point TextureCoordinate;
        #endregion

        public TileType() { ID = 0; }
        public TileType(int type, int spire)
        {
            ID = type;
        }
    }
}
