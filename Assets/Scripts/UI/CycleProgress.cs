using System;
using Events;
using UnityEngine;

namespace UI
{
    public class CycleProgress : MonoBehaviour
    {
        [SerializeField] private float _cycleDuration = 5 * 60;
        [SerializeField] private BaseEvent _cycleFinished;

        public float Progress => _currentCycle / _cycleDuration;

        private float _currentCycle;
        
        private void Update()
        {
            _currentCycle += Time.deltaTime;

            if (_currentCycle >= _cycleDuration)
            {
                _currentCycle = 0;
                _cycleFinished.Fire();
            }
        }
    }
}