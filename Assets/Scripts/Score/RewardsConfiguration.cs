using System;
using System.Collections.Generic;
using System.Linq;
using Building;
using Events;
using UnityEngine;
using Vehicles;
using Random = UnityEngine.Random;

namespace Score
{
    [CreateAssetMenu(order = 0, fileName = "RewardsConfiguration", menuName = "RewardsConfiguration")]
    public class RewardsConfiguration : ScriptableObject
    {
        [SerializeField] private RewardOptions[] _rewardOptions;
        [SerializeField] private BaseEvent _cycleFinished;
        [SerializeField] private RouteInventory _routeInventory;

        private int _cycle = 0;
        public int CurrentCycle => _cycle;

        private void OnEnable()
        {
            _cycle = 0;
            _cycleFinished.Register(SumRegister, 0);
        }

        private void SumRegister()
        {
            _cycle++;
        }

        public RewardOptions GetRewardOptions()
        {
            bool canHaveRoutes = _routeInventory.Routes.Count < _routeInventory.MaxRoutes;

            List<RewardOptions> possibleOptions = new List<RewardOptions>();

            foreach (var option in _rewardOptions)
            {
                if (!canHaveRoutes && option.IncludeRoute)
                {
                    continue;
                }

                if (_cycle < option.MinCycle)
                {
                    continue;
                }

                if (option.MaxCycle != -1 && _cycle > option.MaxCycle)
                {
                    continue;
                }
                
                possibleOptions.Add(option);
            }

            return possibleOptions[Random.Range(0, possibleOptions.Count)];
        }

        private void OnDisable()
        {
            _cycleFinished.UnRegister(SumRegister);
        }
    }

    [Serializable]
    public class RewardOptions
    {
        [SerializeField] private bool _includeRoute;
        [SerializeField] private Option[] _options;
        [SerializeField] private int _minCycle;
        [SerializeField] private int _maxCycle = -1;

        public bool IncludeRoute => _includeRoute;
        public Option[] Options => _options;
        public int MinCycle => _minCycle;
        public int MaxCycle => _maxCycle;
    }
    
    [Serializable]
    public class Option
    {
        [SerializeField] private VehicleType _vehicleType;
        [SerializeField] private int _quantity;

        public VehicleType VehicleType => _vehicleType;
        public int Quantity => _quantity;
    }
}