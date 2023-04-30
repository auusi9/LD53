using System;
using System.Collections.Generic;
using System.Linq;
using Building;
using UnityEngine;

namespace Vehicles
{
    [CreateAssetMenu(order = 0, fileName = "Vehicle", menuName = "Vehicle/VehicleInventory")]
    public class VehicleInventory : ScriptableObject
    {
        [SerializeField] private List<VehicleType> _vehicleTypes;
        [SerializeField] private Vehicle _vehiclePrefab;

        private Dictionary<VehicleType, List<Vehicle>> _vehicles = new Dictionary<VehicleType, List<Vehicle>>();
        private Dictionary<BuildingRoute, List<Vehicle>> _vehiclesInRoute = new Dictionary<BuildingRoute, List<Vehicle>>();

        public Dictionary<VehicleType, List<Vehicle>> AvailableVehicles => _vehicles;
        public Dictionary<BuildingRoute, List<Vehicle>> VehiclesInRoute => _vehiclesInRoute;

        private Transform _transform;

        public event Action VehiclesUpdated;

        private void OnEnable()
        {
            _vehicles.Clear();
            _vehiclesInRoute.Clear();
            
            foreach (var vehicleType in _vehicleTypes)
            {
                _vehicles.Add(vehicleType, new List<Vehicle>());
            }
        }

        public void AddVehicles(List<VehicleType> vehicleTypes)
        {
            foreach (var startingVehicle in vehicleTypes)
            {
                _vehicles[startingVehicle].Add(Instantiate(_vehiclePrefab, _transform));
                _vehicles[startingVehicle][^1].SetType(startingVehicle);
            }
        }
        
        private void RouteReseted(BuildingRoute obj)
        {
            if (_vehiclesInRoute.ContainsKey(obj))
            {
                foreach (var vehicle in _vehiclesInRoute[obj])
                {
                    vehicle.StopMoving();
                }
            }
        }

        private void NewPathCreated(BuildingRoute obj)
        {
            List<Vector3> path = obj.GetPath();
            if (_vehiclesInRoute.ContainsKey(obj))
            {
                foreach (var vehicle in _vehiclesInRoute[obj])
                {
                    vehicle.SetPath(path);
                    vehicle.StartMoving();
                    vehicle.SetColor(obj.Color);
                }
            }
        }

        public void BuildingRouteDestroyed(BuildingRoute buildingRoute)
        {
            if (_vehiclesInRoute.ContainsKey(buildingRoute))
            {
                foreach (var vehicle in _vehiclesInRoute[buildingRoute])
                {
                    _vehicles[vehicle.VehicleType].Add(vehicle);
                }

                _vehiclesInRoute.Remove(buildingRoute);
            }
            
            buildingRoute.NewRouteCreated -= NewPathCreated;
            buildingRoute.RouteReseted -= RouteReseted;
            VehiclesUpdated?.Invoke();
        }

        public void NewBuildingRoute(BuildingRoute route)
        {
            _vehiclesInRoute.Add(route, new List<Vehicle>());
            route.NewRouteCreated += NewPathCreated;
            route.RouteReseted += RouteReseted;
        }

        public void AddVehicleTo(BuildingRoute route, VehicleType vehicleType)
        {
            if (_vehiclesInRoute.ContainsKey(route))
            {
                List<Vehicle> vehicles = AvailableVehicles[vehicleType];
                if (vehicles.Count > 0)
                {
                    var vehicle = vehicles[^1];
                    vehicles.RemoveAt(vehicles.Count - 1);
                    _vehiclesInRoute[route].Add(vehicle);
                    vehicle.gameObject.SetActive(true);

                    if (route.IsActive)
                    {
                        vehicle.SetPath(route.GetPath());
                        vehicle.StartMoving();
                        vehicle.SetColor(route.Color);
                    }
                }
                VehiclesUpdated?.Invoke();
            }
        }

        public void RemoveVehicleFrom(BuildingRoute route, VehicleType vehicleType)
        {
            if (_vehiclesInRoute.ContainsKey(route))
            {
                var vehicle = _vehiclesInRoute[route].FirstOrDefault(x => x.VehicleType == vehicleType);
                if (vehicle != null)
                {
                    _vehiclesInRoute[route].Remove(vehicle);
                    vehicle.StopMoving();
                    vehicle.gameObject.SetActive(false);
                }
                VehiclesUpdated?.Invoke();
            }
        }

        public void SetSpawnTransform(Transform transform)
        {
            _transform = transform;
        }
    }
}