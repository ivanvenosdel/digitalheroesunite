#region Using Statements
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
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
        /// Converts the position from game world to the physics world
        /// </summary>
        /// <param name="position">Position in game world</param>
        /// <returns>Converted position in physics world coordinates</returns>
        public static Vector2 GameToPhysics(Vector2 position)
        {
            return position / DeviceManager.PixelsAMeter;
        }

        public static Vector2 PhysicsToGame(Vector2 position)
        {
            return position * DeviceManager.PixelsAMeter;
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

        /// <summary>
        /// Reads in objects from xml and converts them to their class
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="location"></param>
        /// <returns></returns>
        public static T DeserializeFromXML<T>(string location)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(T));
            TextReader reader = new StreamReader(TitleContainer.OpenStream(DeviceManager.Instance.Content.RootDirectory + "/" + location));
            T obj = (T)deserializer.Deserialize(reader);
            reader.Close();

            return obj;
        }
        #endregion

        #region Private Methods
        #endregion
    }
}
