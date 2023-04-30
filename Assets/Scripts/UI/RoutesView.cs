using System;
using System.Collections.Generic;
using Building;
using UnityEngine;

namespace UI
{
    public class RoutesView : MonoBehaviour
    {
        [SerializeField] private RouteInventory _routeInventory;
        [SerializeField] private List<BuildingRouteView> _buildingRouteViews;
        [SerializeField] private BuildingRouteView _buildingRouteViewPrefab;

        private void Start()
        {
            _routeInventory.RoutesUpdated += RoutesUpdated;
            RoutesUpdated();
        }

        private void OnDestroy()
        {
            _routeInventory.RoutesUpdated -= RoutesUpdated;
        }

        private void RoutesUpdated()
        {
            List<BuildingRoute> buildingRoute = _routeInventory.Routes;

            int moreViews = _buildingRouteViews.Count - _routeInventory.TotalRoutes;
            
            for (int i = 0; i < moreViews; i++)
            {
                BuildingRouteView viewToRemove = _buildingRouteViews[^(i + 1)];
                viewToRemove.gameObject.SetActive(false);
            }
    
            while (_buildingRouteViews.Count < _routeInventory.TotalRoutes)
            {
                BuildingRouteView newView = Instantiate(_buildingRouteViewPrefab, transform);
                _buildingRouteViews.Add(newView);
            }
    
            for (int i = 0; i < _buildingRouteViews.Count; i++)
            {
                _buildingRouteViews[i].gameObject.SetActive(true);

                if (i < buildingRoute.Count)
                {
                    _buildingRouteViews[i].UpdateRoute(buildingRoute[i]);
                }
                else
                {
                    _buildingRouteViews[i].UpdateRoute(null);
                }
            }
        }
    }
}