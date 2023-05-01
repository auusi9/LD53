using System;
using System.Collections;
using Events;
using Score;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Building
{
    public class PickUpEnabler : MonoBehaviour
    {
        [SerializeField] private PickUpPointInventory _pickUpPointInventory;
        [SerializeField] private BuildingInventory _buildingInventory;
        [SerializeField] private RewardsConfiguration _rewardsConfiguration;
        [SerializeField] private BaseEvent _cycleFinished;
        [SerializeField] private float _firstPickupPoint = 10f;
        [SerializeField] private float _minTimePickupPoint = 10f;
        [SerializeField] private float _maxTimePickupPoint = 30f;
        [SerializeField] private float _timeToEnableBuilding = 10f;
        [SerializeField] private int _postOfficeCycle = 4;

        private float _currentTime = 0f;
        private float _timeNextPickup = 0f;
        
        private void Start()
        {
            _cycleFinished.Register(CycleFinished);
            _timeNextPickup = _firstPickupPoint;
            StartCoroutine(EnableFirstBuilding());
        }

        private IEnumerator EnableFirstBuilding()
        {
            yield return 0;
            Building building = _buildingInventory.GetRandomBuildingAll();
            _buildingInventory.EnableBuilding(building);
        }

        private void OnDestroy()
        {
            _cycleFinished.UnRegister(CycleFinished);
        }

        private void CycleFinished()
        {
            if (_rewardsConfiguration.CurrentCycle % _postOfficeCycle == 0)
            {
                StartCoroutine(EnableBuildingAfterTime());
            }
        }

        IEnumerator EnableBuildingAfterTime()
        {
            yield return new WaitForSeconds(_timeToEnableBuilding);
            _buildingInventory.EnableBuildingNextToLast();
        }

        private void Update()
        {
            if (_currentTime >= _timeNextPickup)
            {
                _pickUpPointInventory.EnableRandomPickUpPoint();
                _timeNextPickup = Random.Range(_minTimePickupPoint, _maxTimePickupPoint);
                _currentTime = 0f;
            }

            _currentTime += Time.deltaTime;
        }
    }
}