#region Using Statements
using System;
using System.Collections;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Engine.Logic.Events;
using Engine.Logic.Logger;
using Engine.Logic.ClassComponents;
#endregion

namespace Engine.Logic.Actors
{
    /// <summary>
    /// Authors: James Kirk, Dave Konz
    /// Creation: 7.25.2010
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
        private Actor Construct(Guid id, ActorType type)
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
                    case ActorType.HERO:
                        actor = new HeroActor(id);
                        break;
                    case ActorType.VORTEX:
                        actor = new VortexActor(id);
                        break;

                    case ActorType.UNKNOWN:
                    default:
                        LogManager.Instance.Alert("Unknown Actor Type", "Engine.Logic.Actors.ActorFactory.Create", 0);
                        return null;
                }
            }
            catch (OutOfMemoryException)
            {
                LogManager.Instance.Alert("Out of Memory Exception", "Engine.Logic.Actors.ActorFactory.Create", 0);
                return null;
            }

            // Add the actor type to the actor types list if the type is not already there
            if (actorTypes.Contains(actor.ActorType) == false)
            {
                actorTypes.Add(actor.ActorType);
            }

            // Add the actor to the actor table.  Create the list if it doesn't yet exist
            List<Actor> actorList = this.actorTable[actor.ActorType] as List<Actor>;
            if (actorList == null)
            {
                actorList = new List<Actor>(20);
                this.actorTable[actor.ActorType] = actorList;
            }

            actorList.Add(actor);

            // Notify listener of the new actor
            EventManager.Instance.QueueEvent(new AddActorEvent(actor));

            return actor;
        }
        #endregion Private Methods

        #region Public Methods
        /// <summary>Initializes</summary>
        public void Initialize()
        {
            for (int i = (int)ActorType.UNKNOWN + 1; i < (int)ActorType.END; ++i)
            {
                this.actorTypes.Add((ActorType)i);
                this.actorTable.Add((ActorType)i, new List<Actor>());
            }
        }

        /// <summary>
        /// Creates a fresh hero actor
        /// </summary>
        /// <returns>The created actor</returns>
        public HeroActor CreateHero(Vector2 position, Point boundingDimension)
        {
            ActorType type = ActorType.HERO;
            Guid actorID = System.Guid.NewGuid();

            //Create the Actor
            HeroActor actor = (HeroActor)this.Construct(actorID, type);
            actor.Initialize(position, boundingDimension);
            
            return actor;
        }

        public VortexActor CreateVortex(Vector2 position)
        {
            ActorType type = ActorType.VORTEX;
            Guid actorID = System.Guid.NewGuid();

            //Create the Vortex
            VortexActor actor = (VortexActor)this.Construct(actorID, type);
            actor.Initialize(position);

            return actor;
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
                for (i = 0; i < actorCount; i++)
                {
                    if ((list[i] as Actor).ActorID == actorId)
                    {
                        list[i].Components.Clear();
                        EventManager.Instance.QueueEvent(new RemoveActorEvent(list[i] as Actor));
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