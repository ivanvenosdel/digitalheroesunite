#region Using Statements
using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
#endregion

namespace Engine.Utilities
{
    /// <summary>
    /// Authors: James Kirk
    /// Creation: 1.23.2011
    /// Description: Math Utilities
    /// </summary>
    public static class UtilityMath
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
        /// <param name="min">Minimum Range</param>
        /// <param name="max">Maximum Range Inclusive</param>
        public static float RandomFloat(float min, float max)
        {
            return MathHelper.Lerp(min, max, (float)random.NextDouble());
        }

        /// <summary>
        /// Helper for picking a random number inside the specified range.
        /// [min, max]
        /// </summary>
        /// <param name="rand">A specific random number generator</param>
        /// <param name="min">Minimum Range</param>
        /// <param name="max">Maximum Range Inclusive</param>
        public static float RandomFloat(Random rand, float min, float max)
        {
            return MathHelper.Lerp(min, max, (float)rand.NextDouble());
        }

        /// <summary>
        /// Helper for picking a random number inside the specified range.
        /// [min, max]
        /// </summary>
        /// <param name="min">Minimum Range</param>
        /// <param name="max">Maximum Range Inclusive</param>
        public static int RandomInt(int min, int max)
        {
            return (int)Math.Floor(MathHelper.Lerp(min, max + 1, (float)random.NextDouble()));
        }

        /// <summary>
        /// Helper for picking a random number inside the specified range.
        /// [min, max]
        /// </summary>
        /// <param name="rand">A specific random number generator</param>
        /// <param name="min">Minimum Range</param>
        /// <param name="max">Maximum Range Inclusive</param>
        public static int RandomInt(Random rand, int min, int max)
        {
            return (int)Math.Floor(MathHelper.Lerp(min, max + 1, (float)rand.NextDouble()));
        }
        #endregion

        #region Private Methods
        #endregion
    }
}
