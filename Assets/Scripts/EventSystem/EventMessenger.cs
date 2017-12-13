using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.EventSystem
{
    class EventMessenger
    {
        private static EventMessenger _instance;
        private Dictionary<Event, List<IEventListener>> _listeners;

        private EventMessenger()
        {
            _listeners = new Dictionary<Event, List<IEventListener>>();
        }

        public static EventMessenger Instance()
        {
            if (_instance == null)
                _instance = new EventMessenger();

            return _instance;
        }

        public void SubscribeToEvents(IEventListener subscriber, List<Event> events)
        {
            foreach(Event subscribeEvent in events)
                SubscribeToEvent(subscriber, subscribeEvent);
        }

        public void SubscribeToEvent(IEventListener subscriber, Event subscribeEvent)
        {
            if (_listeners.ContainsKey(subscribeEvent) == false)
                _listeners[subscribeEvent] = new List<IEventListener>();

            if (_listeners[subscribeEvent].Any(x => x == subscriber))
                return;

            _listeners[subscribeEvent].Add(subscriber);
        }

        public void FireEvent(Event subscribeEvent, object eventPacket)
        {
            if (_listeners.ContainsKey(subscribeEvent) == false)
                return;

            foreach (IEventListener subscriber in _listeners[subscribeEvent])
                subscriber.ConsumeEvent(subscribeEvent, eventPacket);
        }
    }
}