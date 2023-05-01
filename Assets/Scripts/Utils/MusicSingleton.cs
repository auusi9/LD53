using System;
using UnityEngine;

namespace Utils
{
    public class MusicSingleton : MonoBehaviour
    {
        private MusicSingleton _musicSingleton;

        private void Awake()
        {
            if (_musicSingleton == null)
            {
                _musicSingleton = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}