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
    /// Description: Scenery Type Defitions
    /// </summary>
    public class SceneryType
    {
        #region Properties
        public int ID;
        public int TextureID;
        public Rectangle Frame;
        public Vector2 Origin;
        #endregion
    }
}
