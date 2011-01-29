#region Using Statements
using System.Collections.ObjectModel;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
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
            InputManager.Instance.OnKeyEvent += new KeyEvent(this.OnKeyEvent);
        }
        #endregion

        #region Event Methods
        private void OnKeyEvent(KeyboardState state)
        {
            
        }
        #endregion
    }
}