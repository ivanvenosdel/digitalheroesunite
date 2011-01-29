#region Using Statements
using Microsoft.Xna.Framework;

using MonsterEscape.Utility;
#endregion

namespace MonsterEscape.Worlds
{
    /// <summary>
    /// Authors: James Kirk
    /// Creation: 7.03.2010
    /// Description: World Defition
    /// </summary>
    public class World
    {
        #region Properties
        public int ID;
        public string Name;
        public int Levels;
        public int[] PuzzleTiles;
        public int[] Spires;
        #endregion

        #region Public Methods
        public int GetRandomSpire()
        {
           int i = WinphoneMath.RandomInt(0, -1);
           return Spires[i];
        }
        #endregion
    }
}
