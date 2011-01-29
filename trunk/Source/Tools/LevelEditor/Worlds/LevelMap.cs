#region Using Statements
using Microsoft.Xna.Framework;
using MonsterEscape.Logic.Puzzle;
#endregion

namespace MonsterEscape.Worlds
{
    /// <summary>
    /// Authors: James Kirk
    /// Creation: 6.12.2010
    /// Description: Level Defition
    /// </summary>
    public class LevelMap
    {
        #region Properties
        public string Name;
        public int World;
        public int Level;
        public int ParTime;
        public int Width;
        public int Height;
        public Point Egg;
        public Point End;
        public int[] Layout;
        public SceneryTile[] Scenery;
        public EntityMarker[] Monsters;
        public EntityMarkerEnemy[] Enemies;
        public EntityMarker[] Items;
        public PuzzlePiece[] PuzzlePieces;
        #endregion             
    }
}
