using System;
using Building;
using UnityEngine;

namespace InputHandling
{
    [CreateAssetMenu(order = 0, fileName = "InputHandler", menuName = "Input/InputHandler")]
    public class InputHandler : ScriptableObject
    {
        private bool _creatingRoute = false;
        private BuildingRoute _editingRoute = null;
        
        private void OnEnable()
        {
            _creatingRoute = false;
            _editingRoute = null;
        }

        public void BuildingSelected(Building.Building building)
        {
            if (_creatingRoute)
            {
                _editingRoute = building.CreateNewRoute();
            }
        }

        public void CancelCreatingRoute()
        {
            if (_editingRoute)
            {
                _editingRoute.RemoveRoute();
            }
        }
        
        public void CreatingRoute()
        {
            _creatingRoute = true;
        }

        public void RouteCreated()
        {
            _creatingRoute = false;
            _editingRoute = null;
        }
    }
}