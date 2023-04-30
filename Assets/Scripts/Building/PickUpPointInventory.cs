using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Building
{
    [CreateAssetMenu(order = 0, fileName = "PickUpPointInventory", menuName = "Building/PickUpPointInventory")]
    public class PickUpPointInventory : ScriptableObject
    {
        [SerializeField] private float _range = 10f;
        [SerializeField] private BuildingInventory _buildingInventory;

        private List<PickUpPoint> _allPickupPoints = new List<PickUpPoint>();
        private List<PickUpPoint> _enabledPickupPoints = new List<PickUpPoint>();

        public List<PickUpPoint> EnabledPickUpPoints => _enabledPickupPoints;

        private void OnEnable()
        {
            _allPickupPoints.Clear();
            _enabledPickupPoints.Clear();
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

            PickUpPoint[] pickUpPoints = _allPickupPoints.Where(x => !x.gameObject.activeInHierarchy && (building.transform.position - x.transform.position).sqrMagnitude < _range).ToArray();

            if (pickUpPoints.Length > 0)
            {
                PickUpPoint pickUpPoint = pickUpPoints[Random.Range(0, pickUpPoints.Length)];
                pickUpPoint.gameObject.SetActive(true);
                _enabledPickupPoints.Add(pickUpPoint);
            }
        }
    }
}