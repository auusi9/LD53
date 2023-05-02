using System;
using System.Collections.Generic;
using InputHandling;
using Navigation;
using UnityEngine;
using Utils;
using Vehicles;

namespace Building
{
    public class Building : MonoBehaviour
    {
        [SerializeField] private GraphNode _thisNode;
        [SerializeField] private InputHandler _inputHandler;
        [SerializeField] private VehicleInventory _vehicleInventory;
        [SerializeField] private BuildingInventory _buildingInventory;
        [SerializeField] private bool _enableOnStart;
        [SerializeField] private GameObject _tutorial1;

        public event Action<BuildingRoute> NewBuildingRoute;
        public event Action<BuildingRoute> BuildingRouteDestroyed;

        private bool _enabled = false;

        public bool Active => _enabled;

        
        public GraphNode Node => _thisNode;

        private void Start()
        {
            _buildingInventory.AddBuilding(this);
            if(_enableOnStart)
                _buildingInventory.EnableBuilding(this);
            _enabled = _enableOnStart;
            gameObject.SetActive(_enabled);
        }

        public void RouteFinished()
        {
            if (TutorialManager.Get().TutorialActive && TutorialManager.Get().CurrentStep == 1)
            {
                TutorialManager.Get().NextStep();
            }
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

        public BuildingRoute CreateNewRoute(BuildingRoute buildingRoute)
        {
            buildingRoute.transform.SetParent(transform);
            buildingRoute.gameObject.SetActive(true);
            buildingRoute.SetBuilding(this);
            buildingRoute.SelectPath();
            NewBuildingRoute?.Invoke(buildingRoute);
            _vehicleInventory.NewBuildingRoute(buildingRoute);
            
            if (TutorialManager.Get().TutorialActive && TutorialManager.Get().CurrentStep == 0)
            {
                _tutorial1.SetActive(false);
                TutorialManager.Get().NextStep();
            }

            return buildingRoute;
        }

        public void DestroyRoute(BuildingRoute route)
        {
            BuildingRouteDestroyed?.Invoke(route);
            _vehicleInventory.BuildingRouteDestroyed(route);
        }

        public void Enabled()
        {
            _enabled = true;
            
            if (TutorialManager.Get().TutorialActive && TutorialManager.Get().CurrentStep == 0)
            {
                _tutorial1.SetActive(true);
            }
        }
    }
}