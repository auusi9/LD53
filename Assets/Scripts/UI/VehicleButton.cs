using System;
using UnityEngine;
using UnityEngine.UI;
using Vehicles;

namespace UI
{
    public class VehicleButton : MonoBehaviour
    {
        [SerializeField] private VehicleType _vehicleType;
        [SerializeField] private Button _button;

        public Action<VehicleType> VehicleButtonClicked;
        
        private void Start()
        {
            _button.onClick.AddListener(AddVehicle);
        }

        private void AddVehicle()
        {
            VehicleButtonClicked?.Invoke(_vehicleType);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(AddVehicle);
        }
    }
}