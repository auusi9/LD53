using System;
using Building;
using UnityEngine;

namespace InputHandling
{
    [CreateAssetMenu(order = 0, fileName = "InputHandler", menuName = "Input/InputHandler")]
    public class InputHandler : ScriptableObject
    {
        [SerializeField] private RouteInventory _routeInventory;
        private bool _creatingRoute = false;
        private BuildingRoute _editingRoute = null;
        private bool _routeCreated = false;
        
        private void OnEnable()
        {
            _creatingRoute = false;
            _editingRoute = null;
        }

        public void BuildingSelected(Building.Building building)
        {
            if (_creatingRoute && _editingRoute != null && !_routeCreated)
            {
                building.CreateNewRoute(_editingRoute);
                _routeCreated = true;
            }
        }

        public void CancelCreatingRoute()
        {
            if (_editingRoute)
            {
                _editingRoute.RemoveRoute();
                _editingRoute = null;
            }
        }
        
        public void CreatingRoute(BuildingRoute buildingRoute)
        {
            if(_editingRoute != null)
                CancelCreatingRoute();
            
            _creatingRoute = true;
            _editingRoute = buildingRoute;
            _routeCreated = false;
        }

        public void RouteCreated()
        {
            _creatingRoute = false;
            _editingRoute = null;
        }
    }
}