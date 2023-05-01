using UnityEngine;

namespace Vehicles
{
    [CreateAssetMenu(order = 0, fileName = "{VEHICLE_NAME}", menuName = "Vehicle/VehicleType")]
    public class VehicleType : ScriptableObject
    {
        [SerializeField] private float _speed;
        [SerializeField] private int _cargoSpace;
        [SerializeField] private Sprite _sprite;

        public float Speed => _speed;
        public int CargoSpace => _cargoSpace;
        public Sprite Sprite => _sprite;
    }
}