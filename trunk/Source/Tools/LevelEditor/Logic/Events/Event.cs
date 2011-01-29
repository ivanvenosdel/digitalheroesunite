#region Using Statements
using System;
using System.Collections;

using Microsoft.Xna.Framework;

using MonsterEscape.Logic.Actors;
#endregion

namespace MonsterEscape.Logic.Events
{
    #region Interfaces
    public enum EventType
    {
        UNKNOWN = 0,
        KILLSWITCH,

        // Actor events
        ACTOR_ADD,
        ACTOR_REMOVE,

        // Game state events
        GAME_STATE_NEW,
        GAME_STATE_EXIT,
        GAME_STATE_PAUSE,
        GAME_STATE_UNPAUSE,

        // Animation
        ANIMATION_DONE,

        END
    };
    #endregion

    /// <summary>
    /// Authors: Dave Konz, James Kirk
    /// Creation: 5.6.2007
    /// Description: Class for handling events
    /// </summary>
    public abstract class Event
    {
        #region Fields
        private EventType type;
        #endregion

        #region Properties
        public EventType EventType { get { return this.type; } }
        #endregion

        #region Constructors
        public Event(EventType type)
        {
            this.type = type;
        }
        #endregion
    }

    #region Actor Events
    /// <summary>
    /// When an Actor is Created
    /// </summary>
    public class AddActorEvent : Event
    {
        #region Fields
        private Actor actor;
        #endregion

        #region Properties
        public Actor Actor { get { return this.actor; } }
        #endregion

        #region Constructors
        public AddActorEvent(Actor actor)
            : base(EventType.ACTOR_ADD)
        {
            this.actor = actor;
        }
        #endregion
    }

    /// <summary>
    /// When an Actor is Removed
    /// </summary>
    public class RemoveActorEvent : Event
    {
        #region Fields
        private Guid actorID;
        private ActorType actorType;
        #endregion

        #region Properties
        public Guid ActorID { get { return this.actorID; } }
        public ActorType ActorType { get { return this.actorType; } }
        #endregion

        #region Constructors
        public RemoveActorEvent(Guid actorId, ActorType type)
            : base(EventType.ACTOR_REMOVE)
        {
            this.actorID = actorId;
            this.actorType = type;
        }
        #endregion
    }
    #endregion Actor Events

    #region Game State
    /// <summary>
    /// When a new game is triggered
    /// </summary>
    public class NewGameEvent : Event
    {
        public NewGameEvent()
            : base(EventType.GAME_STATE_NEW)
        {
        }
    }

    ///// <summary>
    ///// Triggered to close down the current game session
    ///// and back up to the main menu.
    ///// </summary>
    //public class ExitGameEvent : Event
    //{
    //    public ExitGameEvent()
    //        : base(EventType.GAME_STATE_EXIT)
    //    {
    //    }
    //}

    ///// <summary>
    ///// A signal to pause the game logic, but still
    ///// permit the player to perform various non-game 
    ///// tasks.
    ///// </summary>
    //public class PauseGameEvent : Event
    //{
    //    public PauseGameEvent()
    //        : base(EventType.GAME_STATE_PAUSE)
    //    {
    //    }
    //}

    ///// <summary>
    ///// A signal to pause the game logic, but still
    ///// permit the player to perform various non-game 
    ///// tasks.
    ///// </summary>
    //public class UnpauseGameEvent : Event
    //{
    //    public UnpauseGameEvent()
    //        : base(EventType.GAME_STATE_UNPAUSE)
    //    {
    //    }
    //}

    /// <summary>
    /// The final trigger to close the game down and exit the app
    /// </summary>
    public class KillSwitchEvent : Event
    {
        public KillSwitchEvent()
            : base(EventType.KILLSWITCH)
        {
        }
    }
    #endregion

    #region Animation
    /// <summary>
    /// When an Actor is Finishes its animation
    /// </summary>
    public class AnimationDoneEvent : Event
    {
        #region Fields
        private Actor actor;
        private int animation;
        #endregion

        #region Properties
        public Actor Actor { get { return this.actor; } }
        public int Animation { get { return this.animation; } }
        #endregion

        #region Constructors
        public AnimationDoneEvent(Actor actor, int anim)
            : base(EventType.ANIMATION_DONE)
        {
            this.actor = actor;
            this.animation = anim;
        }
        #endregion
    }
    #endregion
}
