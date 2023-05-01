using System;
using System.Collections.Generic;
using System.Text;
using Building;
using TMPro;
using UnityEngine;
using Vehicles;

namespace UI
{
    public class VehiclesUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textMeshProUGUI;
        [SerializeField] private VehicleInventory _vehicleInventory;
        [SerializeField] private VehicleType _vehicleType;

        public void Start()
        {
            UpdateText();
            _vehicleInventory.VehiclesUpdated += UpdateText;
        }

        private void OnDestroy()
        {
            _vehicleInventory.VehiclesUpdated -= UpdateText;
        }

        private void UpdateText()
        {
            StringBuilder stringBuilder = new StringBuilder();
            
            int availableVehicles = _vehicleInventory.AvailableVehicles[_vehicleType].Count;
            int totalVehicles = _vehicleInventory.TotalVehicles[_vehicleType];

            _textMeshProUGUI.text = stringBuilder.Append(availableVehicles).Append("/").Append(totalVehicles).ToString();
        }
    }
}