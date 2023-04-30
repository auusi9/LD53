using System;
using System.Collections.Generic;
using Building;
using InputHandling;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BuildingView : MonoBehaviour
    {
        [SerializeField] private InputHandler _inputHandler;
        [SerializeField] private BuildingRouteView _buildingRouteViewPrefab;
        [SerializeField] private List<BuildingRouteView> _buildingRouteViews;
        [SerializeField] private Button _createNewLine;
        

        private Building.Building _building = null;
        
        private void Start()
        {
            _inputHandler.OnBuildingSelected += OnBuildingSelected;
            _createNewLine.onClick.AddListener(CreateNewLine);
            
            if(_building == null)
                gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _inputHandler.OnBuildingSelected -= OnBuildingSelected;
            _createNewLine.onClick.RemoveListener(CreateNewLine);
        }

        private void OnBuildingSelected(Building.Building building)
        {
            gameObject.SetActive(true);
            _building = building;

            List<BuildingRoute> buildingRoute = _building.BuildingRoutes;
            
            // If data is shorter than views, remove excess views
            while (_buildingRouteViews.Count > buildingRoute.Count)
            {
                BuildingRouteView viewToRemove = _buildingRouteViews[_buildingRouteViews.Count - 1];
                _buildingRouteViews.RemoveAt(_buildingRouteViews.Count - 1);
                Destroy(viewToRemove.gameObject);
            }
    
            // If data is longer than views, spawn new views
            while (_buildingRouteViews.Count < buildingRoute.Count)
            {
                BuildingRouteView newView = Instantiate(_buildingRouteViewPrefab, transform);
                _buildingRouteViews.Add(newView);
            }
    
            // Update views with data
            for (int i = 0; i < buildingRoute.Count; i++)
            {
                _buildingRouteViews[i].UpdateRoute(buildingRoute[i]);
            }
        }

        private void CreateNewLine()
        {
            if (_building != null)
            {
                _building.CreateNewRoute();
            }
        }
    }
}