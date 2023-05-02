using System;
using UnityEngine;

namespace Utils
{
    public class TutorialManager : MonoBehaviour
    {
        [SerializeField] private int _maxSteps = 3;
        private static TutorialManager _tutorialManager;

        private int _step;
        public static TutorialManager Get() => _tutorialManager;

        public Action<int> StepFinished;

        public bool TutorialActive => _step < _maxSteps;
        public int CurrentStep => _step;

        private void Awake()
        {
            if (_tutorialManager == null)
            {
                _tutorialManager = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void NextStep()
        {
            _step++;
            StepFinished?.Invoke(_step);
        }
    }
}