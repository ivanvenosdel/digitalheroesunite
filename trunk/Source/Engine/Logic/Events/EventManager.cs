#region Using Statements
using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
#endregion

namespace Engine.Logic.Events
{
    public delegate void EventListener(Event evt);


    /// <summary>
    /// Authors: Dave Konz, James Kirk
    /// Creation: 7.25.2010
    /// Description: Manager to handle the Events
    /// </summary>
    public sealed class EventManager
    {
        /// <summary>
        /// Helper class used to store delegates.
        /// </summary>
        private class EventListenerList : List<EventListener> { }
        
        #region Fields
        private const int MaxQueues = 2;    // Max number of queues to use for storing events to handle
        private const int QueueCapacity = 50;   // Default capacity of a queue
        private const int QueueGrowthFactor = 10;   // Factor to grow when queue capacity is reached

        private const double MaxEventHandlingTime = 1000.0;

        private static readonly EventManager instance = new EventManager();

        private List<EventType> eventTypeList;    // List of event types subscribed to by listeners
        private Dictionary<EventType, EventListenerList> listenerTable;    // Maps the list of listeners to the event type
        private List<Event>[] events;         // Holds the events added by QueueEvent

        private int curQueue;           // Selects the active queue for adding events
        #endregion

        #region Properties
        public static EventManager Instance { get { return instance; } }
        #endregion

        #region Constructors
        private EventManager()
        {
            // create two queues, both starting with 50
            this.events = new List<Event>[MaxQueues] { new List<Event>(QueueCapacity), 
                                                       new List<Event>(QueueCapacity) };
            this.eventTypeList = new List<EventType>(10);
            this.listenerTable = new Dictionary<EventType, EventListenerList>();
            this.curQueue = 0;
        }
        #endregion

        
        #region Public Methods
        /// <summary>Initializes</summary>
        public void Initialize()
        {

        }

        /// <summary>
        /// Immediately fires the event to listeners.
        /// </summary>
        /// <param name="evt">The event to fire</param>
        /// <returns>
        /// True - Success
        /// False - no listeners available
        /// </returns>
        public bool Trigger(Event evt)
        {
            // Get the listeners for the event type
            EventListenerList listeners;

            if (listenerTable.ContainsKey(evt.EventType) == false)
            {
                // No listeners for this event type
                return false;
            }

            listeners = this.listenerTable[evt.EventType];

            // Send the event to each listener
            foreach (EventListener handler in listeners)
            {
                handler(evt);
            }

            return true;
        }

        /// <summary>
        /// Aborts a queued event
        /// </summary>
        /// <param name="evt">The event to abort</param>
        /// <param name="allOfType">
        /// True - all events of same type as evt are removed
        /// False - remove the first event matching evt
        /// </param>
        /// <returns>
        /// True - removed the event(s)
        /// False - failed to remove the event
        /// </returns>
        public bool AbortEvent(Event evt, bool allOfType)
        {
            bool result = false;

            if (eventTypeList.Contains(evt.EventType) == false)
            {
                return false;
            }
            List<Event> eventQueue = this.events[this.curQueue];

            if (allOfType == true)
            {
                for (int i = 0; i < eventQueue.Count; ++i)
                {
                    if ((eventQueue[i] as Event).EventType == evt.EventType)
                    {
                        eventQueue.RemoveAt(i);
                        --i;    // Decrement for the removed element
                        result = true;
                    }
                }
            }
            else
            {
                if (eventQueue.Contains(evt))
                {
                    eventQueue.Remove(evt);
                    result = true;
                }
            }

            return result;
        }

        /// <summary>
        /// Adds an event listener so it can receive registered events
        /// </summary>
        /// <param name="handler">A delegate listening for the event</param>
        /// <param name="eventType">The type of event to listen to</param>
        /// <returns>
        /// True - added listener
        /// False - listener already registered for eventType
        /// </returns>
        public bool AddListener(EventListener handler, EventType eventType)
        {
            bool added = false;

            if (eventTypeList.Contains(eventType) == false)
            {
                eventTypeList.Add(eventType);
            }

            // Add the event type and an empty listener array to the listener 
            // table if not already added
            EventListenerList list;
            if (listenerTable.ContainsKey(eventType))
            {
                list = listenerTable[eventType];
            }
            else
            {
                // Create a new list to add event listeners for eventType
                list = new EventListenerList();
                listenerTable.Add(eventType, list);
            }

            // Check for listener already in the list, add if not
            bool registered = false;

            foreach (EventListener e in list)
            {
                if (e == handler)
                {
                    registered = true;
                    break;
                }
            }

            if (registered == false)
            {
                // Add the listener to the list for this event type
                list.Add(handler);
                added = true;
            }

            return added;
        }

        /// <summary>
        /// Adds events to the event queue
        /// </summary>
        /// <param name="evt">Event to queue</param>
        /// <returns>
        /// True - event queued
        /// False - listener not registered for event type
        /// </returns>
        public bool QueueEvent(Event evt)
        {
            if (listenerTable.ContainsKey(evt.EventType) == false)
            {
                // No listener list for this event, do not add to queue
                return false;
            }

            // Queue up the event
            this.events[this.curQueue].Add(evt);

            return true;
        }

        /// <summary>
        /// Removes an event listener from the event manager. This will usually 
        /// only get called when the game is shutting down.
        /// </summary>
        /// <param name="handler">The listening delegate to remove</param>
        /// <param name="eventType">The type of event to unregister from</param>
        /// <returns>
        /// True - removed listener
        /// False - no listeners for specified EventType
        /// </returns>
        public bool RemoveListener(EventListener handler, EventType type)
        {
            EventListenerList list;
            bool removed = false;

            if( listenerTable.ContainsKey(type) )
            {
                // Remove the listener from the listener array for the event type
                list = listenerTable[type];
                list.Remove(handler);
                removed = true;

                if (list.Count == 0)
                {
                    eventTypeList.Remove(type);  // Remove type from event type list
                    listenerTable.Remove(type);  // No more listeners, remove table entry
                }
            }

            return removed;
        }

        /// <summary>
        /// Fire events. Use the game time to limit the amount of time handling events.
        /// If too much time passes, add them to the other list of events. The amount of
        /// time spent handling events will need to be fine tuned as more events are added.
        /// </summary>
        /// <param name="gameTime">Game time at this instant</param>
        public void Update(GameTime gameTime)
        {
            // Store the time to quit processing events
            DateTime quitTime = System.DateTime.Now;
            quitTime = quitTime.AddMilliseconds(MaxEventHandlingTime);

            // Save the active queue for event processing
            List<Event> eventQueue = events[this.curQueue];

            // Change the active queue so new events are added to the 
            // queue not being processed
            this.curQueue = (this.curQueue + 1) % MaxQueues;
            
            // Make sure the new queue is clear
            this.events[this.curQueue].Clear();

            EventListenerList handlers;
            Event evt;
            // Handle as many events as possible before quitTime is reached
            while (eventQueue.Count > 0)
            {
                // Get the event and remove it from the queue
                evt = eventQueue[0] as Event;
                eventQueue.RemoveAt(0); // may want to remove range for efficiency

                // Get the listeners for the event type
                handlers = this.listenerTable[evt.EventType];
                if (handlers == null)
                {
                    // No listeners for this event type, get the
                    // next event
                    continue;
                }

                // Send the event to each handler
                foreach (EventListener handler in handlers)
                {
                    handler(evt);
                }

                // Check time quanta
                if (DateTime.Now.CompareTo(quitTime) > 0)
                {
                    // Time quanta ran out, quit processing events
                    break;
                }
            }

            // If any more events are in the queue, move them to the front of the next queue
            if (eventQueue.Count > 0)
            {
                this.events[this.curQueue].InsertRange(0, eventQueue);
            }
        }
        #endregion
    }
}
