using System;
using System.Collections.Generic;

namespace SkySwordKill.NextModEditor.Event
{
    public static class EventCenter
    {
        private static Dictionary<Type, IEventSubject> _events = new Dictionary<Type, IEventSubject>();

        private static IEventSubject GetOrCreateSubject<T>() where T : class, IEventArgs
        {
            if (!_events.TryGetValue(typeof(T), out var targetEvent))
            {
                targetEvent = new EventSubject<T>();
                _events.Add(typeof(T), targetEvent);
            }

            return targetEvent;
        }

        public static void Register<T>(Action<T> callback) where T : class, IEventArgs
        {
            GetOrCreateSubject<T>().Register(callback);
        }

        public static void Unregister<T>(Action<T> callback) where T : class, IEventArgs
        {
            GetOrCreateSubject<T>().Unregister(callback);
        }

        public static void Send<T>(T args) where T : class, IEventArgs
        {
            if (!_events.TryGetValue(typeof(T), out var targetEvent))
            {
                return;
            }

            targetEvent.Send(args);
        }
    }
}