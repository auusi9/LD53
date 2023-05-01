using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vehicles
{
    public class VehicleSpawner : MonoBehaviour
    {
        [SerializeField] private List<VehicleType> _startingVehicles;
        [SerializeField] private VehicleInventory _vehicleInventory;

        private void Start()
        {
            _vehicleInventory.SetSpawnTransform(transform);
            _vehicleInventory.AddVehicles(_startingVehicles);
        }
    }
}