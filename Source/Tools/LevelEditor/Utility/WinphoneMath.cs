#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
#endregion

namespace MonsterEscape.Utility
{   
    /// <summary>
    /// Math Utility Class
    /// </summary>
    public static class WinphoneMath
    {
        #region Fields
        private static Random random = new Random();
        #endregion

        #region Properties
        public static Random Random { get { return random; } }
        #endregion

        #region Public Methods
        /// <summary>
        /// Helper for picking a random number inside the specified range.
        /// [min, max]
        /// </summary>
        public static float RandomFloat(float min, float max)
        {
            return MathHelper.Lerp(min, max, (float)random.NextDouble());
        }

        /// <summary>
        /// Helper for picking a random number inside the specified range.
        /// [min, max]
        /// </summary>
        public static int RandomInt(int min, int max)
        {
            return (int)Math.Floor(MathHelper.Lerp(min, max + 1, (float)random.NextDouble()));
        }

        /// <summary>
        /// Simulates a random die roll such as 1d6 or 2d8
        /// </summary>
        /// <param name="count">The number of dice</param>
        /// <param name="sides">The number of sides</param>
        /// <returns>The rolled value</returns>
        public static int Dice(int count, int sides)
        {
            int roll = 0;
            for (int i = 0; i < count; ++i)
            {
                roll += RandomInt(1, sides);
            }

            return roll;
        }
        #endregion
    }
}
