using System;
using System.Collections.Generic;
using Building;
using InputHandling;
using UnityEngine;
using UnityEngine.UI;
using Vehicles;

namespace UI
{
    public class BuildingRouteView : MonoBehaviour
    {
        [SerializeField] private Image _background;
        [SerializeField] private Button _routeButton;
        [SerializeField] private Button _removeRoute;
        [SerializeField] private GameObject _subMenu;
        [SerializeField] private GameObject _routeDisabled;
        [SerializeField] private GameObject _routeEnabled;
        [SerializeField] private VehicleButton _addPerson;
        [SerializeField] private VehicleButton _addScooter;
        [SerializeField] private VehicleButton _addVan;
        [SerializeField] private VehicleButton _removePerson;
        [SerializeField] private VehicleButton _removeScooter;
        [SerializeField] private VehicleButton _removeVan;
        [SerializeField] private VehicleInventory _vehicleInventory;
        [SerializeField] private InputHandler _inputHandler;

        private BuildingRoute _buildingRoute;
        
        private void Start()
        {
            _removeRoute.onClick.AddListener(ResetPath);
            _routeButton.onClick.AddListener(RouteClicked);
            _addPerson.VehicleButtonClicked += AddVehicleClicked;
            _addScooter.VehicleButtonClicked += AddVehicleClicked;
            _addVan.VehicleButtonClicked += AddVehicleClicked;
            _removePerson.VehicleButtonClicked += RemoveVehicleClicked;
            _removeScooter.VehicleButtonClicked += RemoveVehicleClicked;
            _removeVan.VehicleButtonClicked += RemoveVehicleClicked;
        }

        private void OnDestroy()
        {
            _removeRoute.onClick.RemoveListener(ResetPath);
            _routeButton.onClick.RemoveListener(ResetPath);
            _addPerson.VehicleButtonClicked -= AddVehicleClicked;
            _addScooter.VehicleButtonClicked -= AddVehicleClicked;
            _addVan.VehicleButtonClicked -= AddVehicleClicked;
            _removePerson.VehicleButtonClicked -= RemoveVehicleClicked;
            _removeScooter.VehicleButtonClicked -= RemoveVehicleClicked;
            _removeVan.VehicleButtonClicked -= RemoveVehicleClicked;
        }

        private void AddVehicleClicked(VehicleType vehicleType)
        {
            _vehicleInventory.AddVehicleTo(_buildingRoute, vehicleType);
        }

        private void RemoveVehicleClicked(VehicleType vehicleType)
        {
            _vehicleInventory.RemoveVehicleFrom(_buildingRoute, vehicleType);
        }

        private void ResetPath()
        {
            _buildingRoute.RemoveRoute();
        }

        public void UpdateRoute(BuildingRoute buildingRoute, bool available, Color color)
        {
            _buildingRoute = buildingRoute;
            _background.color = color;

            if (buildingRoute == null && !available)
            {
                _routeButton.gameObject.SetActive(false);
                _background.gameObject.SetActive(false);
                _routeDisabled.SetActive(true);
                _routeEnabled.SetActive(false);
            }
            else if(available && buildingRoute == null)
            {
                _routeDisabled.SetActive(false);
                _routeEnabled.SetActive(true);
            }
            else
            {
                _routeDisabled.SetActive(false);
                _routeEnabled.SetActive(true);
                _background.color = buildingRoute.Color;
                _routeButton.gameObject.SetActive(true);
                _background.gameObject.SetActive(true);
            }
        }

        private void RouteClicked()
        {
            if (_buildingRoute == null)
            {
                _inputHandler.CreatingRoute();
            }
            else
            {
                _removeRoute.gameObject.SetActive(true);
                _subMenu.gameObject.SetActive(true);
            }
        }
    }
}