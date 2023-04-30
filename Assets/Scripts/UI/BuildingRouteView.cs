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
        [SerializeField] private Button _createRoute;
        [SerializeField] private Button _removeRoute;
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
            _createRoute.onClick.AddListener(CreateRoute);
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
            _createRoute.onClick.RemoveListener(ResetPath);
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

        public void UpdateRoute(BuildingRoute buildingRoute)
        {
            _buildingRoute = buildingRoute;
            if (buildingRoute == null)
            {
                _createRoute.gameObject.SetActive(true);
                _background.gameObject.SetActive(false);
            }
            else
            {
                _background.color = buildingRoute.Color;
                _createRoute.gameObject.SetActive(false);
                _background.gameObject.SetActive(true);
            }
        }

        private void CreateRoute()
        {
            if (_buildingRoute == null)
            {
                _inputHandler.CreatingRoute();
            }
        }
    }
}