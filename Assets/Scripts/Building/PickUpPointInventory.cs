using System;
using System.Collections.Generic;
using System.Linq;
using Events;
using Score;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Building
{
    [CreateAssetMenu(order = 0, fileName = "PickUpPointInventory", menuName = "Building/PickUpPointInventory")]
    public class PickUpPointInventory : ScriptableObject
    {
        [SerializeField] private float _range = 10f;
        [SerializeField] private float _rangeXCycle = 2f;
        [SerializeField] private BuildingInventory _buildingInventory;
        [SerializeField] private RewardsConfiguration _rewardsConfiguration;
        [SerializeField] private BaseEvent _restartGame;

        private List<PickUpPoint> _allPickupPoints = new List<PickUpPoint>();
        private List<PickUpPoint> _enabledPickupPoints = new List<PickUpPoint>();

        public List<PickUpPoint> EnabledPickUpPoints => _enabledPickupPoints;

        private void OnEnable()
        {
            _allPickupPoints.Clear();
            _enabledPickupPoints.Clear();
            _restartGame.Register(RestartGame);
        }

        private void RestartGame()
        {
            _allPickupPoints.Clear();
            _enabledPickupPoints.Clear();
        }

        private void OnDisable()
        {
            _restartGame.UnRegister(RestartGame);
        }

        public void AddPickUpPoint(PickUpPoint pickUpPoint)
        {
            if (!_allPickupPoints.Contains(pickUpPoint))
            {
                _allPickupPoints.Add(pickUpPoint);
            }
        }

        public void RemovePickUpPoint(PickUpPoint pickUpPoint)
        {
            if (_allPickupPoints.Contains(pickUpPoint))
            {
                _allPickupPoints.Add(pickUpPoint);
            }
        }

        public void EnableRandomPickUpPoint()
        {
            Building building = _buildingInventory.GetRandomBuilding();
            float totalRange = _range + (_rangeXCycle * _rewardsConfiguration.CurrentCycle);
            PickUpPoint[] pickUpPoints = _allPickupPoints.Where(x => !x.gameObject.activeInHierarchy && (building.transform.position - x.transform.position).sqrMagnitude < totalRange).ToArray();

            if (pickUpPoints.Length > 0)
            {
                PickUpPoint pickUpPoint = pickUpPoints[Random.Range(0, pickUpPoints.Length)];
                pickUpPoint.gameObject.SetActive(true);
                _enabledPickupPoints.Add(pickUpPoint);
            }
        }
    }
}