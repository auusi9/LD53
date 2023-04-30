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
            _randomNumberGenerator.Add(_enabledBuildings.Count, _enabledBuildings.Count - 1);
        }

        public Building GetRandomBuilding()
        {
            return _enabledBuildings[_randomNumberGenerator.NextItem()];
        }
    }
}