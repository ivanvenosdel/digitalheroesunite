#region Using Statements
using System;
using System.Collections.Generic;
#endregion

namespace Engine.Utilities
{
    /// <summary>
    /// Authors: James Kirk
    /// Creation: 1.23.2011
    /// Description: Random Game Utilities
    /// </summary>
    public static class UtilityGame
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Public Methods
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
                roll += UtilityMath.RandomInt(1, sides);
            }

            return roll;
        }

        /// <summary>
        /// Simulates a random die roll such as 1d6 or 2d8
        /// </summary>
        /// <param name="rand">A specific random number generator</param>
        /// <param name="count">The number of dice</param>
        /// <param name="sides">The number of sides</param>
        /// <returns>The rolled value</returns>
        public static int Dice(Random rand, int count, int sides)
        {
            int roll = 0;
            for (int i = 0; i < count; ++i)
            {
                roll += UtilityMath.RandomInt(rand, 1, sides);
            }

            return roll;
        }
        #endregion

        #region Private Methods
        #endregion
    }
}
