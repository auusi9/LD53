using System;
using UnityEngine;
using Utils;

namespace Building
{
    public class RouteSpawner : MonoBehaviour
    {
        [SerializeField] private BuildingRoute _buildingRoutePrefab;
        [SerializeField] private RouteInventory _routeInventory;

        private void Start()
        {
            _routeInventory.SetSpawner(this);
        }

        public BuildingRoute GetBuildingRoute()
        {
            return Instantiate(_buildingRoutePrefab, transform);
        }
    }
}