#region Using Statements
using System;
using System.Xml;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
#endregion

namespace MonsterEscape.Worlds
{
    /// <summary>
    /// Authors: James Kirk
    /// Creation: 5.22.2010
    /// Description: Tile Surface Types
    /// </summary>
    public enum Surfaces
    {
        EMPTY = 0,
        NORMAL,
        IMPASSIBLE
    }

    /// <summary>
    /// Authors: James Kirk
    /// Creation: 6.12.2010
    /// Description: Terrain Type Defitions
    /// </summary>
    public class TerrainType
    {
        #region Properties
        public int ID;
        public Surfaces Surface;
        public Point Position;
        #endregion
    }
}
