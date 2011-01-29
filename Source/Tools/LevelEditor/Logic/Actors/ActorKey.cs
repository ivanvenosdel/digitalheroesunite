#region Using Statements
using System;
using System.Collections.Generic;

using MonsterEscape.Utility;
#endregion


namespace MonsterEscape.Logic.Actors
{
    /// <summary>
    /// Authors: James Kirk
    /// Creation: 9.12.2010
    /// Description: Actor Definitions
    /// </summary>
    public static class ActorKey
    {
        public static Dictionary<ActorType, ActorDefinition> MonsterTypes = new Dictionary<ActorType, ActorDefinition>();
        public static Dictionary<ActorType, ActorDefinition> EnemyTypes = new Dictionary<ActorType, ActorDefinition>();
        public static Dictionary<ActorType, ActorDefinition> PowerUpTypes = new Dictionary<ActorType, ActorDefinition>();

        public static Dictionary<int, ActorType> MonsterEnum = new Dictionary<int, ActorType>();
        public static Dictionary<int, ActorType> EnemyEnum = new Dictionary<int, ActorType>();
        public static Dictionary<int, ActorType> PowerUpEnum = new Dictionary<int, ActorType>();
        /// <summary>
        /// Loads all actor types and stores them for future use
        /// </summary>
        public static void Initialize()
        {
            ////Load Monster Types
            //ActorDefinition[] def = WinphoneUtilities.DeserializeFromXML<ActorDefinition[]>("Monsters/monsters.xml");
            //for (int i = 0; i < def.Length; ++i)
            //{
            //    MonsterTypes.Add(def[i].Type, def[i]);
            //    MonsterEnum.Add(i, def[i].Type);
            //}

            ////Load Enemy Types
            //def = WinphoneUtilities.DeserializeFromXML<ActorDefinition[]>("Enemies/enemies.xml");
            //for (int i = 0; i < def.Length; ++i)
            //{
            //    EnemyTypes.Add(def[i].Type, def[i]);
            //    EnemyEnum.Add(i, def[i].Type);
            //}

            ////Load Powerup Types
            //def = WinphoneUtilities.DeserializeFromXML<ActorDefinition[]>("Powerups/powerups.xml");
            //for (int i = 0; i < def.Length; ++i)
            //{
            //    PowerUpTypes.Add(def[i].Type, def[i]);
            //    PowerUpEnum.Add(i, def[i].Type);
            //}
        }
    }
}
