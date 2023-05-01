using System;
using System.Collections.Generic;
using UnityEngine;

namespace Events
{
    [CreateAssetMenu(menuName = "Events/EmptyEvent", fileName = "EmptyEvent", order = 0)]
    public sealed class BaseEvent : ScriptableObject
    {
        private List<Action> _listeners = new List<Action>();

        private void OnEnable()
        {
            _listeners = new List<Action>();
        }

        public void Register(Action listener)
        {
            _listeners.Add(listener);
        }

        public void UnRegister(Action listener)
        {
            _listeners.Remove(listener);
        }

        public void Fire()
        {
            foreach (var listener in _listeners)
            {
                listener.Invoke();
            }
        }
    }
}