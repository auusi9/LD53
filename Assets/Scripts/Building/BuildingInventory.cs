using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Building
{
    [CreateAssetMenu(order = 0, fileName = "BuildingInventory", menuName = "Building/BuildingInventory")]
    public class BuildingInventory : ScriptableObject
    {
        private List<Building> _buildings = new List<Building>();
        private List<Building> _enabledBuildings = new List<Building>();
        private RandomNumberGenerator<int> _randomNumberGenerator = new RandomNumberGenerator<int>();

        private void OnEnable()
        {
            _buildings.Clear();
            _enabledBuildings.Clear();
            _randomNumberGenerator = new RandomNumberGenerator<int>();
        }

        public void AddBuilding(Building pickUpPoint)
        {
            if (!_buildings.Contains(pickUpPoint))
            {
                _buildings.Add(pickUpPoint);
            }
        }

        public void RemoveBuilding(Building pickUpPoint)
        {
            if (_buildings.Contains(pickUpPoint))
            {
                _buildings.Add(pickUpPoint);
            }
        }

        public void EnableBuilding(Building building)
        {
            _enabledBuildings.Add(building);
            building.Enabled();
            building.gameObject.SetActive(true);
            _randomNumberGenerator.Add(_enabledBuildings.Count, _enabledBuildings.Count - 1);
        }

        public Building GetRandomBuilding()
        {
            return _enabledBuildings[_randomNumberGenerator.NextItem()];
        }

        public Building GetRandomBuildingAll()
        {
            return _buildings[Random.Range(0, _buildings.Count)];
        }
        
        public void EnableBuildingNextToLast()
        {
            Building building;
            if (_enabledBuildings.Count == 0)
                building = _buildings[0];
            else
                building = _enabledBuildings[0];

            if (building != null)
            {
                Building toActivate = GetNearestBuilding(building.transform.position);
                
                if(toActivate)
                    EnableBuilding(toActivate);
            }
        }
        
        private Building GetNearestBuilding(Vector3 searchPosition)
        {
            Building nearestBuilding = null;
            float nearestDistance = Mathf.Infinity;

            foreach (Building building in _buildings)
            {
                if(building.Active)
                    continue;
                
                float distance = (searchPosition - building.transform.position).sqrMagnitude;

                if (distance < nearestDistance)
                {
                    nearestBuilding = building;
                    nearestDistance = distance;
                }
            }

            return nearestBuilding;
        }
    }
}