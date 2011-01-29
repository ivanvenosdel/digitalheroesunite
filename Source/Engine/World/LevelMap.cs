using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace Engine.World
{
    public class LevelMap
    {
        public int Level;
        public int Width;
        public int Height;
        public int[] Layout;
        public Point Start;
        public Point End;
    }
}
