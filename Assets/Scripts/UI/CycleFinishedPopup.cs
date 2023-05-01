using System;
using Events;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class CycleFinishedPopup : MonoBehaviour
    {
        [SerializeField] private TimeManager _timeManager;
        [SerializeField] private BaseEvent _cycleFinished;

        private void Start()
        {
            _cycleFinished.Register(CycleFinished);
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _cycleFinished.UnRegister(CycleFinished);
        }

        private void CycleFinished()
        {
            gameObject.SetActive(true);
        }

        private void OnEnable()
        {
            _timeManager.PauseGame(GetHashCode());
        }

        private void OnDisable()
        {
            _timeManager.ResumeGame(GetHashCode());
        }
    }
}