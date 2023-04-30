using System;
using Building;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BuildingRouteView : MonoBehaviour
    {
        [SerializeField] private Button _updatePath;
        [SerializeField] private Button _resetButton;

        private BuildingRoute _buildingRoute;
        
        private void Start()
        {
            _updatePath.onClick.AddListener(UpdatePath);
            _resetButton.onClick.AddListener(ResetPath);
        }

        private void ResetPath()
        {
            _buildingRoute.ResetPath();
        }

        private void OnDestroy()
        {
            _updatePath.onClick.RemoveListener(UpdatePath);
            _resetButton.onClick.RemoveListener(ResetPath);
        }

        private void UpdatePath()
        {
            if(_buildingRoute == null)
                return;
            
            _buildingRoute.SelectPath();
        }

        public void UpdateRoute(BuildingRoute buildingRoute)
        {
            _buildingRoute = buildingRoute;
        }
    }
}