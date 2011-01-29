#region Using Statements
using System;
using System.Collections;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using MonsterEscape.Logic;
using MonsterEscape.Logic.Actors;
using MonsterEscape.Logic.Actors.Enemies;
using MonsterEscape.Logic.Actors.Items;
using MonsterEscape.Logic.Actors.Misc;
using MonsterEscape.Logic.Actors.Monsters;
using MonsterEscape.Logic.ClassComponents;
#endregion

namespace MonsterEscape.Logic.Actors
{
    /// <summary>
    /// Authors: David Konz, James Kirk
    /// Creation: 5.6.2007
    /// Description: Factory Pattern for generating Actors
    /// </summary>
    public sealed class ActorFactory
    {
        #region Fields
        private static readonly ActorFactory instance = new ActorFactory();

        private Dictionary<ActorType, List<Actor>> actorTable;
        private List<ActorType> actorTypes;
        private int actorCount;
        #endregion

        #region Properties
        /// <summary>Singleton</summary>
        public static ActorFactory Instance { get { return instance; } }
        #endregion

        #region Constructors
        private ActorFactory()
        {
            this.actorCount = 0;
            this.actorTable = new Dictionary<ActorType, List<Actor>>((int)ActorType.END);
            this.actorTypes = new List<ActorType>((int)ActorType.END);
        }
        #endregion

        #region Private Methods
        private Actor Construct(ActorType type, Guid id)
        {
            //Increment Factory Stats
            ++this.actorCount;

            //Resize Capacity in large chunks if needed

            Actor actor;

            try
            {
                //Set Actor Components and Add Actor to List
                switch (type)
                {
                    case ActorType.MONSTER_TANGY:
                        actor = new MonsterTangy(id);
                        break;
                    case ActorType.MONSTER_PAUNCH:
                        actor = new MonsterPaunch(id);
                        break;
                    case ActorType.MONSTER_NIMBLETON:
                        actor = new MonsterNimbletonActor(id);
                        break;
                    case ActorType.MONSTER_CY:
                        actor = new MonsterCyActor(id);
                        break;

                    case ActorType.CITY1:
                        actor = new City1Actor(id);
                        break;
                    case ActorType.CITY2:
                        actor = new City2Actor(id);
                        break;
                    case ActorType.SEWER1:
                        actor = new Sewer1Actor(id);
                        break;

                    case ActorType.POWERUP_SPEED:
                        actor = new PowerupSpeedBoostActor(id);
                        break;
                    case ActorType.POWERUP_LIFE:
                        actor = new PowerupExtraLifeActor(id);
                        break;
                    case ActorType.POWERUP_INVINCIBLE:
                        actor = new PowerupInvincibleActor(id);
                        break;

                    case ActorType.SPIT:
                        actor = new SpitActor(id);
                        break;

                    case ActorType.EGG:
                        actor = new EggActor(id);
                        break;
                    case ActorType.OBSTACLE:
                        actor = new ObstacleActor(id);
                        break;

                    case ActorType.UNKNOWN:
                    default:
                       // LogManager.Instance.Alert("Unknown Actor Type", "MonsterEscape.Logic.Actors.ActorFactory.Create", 0);
                        return null;
                }
            }
            catch (OutOfMemoryException)
            {
               // LogManager.Instance.Alert("Out of Memory Exception", "MonsterEscape.Logic.Actors.ActorFactory.Create", 0);
                return null;
            }

            // Add the actor to the actor table.  Create the list if it
            // doesn't yet exist
            List<Actor> actorList = this.actorTable[actor.ActorType] as List<Actor>;
            if (actorList == null)
            {
                actorList = new List<Actor>(20);
                this.actorTable[actor.ActorType] = actorList;
            }

            actorList.Add(actor);

            return actor;
        }
        #endregion Private Methods

        #region Public Methods

        public void Initialize()
        {
            for (int i = (int)ActorType.UNKNOWN + 1; i < (int)ActorType.END; ++i)
            {
                this.actorTypes.Add((ActorType)i);
                this.actorTable.Add((ActorType)i, new List<Actor>());
            }
        }

        public Actor Create(ActorType type)
        {
            Guid actorID = System.Guid.NewGuid();

            return Construct(type, actorID);
        }


        ///<summary>
        /// Returns the actor with the given type and ID
        ///</summary>
        ///<param name="type">The actor type to search through</param>
        ///<param name="actorID">The actor ID of the requested actor.</param>
        ///<returns>The actor with the given type and ID, or null if the actor was not found</returns>
        public Actor GetActor(ActorType type, Guid actorID)
        {
            List<Actor> list = this.actorTable[type] as List<Actor>;

            foreach (Actor actor in list)
            {
                if (actor.ActorID == actorID)
                {
                    return actor;
                }
            }

            return null;
        }

        /// <summary>
        /// Removes the actor from the actor table
        /// </summary>
        /// <param name="actorId">the unique id</param>
        /// <returns>success</returns>
        public bool RemoveActor(Guid actorId)
        {
            int actorCount = 0;
            int i = 0;

            // Search all actor types to find actor with actorId and remove it
            foreach (ActorType t in actorTypes)
            {
                List<Actor> list = this.actorTable[t] as List<Actor>;

                actorCount = list.Count;
                for( i = 0; i < actorCount; i++ )
                {
                    if ((list[i] as Actor).ActorID == actorId)
                    {
                        list.RemoveAt(i);
                        return true;    // actor found and removed
                    }
                }
            }

            return false;   // actor was not found
        }
        #endregion
    }
}