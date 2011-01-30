using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Engine.Logic.Actors;

namespace Engine.World
{
    public class TileExtra
    {
        public int TileID { get; set; }
        public Vector2 CameraDirection { get; set; }
        public Vector2 Position { get; set; }
    }

    public class LevelTileExtras
    {
        public int LevelID { get; set; }
        public TileExtra[] TileExtras { get; set; }
    }
}
