using System;
using System.Collections.Generic;
using Score;
using UnityEngine;

namespace Building
{
    public class PickUpPoint : MonoBehaviour
    {
        [SerializeField] private float _maxWaitTime = 30f;
        [SerializeField] private float _nextPackageTime = 30f;
        [SerializeField] private List<GameObject> _packages;
        [SerializeField] private PickUpPointInventory _pickUpPointInventory;
        [SerializeField] private ScoreSystem _scoreSystem;

        private float _currentTimePackageTime = 0f;
        private float _currentTimeMaxWaitTime = 0f;

        private Stack<GameObject> _enabledPackages = new Stack<GameObject>();

        public bool HasDrop => _enabledPackages.Count > 0;

        private void Start()
        {
            _pickUpPointInventory.AddPickUpPoint(this);
            gameObject.SetActive(false);

            foreach (var package in _packages)
            {
                package.SetActive(false);
            }
        }

        private void OnDestroy()
        {
            _pickUpPointInventory.RemovePickUpPoint(this);
        }

        private void Update()
        {
            if (_packages.Count == 0)
            {
                _currentTimeMaxWaitTime += Time.deltaTime;
            }
            else
            {
                _currentTimeMaxWaitTime = 0f;
            }

            if (_currentTimeMaxWaitTime >= _maxWaitTime)
            {
                _scoreSystem.PickupFailed();
            }
            
            if (_currentTimePackageTime >= _nextPackageTime)
            {
                _currentTimePackageTime = 0f;

                if (_packages.Count > 0)
                {
                    _enabledPackages.Push(_packages[0]);
                    _packages[0].SetActive(true);
                    _packages.RemoveAt(0);
                }
            }
            
            _currentTimePackageTime += Time.deltaTime;
        }

        public void DropPickup()
        {
            if (_enabledPackages.Count == 0)
            {
                return;
            }
            
            _packages.Insert(0, _enabledPackages.Pop());
            _packages[0].SetActive(false);
            _scoreSystem.PickupDropped();
        }
    }
}