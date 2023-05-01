using System;
using System.Collections.Generic;
using System.Linq;
using Building;
using InputHandling;
using TMPro;
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
        [SerializeField] private TextMeshProUGUI _personCountText;
        [SerializeField] private TextMeshProUGUI _scooterCountText;
        [SerializeField] private TextMeshProUGUI _vanCountText;
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
            _vehicleInventory.VehiclesUpdated += VehiclesUpdated;
        }

        private void VehiclesUpdated()
        {
            if(_buildingRoute == null || !_vehicleInventory.VehiclesInRoute.ContainsKey(_buildingRoute))
                return;
            
            List<Vehicle> vehicles = _vehicleInventory.VehiclesInRoute[_buildingRoute];
            _personCountText.text = vehicles.Count(x => x.VehicleType == _vehicleInventory.PersonType).ToString();
            _scooterCountText.text = vehicles.Count(x => x.VehicleType == _vehicleInventory.ScooterType).ToString();
            _vanCountText.text = vehicles.Count(x => x.VehicleType == _vehicleInventory.VanType).ToString();
        }

        private void OnDestroy()
        {
            _removeRoute.onClick.RemoveListener(ResetPath);
            _routeButton.onClick.RemoveListener(RouteClicked);
            _addPerson.VehicleButtonClicked -= AddVehicleClicked;
            _addScooter.VehicleButtonClicked -= AddVehicleClicked;
            _addVan.VehicleButtonClicked -= AddVehicleClicked;
            _removePerson.VehicleButtonClicked -= RemoveVehicleClicked;
            _removeScooter.VehicleButtonClicked -= RemoveVehicleClicked;
            _removeVan.VehicleButtonClicked -= RemoveVehicleClicked;
            _vehicleInventory.VehiclesUpdated -= VehiclesUpdated;
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
            _inputHandler.CancelCreatingRoute();
            _buildingRoute.RemoveRoute();
            _subMenu.SetActive(false);
            _removeRoute.gameObject.SetActive(false);
        }

        public void UpdateRoute(BuildingRoute buildingRoute)
        {
            _buildingRoute = buildingRoute;

            if (buildingRoute == null)
            {
                _background.gameObject.SetActive(false);
                _routeDisabled.SetActive(true);
                _routeEnabled.SetActive(false);
            }
            else if(buildingRoute.Available)
            {
                _routeDisabled.SetActive(false);
                _routeEnabled.SetActive(true);
                _background.color = buildingRoute.Color;
            }
            else
            {
                _routeDisabled.SetActive(false);
                _routeEnabled.SetActive(true);
                _background.color = buildingRoute.Color;
                _background.gameObject.SetActive(true);
                VehiclesUpdated();
            }
        }

        private void RouteClicked()
        {
            if (_buildingRoute == null)
            {
            }
            else if (_buildingRoute.Available)
            {
                _inputHandler.CreatingRoute(_buildingRoute);
            }
            else
            {
                _removeRoute.gameObject.SetActive(true);
                _subMenu.gameObject.SetActive(true);
            }
        }
    }
}