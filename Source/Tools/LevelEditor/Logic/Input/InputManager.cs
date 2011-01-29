#region Using Statements
using System.Collections.ObjectModel;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using MonsterEscape.Worlds;
using MonsterEscape.Graphics.Cameras;
using MonsterEscape.Graphics.UI;
#endregion

namespace MonsterEscape.Logic.Input
{
    /// <summary>Mouse Button Types</summary>
    public enum MouseButtonTypes
    {
        /// <summary>Left</summary>
        Left,
        /// <summary>Middle</summary>
        Middle,
        /// <summary>Right</summary>
        Right,
        /// <summary>XButton1</summary>
        XButton1,
        /// <summary>XButton2</summary>
        XButton2
    }

    /// <summary>Key Event Delegate</summary>
    public delegate void KeyEvent(KeyboardState keyboardState);
    /// <summary>Mouse Event Delegate</summary>
    public delegate void MouseEvent(ButtonList buttons, Vector2 mouseDelta, float scrollDelta, Point mousePos);

    /// <summary>ButtonList</summary>
    public class ButtonList : Collection<ButtonState> { }
    /// <summary>KeyList</summary>
    public class KeyList : Collection<Keys> { }

    /// <summary>
    /// Authors: James Kirk, David Konz
    /// Creation: 5.6.2007
    /// Description: The core input handler
    /// </summary>
    public sealed class InputManager
    {
        #region Fields
        private static readonly InputManager instance = new InputManager();

        public Point lastMousePos;
        private float lastMouseScroll;
        private Vector2 mouseDelta;
        private ButtonList mouseButtons;
        private Point currentTile;
        #endregion

        #region Properties
        /// <summary>Singleton</summary>
        public static InputManager Instance { get { return instance; } }
        /// <summary>Key Event Handler</summary>
        public event KeyEvent OnKeyEvent;
        /// <summary>Mouse Event Handler</summary>
        public event MouseEvent OnMouseEvent;
        /// <summary>The clicked tile</summary>
        public Point CurrentTile { get { return this.currentTile; } }
        #endregion

        #region Constructors
        /// <summary>Constructor</summary>
        private InputManager()
        {
            this.mouseButtons = new ButtonList();

            MouseState startState = Mouse.GetState();
            this.lastMousePos = new Point(startState.X, startState.Y);
            this.lastMouseScroll = 0.0f;
            this.currentTile = Point.Zero;
        }
        #endregion

        #region Public Methods
        /// <summary>Initializes</summary>
        public void Initialize()
        {
        }

        /// <summary>Updates subscribers to mouse and keyboard events</summary>
        /// <param name="gameTime">The current update time</param>
        /// <devdoc>Called by the Logic Core</devdoc>
        public void Update(GameTime gameTime)
        {
            MouseState state = Mouse.GetState();

            this.mouseDelta = new Vector2(
                this.lastMousePos.X - state.X,
                this.lastMousePos.Y - state.Y
            );

            this.lastMousePos.X = state.X;
            this.lastMousePos.Y = state.Y;

            Vector3 position = Camera.Instance.View.Translation;
            this.currentTile.X = (int)(position.X - this.lastMousePos.X) / TerrainKey.TILE_SIZE * -1;
            this.currentTile.Y = (int)(position.Y - this.lastMousePos.Y) / TerrainKey.TILE_SIZE * -1;
            if (this.currentTile.X > CurrentLevel.Instance.Width - 1 || this.currentTile.X < 0 ||
                this.currentTile.Y > CurrentLevel.Instance.Height - 1 || this.currentTile.Y < 0)
            {
                this.currentTile.X = -1;
                this.currentTile.Y = -1;
            }

            float scrollDelta = lastMouseScroll - state.ScrollWheelValue;
            this.lastMouseScroll = state.ScrollWheelValue;

            this.mouseButtons.Clear();
            this.mouseButtons.Add(state.LeftButton);
            this.mouseButtons.Add(state.MiddleButton);
            this.mouseButtons.Add(state.RightButton);
            this.mouseButtons.Add(state.XButton1);
            this.mouseButtons.Add(state.XButton2);

            if (OnKeyEvent != null)
            {
                OnKeyEvent(Keyboard.GetState());
            }

            if (OnMouseEvent != null)
            {
                OnMouseEvent(this.mouseButtons, this.mouseDelta, scrollDelta, this.lastMousePos);
            }
        }
        #endregion
    }
}