using System;
using System.Collections.Generic;
using UnityEngine;

namespace Building
{
    [CreateAssetMenu(order = 0, fileName = "RouteInventory", menuName = "Building/RouteInventory")]
    public class RouteInventory : ScriptableObject
    {
        [SerializeField] private int _initalRoutes = 2;
        [SerializeField] private Color[] _colors;

        private List<BuildingRoute> _buildingRoutes = new List<BuildingRoute>();

        public event Action RoutesUpdated;
        public List<BuildingRoute> Routes => _buildingRoutes;

        private int _availableRoutes = 0;

        public int TotalRoutes => _buildingRoutes.Count + _availableRoutes;

        private void OnEnable()
        {
            _availableRoutes = _initalRoutes;
            _buildingRoutes.Clear();
        }

        public void AddRoute(BuildingRoute route)
        {
            if (_buildingRoutes.Contains(route))
            {
                return;
            }
            
            _buildingRoutes.Add(route);
            route.SetColor(_colors[_buildingRoutes.Count - 1]);
            RoutesUpdated?.Invoke();
        }

        public void RemoveRoute(BuildingRoute route)
        {
            if (!_buildingRoutes.Contains(route))
            {
                return;
            }
            
            _buildingRoutes.Remove(route);
            RoutesUpdated?.Invoke();
        }
    }
}