#region Using Statements
using System.Collections.ObjectModel;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Engine.World;
#endregion

namespace Engine.Logic.Input
{
    enum MoveDirection
    {
        Left,
        Right,
        None
    }

    public sealed class Heromanager
    {
        #region Fields
        private static readonly Heromanager instance = new Heromanager();
        #endregion

        #region Constructors
        /// <summary>Constructor</summary>
        private Heromanager() 
        {
        }
        #endregion

        #region Properties
        public static Heromanager Instance { get { return instance; } }
        #endregion

        #region Fields
        private MoveDirection jumpDirection = MoveDirection.None;
        private MoveDirection walkDirection = MoveDirection.None;
        #endregion

        #region Public Methods
        /// <summary>
        /// Initializes the Input Manager
        /// </summary>
        public void Initialize()
        {
            InputManager.Instance.OnKeyEvent += new KeyEvent(this.OnKeyEvent);
        }
        #endregion

        #region Event Methods
        private void OnKeyEvent(KeyboardState keyboardState)
        {
            int dX = 0;

            //Translate Right
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                //It's ok to move if we are jumping in the same direction or not at all
                if (!GameWorld.Instance.hero.Jumping || this.jumpDirection == MoveDirection.Right)
                {
                    dX = 8;
                    this.walkDirection = MoveDirection.Right;
                }
            }
            //Translate Left
            else if (keyboardState.IsKeyDown(Keys.Left))
            {
                //It's ok to move if we are jumping in the same direction or not at all
                if (!GameWorld.Instance.hero.Jumping || this.jumpDirection == MoveDirection.Left)
                {
                    dX = -8;
                    this.walkDirection = MoveDirection.Left;
                }
            }
            else
            {
                this.walkDirection = MoveDirection.None;
            }
            
            //Translate RightControl
            if (keyboardState.IsKeyDown(Keys.LeftControl))
            {
                if (!GameWorld.Instance.hero.Jumping)
                {
                    this.jumpDirection = this.walkDirection;
                    GameWorld.Instance.hero.BeginJump(WorldTile.TILE_SIZE * 5);
                }
            }

            GameWorld.Instance.Hero.Walk(dX);
        }
        #endregion
    }
}