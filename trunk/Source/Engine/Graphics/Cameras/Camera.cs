#region Using Statements
using System;
using System.Collections.Generic;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Engine.Logic.Input;
#endregion

namespace Engine.Graphics.Cameras
{
    /// <summary>
    /// Authors: James Kirk
    /// Creation: 4.20.2010
    /// Description: Main Camera
    /// </summary>
    public class Camera
    {
        #region Fields
        private static readonly Camera instance = new Camera();

        public const int BUFFER_SPACE = 100;
        public const int CLIP_BUFFER = 150;
        public const int RATE = 2;

        private float zoom;
        private Matrix view;
        private Matrix projection;
        private Vector2 position;
        private Vector2 goToPosition;
        private float goToWeight;
        private Matrix zoomMatrix;
        private BoundingFrustum frustum;
        private Vector2 direction = Vector2.Zero;
#if DEBUG
        private bool freeRange = Debug.CameraFreeRange;
        public bool FreeRange { get { return this.freeRange; } set { this.freeRange = value; } }
#else 
        private bool freeRange = false;
#endif
        #endregion

        #region Properties
        /// <summary>Singleton</summary>
        public static Camera Instance { get { return instance; } }
        /// <summary>Zoom</summary>
        public float Zoom { get { return this.zoom; } }
        /// <summary>Position</summary>
        public Vector2 Position { get { return this.position; } }
        /// <summary>View Matrix</summary>
        public Matrix View { get { return this.view; } }
        /// <summary>Projection Matrix</summary>
        public Matrix Projection { get { return this.projection; } }
        public Vector2 Direction { get { return this.direction; } set { this.direction = value; } }
        #endregion

        #region Constructors
        public Camera()
        {
            InputManager.Instance.OnMouseEvent += new MouseEvent(this.OnMouseEvent);
            InputManager.Instance.OnKeyEvent += new KeyEvent(this.OnKeyEvent);
        }
        #endregion

        #region Public Methods
        /// <summary>Initializes</summary>
        public void Initialize()
        {
            Reset();
        }

        /// <summary>
        /// Returns the camera to the starting position
        /// </summary>
        public void Reset()
        {
            Viewport vp = DeviceManager.Instance.GraphicsDevice.Viewport;
            this.position = Vector2.Zero;
            this.goToPosition = Vector2.Zero;
            this.zoom = 1.0f;
            this.zoomMatrix = Matrix.CreateScale(new Vector3(this.zoom, this.zoom, 0));

            this.projection = Matrix.CreateOrthographicOffCenter(0, DeviceManager.Instance.GraphicsDevice.Viewport.Width,
                                                                   DeviceManager.Instance.GraphicsDevice.Viewport.Height, 0, 0, 1);

            CreateView();
        }

        public void Update(GameTime gameTime)
        {
            float precision = 0.1f;

            if (this.goToPosition != Vector2.Zero)
            {
                this.position.X = MathHelper.SmoothStep(this.position.X, this.goToPosition.X, this.goToWeight);
                this.position.Y = MathHelper.SmoothStep(this.position.Y, this.goToPosition.Y, this.goToWeight);

                this.goToWeight = MathHelper.Clamp(this.goToWeight + precision, 0, 1);

                if (this.goToWeight == 1)
                    this.goToPosition = Vector2.Zero;
            }
            else if (!freeRange)
            {
                //this.position.X = this.Direction.X * RATE;
                //this.position.Y = this.Direction.Y * RATE;
                this.position += this.Direction * RATE;
            }
            CreateView();
        }

        /// <summary>Is the point on screen</summary>
        /// <param name="point">A point in world space</param>
        public bool OnScreen(Point point)
        {
            Viewport vp = DeviceManager.Instance.GraphicsDevice.Viewport;
            int halfScreenWidth = vp.Width / 2;
            int halfScreenHeight = vp.Height / 2;

            if ((this.position.X - halfScreenWidth - CLIP_BUFFER <= point.X && this.position.X + halfScreenWidth + CLIP_BUFFER >= point.X) &&
                (this.position.Y - halfScreenHeight - CLIP_BUFFER <= point.Y && this.position.Y + halfScreenHeight + CLIP_BUFFER >= point.Y))
                return true;

            return false;
        }

        /// <summary>Is the rectangle on screen</summary>
        /// <param name="point">A rectangle in world space</param>
        public bool OnScreen(Rectangle shape)
        {
            int halfW = shape.Width / 2;
            int halfH = shape.Height / 2;

            int LeftX = shape.X - halfW;
            int RightX = shape.X + halfW;
            int TopY = shape.Y - halfH;
            int BottomY = shape.Y + halfH;
            if (OnScreen(new Point(LeftX,TopY)) || OnScreen(new Point(LeftX, BottomY)) ||
                OnScreen(new Point(RightX, TopY)) || OnScreen(new Point(RightX, BottomY)))
                return true;

            return false;
        }

        /// <summary>Handles key events from InputManager</summary>
        /// <param name="keyboardState">Keyboard State</param>
        public void OnKeyEvent(KeyboardState keyboardState)
        {
            if (this.freeRange)
            {
                //Increase Speed
                int additionalSpeed = 1;
                if (keyboardState.IsKeyDown(Keys.LeftShift) ||
                    keyboardState.IsKeyDown(Keys.RightShift))
                {
                    additionalSpeed = 4;
                }

                int dX = 0;
                int dY = 0;
                //Translate Left
                if (keyboardState.IsKeyDown(Keys.J))
                {
                    dX = -8 * additionalSpeed;
                }
                //Translate Right
                else if (keyboardState.IsKeyDown(Keys.L))
                {
                    dX = 8 * additionalSpeed;
                }

                //Translate Down
                if (keyboardState.IsKeyDown(Keys.I))
                {
                    dY = -8 * additionalSpeed;
                }
                //Translate Up
                else if (keyboardState.IsKeyDown(Keys.K))
                {
                    dY = 8 * additionalSpeed;
                }

                //Only move if needed
                if (dX != 0 || dY != 0)
                {
                    Walk(dX, dY);
                }
            }
        }

        /// <summary>Handles mouse events from InputManager</summary>
        /// <param name="buttons">List of mouse button states</param>
        /// <param name="mouseDelta">The distance moved by the mouse since last handled</param>
        /// <param name="scrollDelta">The distance scrolled by the scroll wheel since last handled</param>
        /// <param name="mousePos">The mouse cursor location on screen</param>
        public void OnMouseEvent(MouseState current, MouseState previous, float scrollDelta)
        {
            if (scrollDelta != 0)
            {
                float delta = scrollDelta /= -1200.0f;
                delta *= 2; //keep scaling in even amount to prevent artifacts

                float lastZoomLevel = this.zoom;
                this.zoom = (float)Math.Round((double)(this.zoom + delta), 2);
                this.zoom = Math.Max(0.1f, Math.Min(this.zoom, 1));

                if (this.zoom != lastZoomLevel)
                {
                    Viewport vp = DeviceManager.Instance.GraphicsDevice.Viewport;
                    this.zoomMatrix = Matrix.CreateScale(new Vector3(this.zoom, this.zoom, 0));
                    CreateView();
                }
            }
        }

        /// <summary>Jumps to X/Y</summary>
        public void Jump(float X, float Y)
        {
            this.position.X = X;
            this.position.Y = Y;
            this.goToPosition = Vector2.Zero;

            CreateView();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Toggles the free Range mode of the camera
        /// </summary>
        private void ToggleFreeRange()
        {
            freeRange = !freeRange;
        }

        /// <summary>Move only X</summary>
        private void StrafeX(float dX)
        {
            Walk(dX, 0);
        }

        /// <summary>Move only Y</summary>
        private void StrafeY(float dY)
        {
            Walk(0, dY);
        }

        /// <summary>Moves on X/Y</summary>
        private void Walk(float dX, float dY)
        {
            this.position.X += dX;
            this.position.Y += dY;

            CreateView();
        }

        /// <summary>Creates Transformation Matrix</summary>
        private void CreateView()
        {
            Viewport vp = DeviceManager.Instance.GraphicsDevice.Viewport;

            //Create the new View Matrix
            this.view = Matrix.CreateTranslation(
                                new Vector3(-this.position.X, -this.position.Y, 0)) *
                                zoomMatrix *
                                Matrix.CreateTranslation(new Vector3(vp.Width * 0.5f, vp.Height * 0.5f, 0));

            this.frustum = new BoundingFrustum(this.view * this.projection);
        }
        #endregion
    }
}