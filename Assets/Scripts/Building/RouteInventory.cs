using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Building
{
    [CreateAssetMenu(order = 0, fileName = "RouteInventory", menuName = "Building/RouteInventory")]
    public class RouteInventory : ScriptableObject
    {
        [SerializeField] private int _initalRoutes = 2;
        [SerializeField] private int _maxRoutes = 8;
        [SerializeField] private Color[] _colors;

        private List<BuildingRoute> _buildingRoutes = new List<BuildingRoute>();
        private RouteSpawner _routeSpawner;
        
        public event Action RoutesUpdated;
        public List<BuildingRoute> Routes => _buildingRoutes;

        
        public int MaxRoutes => _maxRoutes;

        private void OnEnable()
        {
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
            route.gameObject.SetActive(false);
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

        public Color GetColor(int i)
        {
            return _colors[i];
        }

        public void SetSpawner(RouteSpawner routeSpawner)
        {
            _routeSpawner = routeSpawner;
            for (int i = 0; i < _initalRoutes; i++)
            {
                _routeSpawner.GetBuildingRoute();
            }
        }

        public void ReturnRoute(BuildingRoute buildingRoute)
        {
            buildingRoute.transform.SetParent(_routeSpawner.transform);
            buildingRoute.gameObject.SetActive(false);
        }

        public void GiveExtraLine()
        {
            if(_buildingRoutes.Count >= _maxRoutes)
                return;

            _routeSpawner.GetBuildingRoute();
        }
    }
}