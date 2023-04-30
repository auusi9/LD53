using System;
using System.Collections.Generic;
using InputHandling;
using Navigation;
using UnityEngine;

namespace Building
{
    public class Building : MonoBehaviour
    {
        [SerializeField] private GraphNode _thisNode;
        [SerializeField] private List<BuildingRoute> _buildingRoutes;
        [SerializeField] private BuildingRoute _buildingRoutePrefab;
        [SerializeField] private InputHandler _inputHandler;
        
        private bool _selected = false;

        public GraphNode Node => _thisNode;
        public List<BuildingRoute> BuildingRoutes => _buildingRoutes;

        public void Select()
        {
            _selected = true;
        }

        public void Deselect()
        {
            _selected = false;
        }

        public void OnMouseDown()
        {
            if(_selected)
                return;
            
            _inputHandler.BuildingSelected(this);
        }

        public void CreateNewRoute()
        {
            BuildingRoute buildingRoute = Instantiate(_buildingRoutePrefab, transform);
            buildingRoute.SetBuilding(this);
            
            _buildingRoutes.Add(buildingRoute);
        }
    }
}