using System;
using UnityEngine;

namespace InputHandling
{
    [CreateAssetMenu(order = 0, fileName = "InputHandler", menuName = "Input/InputHandler")]
    public class InputHandler : ScriptableObject
    {
        private bool _editingRoute = false;
        private Building.Building _selectedBuilding = null;

        public Action<Building.Building> OnBuildingSelected;

        private void OnEnable()
        {
            _editingRoute = false;
            _selectedBuilding = null;
        }

        public void BuildingSelected(Building.Building building)
        {
            if(_editingRoute || building == null)
                return;

            DeselectBuilding();
            _selectedBuilding = building;
            building.Select();
            OnBuildingSelected?.Invoke(building);
        }
        
        public void DeselectBuilding()
        {
            if(_editingRoute || _selectedBuilding == null)
                return;

            _selectedBuilding.Deselect();
            _selectedBuilding = null;
        }

        public void EditingRoute()
        {
            _editingRoute = true;
        }

        public void FinishedEditingRoute()
        {
            _editingRoute = false;
        }
    }
}