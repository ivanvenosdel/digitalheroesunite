#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonsterEscape.Logic.Input;
using MonsterEscape.Worlds;
using MonsterEscape.Utility;

using Engine.World;
#endregion

namespace MonsterEscape.Logic.Puzzle
{
    public enum PuzzlePieceTypes
    {
        STRAIGHT = 0,
        STRAIGHT_MID,
        STRAIGHT_FRONT,
        STRAIGHT_BACK,
        L
    }

    public enum PuzzlePieceRotation
    {
        DEGREES_0 = 0,
        DEGREES_90,
        DEGREES_180,
        DEGREES_270
    }

    /// <summary>
    /// The game's puzzle pieces
    /// </summary>
    public class PuzzlePiece
    {
        #region Fields
        private PuzzlePieceTypes type;
        private Point position;
        private PuzzlePieceRotation rotation;
        private bool transparent;
        private TileType[] terrainTiles = new TileType[3];
        #endregion

        #region Properties
        /// <summary> Piece Type </summary>
        public PuzzlePieceTypes Type { get { return this.type; } set { this.type = value; } }
        /// <summary> Piece Rotation </summary>
        public PuzzlePieceRotation Rotation { get { return this.rotation; } set { this.rotation = value; } }
        /// <summary> Tile Position </summary>
        public Point Position { get { return this.position; } set { this.position = value; } }
        /// <summary> True if we will just draw the piece as a UI element since it doesn't fit in the world </summary>
        public bool Transparent { get { return this.transparent; } set { this.transparent = value; } }
        #endregion

        #region Constructors
        public PuzzlePiece()
        {
            World currentWorld = TerrainKey.WorldTypes[CurrentLevel.Instance.World];

            this.terrainTiles[0] = new TileType(0, currentWorld.GetRandomSpire());
            this.terrainTiles[1] = new TileType(0, currentWorld.GetRandomSpire());
            this.terrainTiles[2] = new TileType(0, currentWorld.GetRandomSpire());
            this.position = new Point(-1, -1);
        }

        public PuzzlePiece(PuzzlePieceTypes t, PuzzlePieceRotation r, int[] types)
        {
            World currentWorld = TerrainKey.WorldTypes[CurrentLevel.Instance.World];

            this.type = t;
            this.rotation = r;
            this.terrainTiles[0] = new TileType(types[0], currentWorld.GetRandomSpire());
            this.terrainTiles[1] = new TileType(types[1], currentWorld.GetRandomSpire());
            this.terrainTiles[2] = new TileType(types[2], currentWorld.GetRandomSpire());
            this.position = new Point(-1, -1);
        }
        #endregion
    }
}
