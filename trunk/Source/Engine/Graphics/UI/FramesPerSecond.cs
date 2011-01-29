#region Using Statements
using System;

using Microsoft.Xna.Framework;
#endregion

namespace Engine.Graphics.UI
{
    /// <summary>
    /// Handles the frames per second calculations
    /// </summary>
    public static class FramesPerSecond
    {
        #region Fields
        private static int frameRate = 0;
        private static int frameCounter = 0;
        private static TimeSpan elapsedTime = TimeSpan.Zero;
        #endregion

        #region Properties
        public static int FPS { get { return frameRate; } }
        #endregion

        #region Public Methods
        public static void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime;
            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                frameRate = frameCounter;
                frameCounter = 0;
            }
            ++frameCounter;
        }
        #endregion
    }
}
