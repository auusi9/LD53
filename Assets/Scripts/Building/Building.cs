using System;
using System.Collections.Generic;
using InputHandling;
using Navigation;
using UnityEngine;
using Vehicles;

namespace Building
{
    public class Building : MonoBehaviour
    {
        [SerializeField] private GraphNode _thisNode;
        [SerializeField] private List<BuildingRoute> _buildingRoutes;
        [SerializeField] private BuildingRoute _buildingRoutePrefab;
        [SerializeField] private InputHandler _inputHandler;
        [SerializeField] private VehicleInventory _vehicleInventory;
        [SerializeField] private BuildingInventory _buildingInventory;
        [SerializeField] private bool _enableOnStart;

        public event Action<BuildingRoute> NewBuildingRoute;
        public event Action<BuildingRoute> BuildingRouteDestroyed;

        private bool _enabled = false;

        public bool Active => _enabled;
        
        public GraphNode Node => _thisNode;
        public List<BuildingRoute> BuildingRoutes => _buildingRoutes;

        private void Start()
        {
            _buildingInventory.AddBuilding(this);
            if(_enableOnStart)
                _buildingInventory.EnableBuilding(this);
        }

        private void OnDestroy()
        {
            _buildingInventory.RemoveBuilding(this);
        }

        public void OnMouseDown()
        {
            if(!_enabled)
                return;
            
            _inputHandler.BuildingSelected(this);
        }

        public BuildingRoute CreateNewRoute()
        {
            BuildingRoute buildingRoute = Instantiate(_buildingRoutePrefab, transform);
            buildingRoute.SetBuilding(this);
            
            _buildingRoutes.Add(buildingRoute);
            buildingRoute.SelectPath();
            NewBuildingRoute?.Invoke(buildingRoute);
            _vehicleInventory.NewBuildingRoute(buildingRoute);

            return buildingRoute;
        }

        public void DestroyRoute(BuildingRoute route)
        {
            BuildingRouteDestroyed?.Invoke(route);
            _vehicleInventory.BuildingRouteDestroyed(route);
            Destroy(route.gameObject);
        }

        public void Enabled()
        {
            _enabled = true;
        }
    }
}