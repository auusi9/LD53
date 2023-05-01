using System;
using System.Collections.Generic;
using Building;
using Score;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vehicles;

namespace UI.UpgradeMenu
{
    public class RewardButton : MonoBehaviour
    {
        [SerializeField] private bool _isExtraLine;
        [SerializeField] private VehicleType _vehicleType;
        [SerializeField] private TextMeshProUGUI _quantityText;
        [SerializeField] private VehicleInventory _vehicleInventory;
        [SerializeField] private RouteInventory _routeInventory;
        [SerializeField] private Button _button;

        public event Action RewardGiven;
        private int _quantity;

        public VehicleType VehicleType => _vehicleType;
        private void Start()
        {
            _button.onClick.AddListener(ButtonPressed);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(ButtonPressed);
        }

        private void ButtonPressed()
        {
            if (_isExtraLine)
            {
                _routeInventory.GiveExtraLine();
            }
            else
            {
                GiveVehicle();
            }
            
            RewardGiven?.Invoke();
        }

        private void GiveVehicle()
        {
            List<VehicleType> vehicleTypes = new List<VehicleType>();

            for (int i = 0; i < _quantity; i++)
            {
                vehicleTypes.Add(_vehicleType);
            }
            
            _vehicleInventory.AddVehicles(vehicleTypes);
        }

        public void SetReward(int quantity)
        {
            _quantityText.text = quantity.ToString();
            _quantity = quantity;
        }
    }
}