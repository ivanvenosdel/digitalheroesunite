#region Using Statements
using System;
using System.Collections;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Engine.Logic.Actors;
using Engine.Logic.Audio;
using Engine.Logic.Events;
using Engine.Logic.Input;
using Engine.Logic.Logger;
using Engine.World;
using Engine.Utilities;
#endregion

namespace Engine.Logic 
{
    /// <summary>
    /// Authors: James Kirk
    /// Creation: 7.25.2010
    /// Description: The Logic Core
    /// </summary>
    public class LogicCore : GameComponent
    {
        #region Fields
        private bool killSwitch;
        #endregion

        #region Properties
        /// <summary>Game kill switch</summary>
        public bool KillSwitch { get { return killSwitch; } }
        #endregion

        #region Constructors
        /// <summary>Constructor</summary>
        /// <param name="game">The XNA game object</param>
        public LogicCore(Game game)
           : base(game)
        {
            this.killSwitch = false;

            //Events
            EventManager.Instance.AddListener(new EventListener(HandleEvents), EventType.ACTOR_ADD);
            EventManager.Instance.AddListener(new EventListener(HandleEvents), EventType.ACTOR_REMOVE);
            EventManager.Instance.AddListener(new EventListener(HandleEvents), EventType.GAME_PAUSE);
            EventManager.Instance.AddListener(new EventListener(HandleEvents), EventType.GAME_UNPAUSE);
            EventManager.Instance.AddListener(new EventListener(HandleEvents), EventType.KILLSWITCH);
        }
        #endregion

        #region Public Methods
        /// <summary>Initializes Logic Core</summary>
        public override void Initialize()
        {
            InputManager.Instance.Initialize();
            EventManager.Instance.Initialize();
            ActorFactory.Instance.Initialize();
            SoundManager.Instance.Initialize();
            WorldTypes.Initialize();

            Heromanager.Instance.Initialize();

#if DEBUG
            Debug.Initialize();
#endif
            base.Initialize();
        }

        /// <summary>Updates Logic and its components</summary>
        /// <param name="gameTime">The current update time</param>
        public override void Update(GameTime gameTime)
        {
            InputManager.Instance.Update(gameTime);
            EventManager.Instance.Update(gameTime);
            SoundManager.Instance.Update(gameTime);

            
            //Portions that shouldn't run when the game is paused
            if (!DeviceManager.Instance.Paused)
            {
                GameWorld.Instance.Update(gameTime);
            }
#if DEBUG
            Debug.Update(gameTime);
#endif
        }
        #endregion

        #region Private Methods
        /// <summary>Logic event handler</summary>
        /// <param name="evt">The event being passed in</param>
        private void HandleEvents(Event evt)
        {
            switch (evt.EventType)
            {
                case EventType.ACTOR_ADD:
                    ProcessAddActorEvent(evt as AddActorEvent);
                    break;

                case EventType.ACTOR_REMOVE:
                    ProcessRemoveActorEvent(evt as RemoveActorEvent);
                    break;

                case EventType.GAME_PAUSE:
                    DeviceManager.Instance.Paused = true;
                    break;

                case EventType.GAME_UNPAUSE:
                    DeviceManager.Instance.Paused = false;
                    break;

                case EventType.KILLSWITCH:
                    this.killSwitch = true;
                    break;

                case EventType.UNKNOWN:
                default:
                    LogManager.Instance.Alert("Unknown Event Type", "Engine.Logic.HandleEvent", 0);
                    break;
            }
        }

        private void ProcessAddActorEvent(AddActorEvent evt)
        {
            //Add the actor to the world
            evt.Actor.AddToWorld();
        }

        private void ProcessRemoveActorEvent(RemoveActorEvent evt)
        {
            //Remove the actor from the world
            evt.Actor.RemoveFromWorld();
        }
        #endregion
    }
}
