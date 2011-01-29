#region Using Statements
using System;
using System.Collections;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonsterEscape.Logic.Input;
using MonsterEscape.Worlds;
#endregion

namespace MonsterEscape.Graphics.Cameras
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

        private Matrix view;
        private Matrix projection;
        private Vector2 position;
        private Vector2 goToPosition;
        private float goToWeight;
        #endregion

        #region Properties
        /// <summary>Singleton</summary>
        public static Camera Instance { get { return instance; } }
        /// <summary>Position</summary>
        public Vector2 Position { get { return this.position; } }
        /// <summary>Position</summary>
        public Matrix View { get { return this.view; } }
        /// <summary>Projection</summary>
        public Matrix Projection { get { return this.projection; } }

        #endregion

        #region Constructors
        public Camera()
        {
            this.position = Vector2.Zero;
            this.goToPosition = Vector2.Zero;

#if DEBUG
            InputManager.Instance.OnKeyEvent += new KeyEvent(this.OnKeyEvent);
#endif
        }
        #endregion

        #region Public Methods
        /// <summary>Initializes</summary>
        public void Initialize()
        {
            CreateView();
            this.projection = Matrix.CreateOrthographic(DeviceManager.SCREEN_WIDTH, DeviceManager.SCREEN_HEIGHT, 0, 1);
        }

        public void Update(GameTime gameTime)
        {
            float precision = 0.1f;

            if (this.goToPosition != Vector2.Zero)
            {
                this.position.X = MathHelper.SmoothStep(this.position.X, this.goToPosition.X, this.goToWeight);
                this.position.Y = MathHelper.SmoothStep(this.position.Y, this.goToPosition.Y, this.goToWeight);

                this.goToWeight = MathHelper.Clamp(this.goToWeight + precision, 0, 1);
                CreateView();

                if (this.goToWeight == 1)
                    this.goToPosition = Vector2.Zero;
            }
        }

        /// <summary>Centers the Camera on a passed tile location</summary>
        public void GoToTile(Point tile)
        {
            GoToTile(tile.X, tile.Y);
        }

        public void GoToTile(int x, int y)
        {
            if (x < 0)
                x = 0;
            else if (x > CurrentLevel.Instance.Width - 1)
                x = CurrentLevel.Instance.Width - 1;

            if (y < 0)
                y = 0;
            else if (y > CurrentLevel.Instance.Height - 1)
                y = CurrentLevel.Instance.Height - 1;

            Jump(x * TerrainKey.TILE_SIZE + TerrainKey.HALF_TILE_SIZE,
                 y * TerrainKey.TILE_SIZE + TerrainKey.HALF_TILE_SIZE);
        }

        /// <summary>Is the point on screen (within bufferzone)</summary>
        public bool OnScreen(Point point)
        {
            if ((this.position.X - DeviceManager.HALF_SCREEN_WIDTH <= point.X && this.position.X + DeviceManager.HALF_SCREEN_WIDTH >= point.X) &&
                (this.position.Y - DeviceManager.HALF_SCREEN_HEIGHT <= point.Y && this.position.Y + DeviceManager.HALF_SCREEN_HEIGHT >= point.Y))
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
            if (OnScreen(new Point(LeftX, TopY)) || OnScreen(new Point(LeftX, BottomY)) ||
                OnScreen(new Point(RightX, TopY)) || OnScreen(new Point(RightX, BottomY)))
                return true;

            return false;
        }

#if DEBUG
        /// <summary>Handles key events from InputManager</summary>
        /// <param name="keyboardState">Keyboard State</param>
        public void OnKeyEvent(KeyboardState keyboardState)
        {
            //Reset Camera
            if (keyboardState.IsKeyDown(Keys.Space))
            {
                GoToTile(CurrentLevel.Instance.EggPoint);
            }

            //Increase Speed
            int additionalSpeed = 1;
            if (keyboardState.IsKeyDown(Keys.LeftShift) ||
                keyboardState.IsKeyDown(Keys.RightShift))
            {
                additionalSpeed = 4;
            }

            //Translate Left
            if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
            {
                StrafeX(8 * additionalSpeed);
            }
            //Translate Right
            else if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
            {
                StrafeX(-8 * additionalSpeed);
            }

            //Translate Down
            if (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down))
            {
                StrafeY(8 * additionalSpeed);
            }
            //Translate Up
            else if (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up))
            {
                StrafeY(-8 * additionalSpeed);
            }
        }
#endif
        #endregion

        #region Private Methods
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

        /// <summary>Jumps to X/Y</summary>
        private void Jump(int X, int Y)
        {
            this.position.X = X;
            this.position.Y = Y;
            this.goToPosition = Vector2.Zero;

            CreateView();
        }

        /// <summary>Don't let the camera look off world</summary>
        private void ConstrainToWorld()
        {
            bool changedX = false;
            bool changedY = false;

            int halfScreenWidth = DeviceManager.HALF_SCREEN_WIDTH;
            int halfScreenHeight = DeviceManager.HALF_SCREEN_HEIGHT;

            int minWidth = halfScreenWidth - BUFFER_SPACE;
            int minHeight = halfScreenHeight - BUFFER_SPACE;
            int maxWidth = (CurrentLevel.Instance.Width * TerrainKey.TILE_SIZE) - halfScreenWidth + BUFFER_SPACE;
            int maxHeight = (CurrentLevel.Instance.Height * TerrainKey.TILE_SIZE) - halfScreenHeight + BUFFER_SPACE;
            
            //SanityCheck
            if (maxWidth <= halfScreenWidth)
                minWidth = halfScreenWidth / 2;
            if (maxHeight <= halfScreenHeight)
                minHeight = halfScreenHeight / 2;
            if (maxWidth < minWidth)
                maxWidth = minWidth;
            if (maxHeight < minHeight)
                maxHeight = minHeight;

            //Constrain to the left edge if needed
            if (position.X < minWidth)
            {
                changedX = true;
                this.goToPosition.X = minWidth;
            }
            else if (position.X > maxWidth)//Constrain to the right edge
            {
                changedX = true;
                this.goToPosition.X = maxWidth;
            }

            //Constrain to the top edge if needed
            if (position.Y < minHeight)
            {
                changedY = true;
                this.goToPosition.Y = minHeight;
            }
            else if (position.Y > maxHeight)//Constrain to the bottom edge
            {
                changedY = true;
                this.goToPosition.Y = maxHeight;
            }

            if (changedX && !changedY)
                this.goToPosition.Y = this.position.Y;

            if (changedY && !changedX)
                this.goToPosition.X = this.position.X;

            //Saftey
            if (this.goToPosition.X == this.position.X &&
                this.goToPosition.Y == this.position.Y)
                this.goToPosition = Vector2.Zero;
        }

        /// <summary>Creates Transformation Matrix</summary>
        private void CreateView()
        {
            //Ensure we don't see off the world first
            ConstrainToWorld();

            //Create the new Transformation Matrix
            this.view = Matrix.CreateTranslation(
                                new Vector3(-this.position.X, -this.position.Y, 0)) *
                                Matrix.CreateTranslation(new Vector3(DeviceManager.HALF_SCREEN_WIDTH,
                                                                     DeviceManager.HALF_SCREEN_HEIGHT,
                                                                     0));
        }
        #endregion
    }
}