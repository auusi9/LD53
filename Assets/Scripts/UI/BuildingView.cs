using System;
using System.Collections.Generic;
using System.Linq;
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
            _createNewLine.onClick.AddListener(CreateNewLine);
            
            if(_building == null)
                gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _createNewLine.onClick.RemoveListener(CreateNewLine);
        }

        private void OnBuildingSelected(Building.Building building)
        {
            gameObject.SetActive(true);

            if (_building != null)
            {
                _building.NewBuildingRoute -= AddNewBuildingRoute;
            }
            
            _building = building;
            _building.NewBuildingRoute += AddNewBuildingRoute;

            List<BuildingRoute> buildingRoute = _building.BuildingRoutes;
            
            // If data is shorter than views, remove excess views
            while (_buildingRouteViews.Count > buildingRoute.Count)
            {
                BuildingRouteView viewToRemove = _buildingRouteViews[^1];
                viewToRemove.gameObject.SetActive(false);
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
                _buildingRouteViews[i].gameObject.SetActive(true);
                _buildingRouteViews[i].UpdateRoute(buildingRoute[i]);
            }
        }

        private void AddNewBuildingRoute(BuildingRoute buildingRoute)
        {
            BuildingRouteView routeView = GetAvailableRouteView();
            routeView.UpdateRoute(buildingRoute);
        }

        private BuildingRouteView GetAvailableRouteView()
        {
            BuildingRouteView buildingRouteView =
                _buildingRouteViews.FirstOrDefault(x => !x.gameObject.activeInHierarchy);

            if (buildingRouteView != null)
            {
                buildingRouteView.gameObject.SetActive(true);
                return buildingRouteView;
            }
            
            BuildingRouteView newView = Instantiate(_buildingRouteViewPrefab, transform);
            _buildingRouteViews.Add(newView);

            return newView;
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