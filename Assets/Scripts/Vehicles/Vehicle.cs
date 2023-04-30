using System;
using System.Collections.Generic;
using System.Linq;
using Building;
using UnityEngine;

namespace Vehicles
{
    public class Vehicle : MonoBehaviour
    {
        [SerializeField] private VehicleType _vehicleType;
        [SerializeField] private PickUpPointInventory _pickUpPointInventory;
        [SerializeField] private SpriteRenderer _icon;
        [SerializeField] private SpriteRenderer _background;
        [SerializeField] private float _minDistance = 0.001f;
        [SerializeField] private float _pickupDistance = 1f;
        [SerializeField] private float _checkTime = 0.5f;

        private List<Vector3> _path = new List<Vector3>();
        private int _currentTargetIndex = 0;
        private bool _moving = false;
        private float _currentTime = 0f;
        private int _currentCargo = 0;

        public VehicleType VehicleType => _vehicleType;

        public event Action PathFinished;

        private void Awake()
        {
            if (_vehicleType != null)
            {
                _icon.sprite = _vehicleType.Sprite;
            }
        }

        public void SetType(VehicleType vehicleType)
        {
            _vehicleType = vehicleType;
            _icon.sprite = vehicleType.Sprite;
        }

        public void SetColor(Color color)
        {
            _background.color = color;
        }

        public void SetPath(List<Vector3> path)
        {
            _path = path;
        }

        public void StopMoving()
        {
            _moving = false;
        }

        public void StartMoving()
        {
            _moving = true;
            transform.position = _path[0];
            _currentCargo = _vehicleType.CargoSpace;
        }

        private void Update()
        {
            if(_path.Count == 0 || !_moving)
                return;

            if (_checkTime <= _currentTime && _currentCargo > 0)
            {
                var pickUpPoints = _pickUpPointInventory.EnabledPickUpPoints.Where(x => x.HasDrop &&
                    (transform.position - x.transform.position).sqrMagnitude < _pickupDistance).ToArray();

                foreach (var pickUpPoint in pickUpPoints)
                {
                    if (_currentCargo > 0)
                    {
                        pickUpPoint.DropPickup();
                        _currentCargo--;
                    }
                }

                _currentTime = 0f;
            }

            _currentTime += Time.deltaTime;

            transform.position += (_path[_currentTargetIndex] - transform.position).normalized * _vehicleType.Speed *
                                  Time.deltaTime;
            
            if (Vector3.Distance(transform.position, _path[_currentTargetIndex]) < _minDistance)
            {
                _currentTargetIndex++;

                if (_currentTargetIndex >= _path.Count)
                {
                    _currentTargetIndex = 0;
                    _currentCargo = _vehicleType.CargoSpace;
                    PathFinished?.Invoke();
                }
            }
        }
    }
}