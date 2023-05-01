using System;
using UnityEngine;

namespace UI
{
    public class CycleProgress : MonoBehaviour
    {
        [SerializeField] private float _cycleDuration = 5 * 60;

        public float Progress => _currentCycle / _cycleDuration;

        private float _currentCycle;

        public event Action CycleFinished;

        private void Update()
        {
            _currentCycle += Time.deltaTime;

            if (_currentCycle >= _cycleDuration)
            {
                _currentCycle = 0;
                CycleFinished?.Invoke();
            }
        }
    }
}