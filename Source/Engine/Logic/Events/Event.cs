#region Using Statements
using System;
using System.Collections;

using Microsoft.Xna.Framework;

using Engine.Logic.Actors;
#endregion

namespace Engine.Logic.Events
{
    #region Interfaces
    public enum EventType
    {
        UNKNOWN = 0,
        KILLSWITCH,

        // Actor events
        ACTOR_ADD,
        ACTOR_REMOVE,

        //Game events
        GAME_PAUSE,
        GAME_UNPAUSE,

        //ClassComponents
        ANIMATION_DONE,

        END
    };
    #endregion

    /// <summary>
    /// Authors: Dave Konz, James Kirk
    /// Creation: 7.25.2010
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
        private Actor actor;
        #endregion

        #region Properties
        public Actor Actor { get { return this.actor; } }
        #endregion

        #region Constructors
        public RemoveActorEvent(Actor actor)
            : base(EventType.ACTOR_REMOVE)
        {
            this.actor = actor;
        }
        #endregion
    }
    #endregion Actor Events

    #region Game Events
    /// <summary>
    /// Pause the game
    /// </summary>
    public class PauseGameEvent : Event
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public PauseGameEvent()
            : base(EventType.GAME_PAUSE)
        {
        }
        #endregion
    }

    /// <summary>
    /// Unpause the game
    /// </summary>
    public class UnpauseGameEvent : Event
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public UnpauseGameEvent()
            : base(EventType.GAME_UNPAUSE)
        {
        }
        #endregion
    }
    #endregion

    #region ClassComponents
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
