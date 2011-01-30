#if DEBUG

#region Using Statements
using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Engine.Graphics.Cameras;
using Engine.Logic.Events;
using Engine.Logic.ClassComponents;
using Engine.Logic.Input;
using Engine.World;
#endregion

namespace Engine
{
    /// <summary>
    /// Simple Debug Center
    /// </summary>
    public static class Debug
    {
        #region Fields
        private static Keys lastKey;

        /// <summary>
        /// Base
        /// </summary>
        public static bool RenderFPS = false;

        /// <summary>
        /// Graphics Core
        /// </summary>
        public static bool CameraFreeRange = false;

        /// <summary>
        /// Logic Core
        /// </summary>
        public static bool DrawBounding = false;
        #endregion

        #region Public Methods
        public static void Initialize()
        {

            InputManager.Instance.OnKeyEvent += new KeyEvent(OnKeyEvent);
        }

        public static void Update(GameTime gameTime)
        {

        }

        /// <summary>
        /// KeyEvents from the Input Manager
        /// </summary>
        /// <param name="keyboardState">The current keyboard state</param>
        public static void OnKeyEvent(KeyboardState keyboardState)
        {
            //// Allows the game to exit
            //if (keyboardState.IsKeyDown(Keys.Escape))
            //{
            //    lastKey = Keys.Escape;
            //}
            //else if (lastKey == Keys.Escape)
            //{
            //    EventManager.Instance.QueueEvent(new KillSwitchEvent());

            //    lastKey = Keys.None;
            //}

            ////////////////////////////////////////////////////////////
            //                   TOGGLES
            ////////////////////////////////////////////////////////////
            //Camera FreeRange Toggle
            if (keyboardState.IsKeyDown(Keys.F1))
            {
                lastKey = Keys.F1;
            }
            else if (lastKey == Keys.F1)
            {
                Debug.CameraFreeRange = !Debug.CameraFreeRange;
                Camera.Instance.FreeRange = Debug.CameraFreeRange;
                lastKey = Keys.None;
            }

            //Bounding Box Toggle
            if (keyboardState.IsKeyDown(Keys.F2))
            {
                lastKey = Keys.F2;
            }
            else if (lastKey == Keys.F2)
            {
                Debug.DrawBounding = !Debug.DrawBounding;
                lastKey = Keys.None;
            }

            ////////////////////////////////////////////////////////////
            //                        World
            ////////////////////////////////////////////////////////////
            if (GameWorld.Instance.Enabled)
            {
                if (keyboardState.IsKeyDown(Keys.OemComma))
                {
                    lastKey = Keys.OemComma;
                }
                else if (lastKey == Keys.OemComma && keyboardState.IsKeyUp(Keys.OemComma))
                {
                    SpriteComponent sprite = GameWorld.Instance.Hero.GetSprite();
                    int nextAnim = sprite.CurrentAnimation - 1;
                    if (nextAnim < 0)
                        nextAnim = sprite.AnimationCount - 1;
                    sprite.PlayAnimation(nextAnim, true);
                    lastKey = Keys.None;
                }

                if (keyboardState.IsKeyDown(Keys.OemPeriod))
                {
                    lastKey = Keys.OemPeriod;
                }
                else if (lastKey == Keys.OemPeriod && keyboardState.IsKeyUp(Keys.OemPeriod))
                {
                    SpriteComponent sprite = GameWorld.Instance.Hero.GetSprite();
                    int nextAnim = sprite.CurrentAnimation + 1;
                    if (nextAnim > sprite.AnimationCount - 1)
                        nextAnim = 0;
                    sprite.PlayAnimation(nextAnim, true);
                    lastKey = Keys.None;
                }
            }
        }
        #endregion
    }
}
#endif