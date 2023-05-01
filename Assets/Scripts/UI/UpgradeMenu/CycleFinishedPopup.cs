using Events;
using Score;
using TMPro;
using UnityEngine;
using Utils;
using Vehicles;

namespace UI.UpgradeMenu
{
    public class CycleFinishedPopup : MonoBehaviour
    {
        [SerializeField] private TimeManager _timeManager;
        [SerializeField] private BaseEvent _cycleFinished;
        [SerializeField] private RewardButton _lineReward;
        [SerializeField] private RewardButton _personReward;
        [SerializeField] private RewardButton _scooterReward;
        [SerializeField] private RewardButton _vanReward;
        [SerializeField] private TextMeshProUGUI _weekText;
        [SerializeField] private string _weekString = "Week {0}";
        [SerializeField] private RewardsConfiguration _rewardsConfiguration;
        
        private void Start()
        {
            _lineReward.RewardGiven += RewardGiven;
            _personReward.RewardGiven += RewardGiven;
            _scooterReward.RewardGiven += RewardGiven;
            _vanReward.RewardGiven += RewardGiven;
            _cycleFinished.Register(CycleFinished);
            gameObject.SetActive(false);
        }

        private void RewardGiven()
        {
            FXSingleton.Get().PlayPressButton();
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _lineReward.RewardGiven -= RewardGiven;
            _personReward.RewardGiven -= RewardGiven;
            _scooterReward.RewardGiven -= RewardGiven;
            _vanReward.RewardGiven -= RewardGiven;
            _cycleFinished.UnRegister(CycleFinished);
        }

        private void CycleFinished()
        {
            gameObject.SetActive(true);
            FXSingleton.Get().PlayAchievementSound();

            RewardOptions rewardOptions = _rewardsConfiguration.GetRewardOptions();
            
            _lineReward.gameObject.SetActive(rewardOptions.IncludeRoute);
            _personReward.gameObject.SetActive(false);
            _scooterReward.gameObject.SetActive(false);
            _vanReward.gameObject.SetActive(false);
            
            foreach (var reward in rewardOptions.Options)
            {
                if (reward.VehicleType == _personReward.VehicleType)
                {
                    _personReward.gameObject.SetActive(true);
                    _personReward.SetReward(reward.Quantity);
                }
                
                if (reward.VehicleType == _scooterReward.VehicleType)
                {
                    _scooterReward.gameObject.SetActive(true);
                    _scooterReward.SetReward(reward.Quantity);
                }
                
                if (reward.VehicleType == _vanReward.VehicleType)
                {
                    _vanReward.gameObject.SetActive(true);
                    _vanReward.SetReward(reward.Quantity);
                }
            }
        }

        private void OnEnable()
        {
            _timeManager.PauseGame(GetHashCode());
            _weekText.text = string.Format(_weekString, _rewardsConfiguration.CurrentCycle);
        }

        private void OnDisable()
        {
            _timeManager.ResumeGame(GetHashCode());
        }
    }
}