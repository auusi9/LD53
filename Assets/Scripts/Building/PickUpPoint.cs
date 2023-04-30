using System;
using Score;
using UnityEngine;

namespace Building
{
    public class PickUpPoint : MonoBehaviour
    {
        [SerializeField] private float _expireTime = 30f;
        [SerializeField] private PickUpPointInventory _pickUpPointInventory;
        [SerializeField] private ScoreSystem _scoreSystem;
        [SerializeField] private GameObject _notification;

        private float _currentTime = 0f;
        private bool _hasDrop;

        public bool HasDrop => _hasDrop;

        private void Start()
        {
            _pickUpPointInventory.AddPickUpPoint(this);
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _pickUpPointInventory.RemovePickUpPoint(this);
        }

        private void Update()
        {
            if (_currentTime >= _expireTime)
            {
                if (!_hasDrop)
                {
                    _scoreSystem.PickupFailed();
                }

                _currentTime = 0f;
                _hasDrop = true;
                _notification.gameObject.SetActive(true);
            }
            
            _currentTime += Time.deltaTime;
        }

        public void DropPickup()
        {
            _hasDrop = false;
            _scoreSystem.PickupDropped();
            _notification.gameObject.SetActive(false);
        }
    }
}