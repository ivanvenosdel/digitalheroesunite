#region Using Statements
using System.Collections.ObjectModel;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
#endregion

namespace Engine.Logic.Input
{
    /// <summary>Key Event Delegate</summary>
    public delegate void KeyEvent(KeyboardState keyboardState);
    /// <summary>Mouse Event Delegate</summary>
    public delegate void MouseEvent(MouseState current, MouseState previous, float scrollDelta);

    /// <summary>
    /// Authors: James Kirk, David Konz
    /// Creation: 5.6.2007
    /// Description: The core input handler
    /// </summary>
    public sealed class InputManager
    {
        #region Fields
        private static readonly InputManager instance = new InputManager();

        private MouseState previousMouseState;
        private MouseState currentMouseState;
        #endregion

        #region Properties
        /// <summary>Singleton</summary>
        public static InputManager Instance { get { return instance; } }
        /// <summary>Key Event Handler</summary>
        public event KeyEvent OnKeyEvent;
        /// <summary>Mouse Event Handler</summary>
        public event MouseEvent OnMouseEvent;
        #endregion

        #region Constructors
        /// <summary>Constructor</summary>
        private InputManager() { }
        #endregion

        #region Public Methods
        /// <summary>
        /// Initializes the Input Manager
        /// </summary>
        public void Initialize()
        {
            MouseState startState = Mouse.GetState();
            this.previousMouseState = startState;
            this.currentMouseState = startState;
        }

        /// <summary>Updates subscribers to mouse and keyboard events</summary>
        /// <param name="gameTime">The current update time</param>
        /// <devdoc>Called by the Logic Core</devdoc>
        public void Update(GameTime gameTime)
        {
            this.previousMouseState = this.currentMouseState;
            this.currentMouseState = Mouse.GetState();

            float scrollDelta = this.previousMouseState.ScrollWheelValue - this.currentMouseState.ScrollWheelValue;

            if (OnKeyEvent != null)
            {
                OnKeyEvent(Keyboard.GetState());
            }

            if (OnMouseEvent != null)
            {
                OnMouseEvent(this.currentMouseState, this.previousMouseState, scrollDelta);
            }
        }
        #endregion
    }
}