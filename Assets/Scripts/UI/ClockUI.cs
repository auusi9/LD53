using UnityEngine;
using Image = UnityEngine.UI.Image;

namespace UI
{
    public class ClockUI : MonoBehaviour
    {
        [SerializeField] private CycleProgress _cycleProgress;
        [SerializeField] private Image _fillImage;
        
        private void Update()
        {
            _fillImage.fillAmount = 1 - _cycleProgress.Progress;
        }
    }
}