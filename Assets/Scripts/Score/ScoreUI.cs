using System;
using TMPro;
using UnityEngine;

namespace Score
{
    public class ScoreUI : MonoBehaviour
    {
        [SerializeField] private ScoreSystem _scoreSystem;
        [SerializeField] private TextMeshProUGUI _textMeshProUGUI;

        private void Update()
        {
            _textMeshProUGUI.text = _scoreSystem.Score.ToString();
        }
    }
}