using System;
using Events;
using UnityEngine;

namespace Score
{
    [CreateAssetMenu(order = 0, fileName = "ScoreSystem", menuName = "ScoreSystem")]
    public class ScoreSystem : ScriptableObject
    {
        [SerializeField] private BaseEvent _restartGame;
        [SerializeField] private BaseEvent _gameOver;

        private int _score;
        public int Score => _score;

        private void OnEnable()
        {
            _score = 0;
            _restartGame.Register(RestartGame);
        }

        private void OnDisable()
        {
            _restartGame.UnRegister(RestartGame);
        }

        private void RestartGame()
        {
            _score = 0;
        }

        public void PickupDropped()
        {
            _score++;
        }

        public void PickupFailed()
        {
            _gameOver.Fire();
        }
    }
}