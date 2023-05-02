using System;
using System.Collections.Generic;
using Score;
using UnityEngine;
using Utils;

namespace Building
{
    public class PickUpPoint : MonoBehaviour
    {
        [SerializeField] private float _maxWaitTime = 30f;
        [SerializeField] private float _nextPackageTime = 30f;
        [SerializeField] private List<GameObject> _packages;
        [SerializeField] private PickUpPointInventory _pickUpPointInventory;
        [SerializeField] private ScoreSystem _scoreSystem;
        [SerializeField] private SpriteRenderer _fillImage;
        [SerializeField] private GameObject _tutorial;

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

        private void OnEnable()
        {
            if (TutorialManager.Get().TutorialActive && TutorialManager.Get().CurrentStep == 1)
            {
                _tutorial.SetActive(true);
                TutorialManager.Get().StepFinished += ShowTutorial;
            }
            else if (TutorialManager.Get().TutorialActive && TutorialManager.Get().CurrentStep < 1)
            {
                TutorialManager.Get().StepFinished += ShowTutorial;
            }
        }

        private void ShowTutorial(int obj)
        {
            if (obj == 1)
            {
                _tutorial.SetActive(true);
            }
            else if(obj > 1)
            {
                TutorialManager.Get().StepFinished -= ShowTutorial;
                _tutorial.SetActive(false);
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
            
            _fillImage.material.SetInt("_Arc2", (int)((1 - (_currentTimeMaxWaitTime / _maxWaitTime)) * 360));
            
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