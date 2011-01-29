#region Using Statements
using System.Collections.ObjectModel;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Engine.World;
#endregion

namespace Engine.Logic.Input
{
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

            //Translate Left
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                dX = -8;
            }
            //Translate Right
            else if (keyboardState.IsKeyDown(Keys.Right))
            {
                dX = 8;
            }
            
            //Translate RightControl
            if (keyboardState.IsKeyDown(Keys.RightControl))
            {
                GameWorld.Instance.hero.BeginJump(WorldTile.TILE_SIZE * 5);
            }

            GameWorld.Instance.Hero.Walk(dX);
        }
        #endregion
    }
}