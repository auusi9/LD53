using System;
using UnityEngine;

namespace Score
{
    [CreateAssetMenu(order = 0, fileName = "ScoreSystem", menuName = "ScoreSystem")]
    public class ScoreSystem : ScriptableObject
    {
        private int _score;
        private int _failedPickups;
        public int Score => _score;

        private void OnEnable()
        {
            _score = 0;
        }

        public void PickupDropped()
        {
            _score++;
        }

        public void PickupFailed()
        {
            _failedPickups++;
        }
    }
}