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

        public void Start()
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (var vehicle in _vehicleInventory.AvailableVehicles)
            {
                stringBuilder.Append($"Vehicle: {vehicle.Key.name} -> {vehicle.Value.Count}");
            }

            _textMeshProUGUI.text = stringBuilder.ToString();
        }
    }
}