#region Using Statements
using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

using MonsterEscape.Logic.Actors;
using MonsterEscape.Logic.ClassComponents;
using MonsterEscape.Worlds;
#endregion

namespace MonsterEscape.Logic.AI
{
    public enum PathStatus
    {
        NonExistent = 0,
        Found,
        Walkable,
        Unwalkable
    }

    public enum PathDirection
    {
        Unknown = 0,
        Down,
        Up,
        Left,
        Right
    }

    public enum AIFollowing
    {
        Unknown = 0,
        None,
        AI
    }

    public enum WalkerType
    {
        Unknown = 0, 
        Ground,
        Sky,
        Water
    }

    /// <summary>
    /// Authors: James Kirk
    /// Creation: 4.21.2008
    /// Description: A* Pathfinding
    /// </summary>
    public class AStar
    {
        #region Fields
        int[] g_openList = new int[CurrentLevel.Instance.Width * CurrentLevel.Instance.Height + 2];
        int[,] g_whichList = new int[CurrentLevel.Instance.Width + 1, CurrentLevel.Instance.Height + 1];
        int[] g_openX = new int[CurrentLevel.Instance.Width * CurrentLevel.Instance.Height + 2];
        int[] g_openY = new int[CurrentLevel.Instance.Width * CurrentLevel.Instance.Height + 2];
        int[,] g_parentX = new int[CurrentLevel.Instance.Width + 1, CurrentLevel.Instance.Height + 1];
        int[,] g_parentY = new int[CurrentLevel.Instance.Width + 1, CurrentLevel.Instance.Height + 1];
        int[] g_Fcost = new int[CurrentLevel.Instance.Width * CurrentLevel.Instance.Height + 2];
        int[,] g_Gcost = new int[CurrentLevel.Instance.Width + 1, CurrentLevel.Instance.Height + 1];
        int[] g_Hcost = new int[CurrentLevel.Instance.Width * CurrentLevel.Instance.Height + 2];

        private Actor owner;
        private WalkerType walkerType;

        private int pathLocation;	
        private int[] pathBank;

        private PathStatus pathStatus = PathStatus.NonExistent;
        #endregion

        #region Properties
        public int PathLocation { get { return this.pathLocation; } set { this.pathLocation = value; } }
        public int[] PathBank { get { return this.pathBank; } set { this.pathBank = value; } }
        public PathStatus PathStatus { get { return this.pathStatus; } set { this.pathStatus = value; } }
        #endregion

        #region Constructors
        public AStar(Actor agent, WalkerType walker)
        {
            this.owner = agent;
            this.walkerType = walker;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// PathFind
        /// </summary>
        /// <param name="startPos">The starting tile</param>
        /// <param name="targetPos">The ending tile</param>
        /// <returns>Is there a path</returns>
        public PathStatus FindPath(Point start, Point target)
        {
            pathStatus = PathStatus.NonExistent;

            return pathStatus;
        }

        public Point GetPathLocation(int index)
        {
            return new Point(ReadPathX(index), ReadPathY(index));
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Reads the x coordinate of the next path step
        /// </summary>
        /// <param name="pathLocation">Index</param>
        /// <returns>Tile X</returns>
        private int ReadPathX(int pathLocation)
        {
            int x = -1;
            return x;
        }

        /// <summary>
        /// Reads the y coordinate of the next path step
        /// </summary>
        /// <param name="pathLocation">Index</param>
        /// <returns>Tile Y</returns>
        private int ReadPathY(int pathLocation)
        {
            int y = -1;
            return y;
        }
        #endregion
    }

    #region Nested Classes
    public class SearchNode
    {
        private SearchNode m_parent;
        private int m_length;
        private int m_G;
        private int m_H;

        public int Length { get { return this.m_length; } }

        SearchNode(SearchNode parent, int G, int H)
        {
            this.m_parent = parent;
            if (this.m_parent == null)
                this.m_length = 0;
            else
                this.m_length = this.m_parent.Length + 1;

            this.m_G = G;
            this.m_H = H;
        }
    }
    #endregion
}
