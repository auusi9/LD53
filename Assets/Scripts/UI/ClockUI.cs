using UnityEngine;

namespace UI
{
    public class ClockUI : MonoBehaviour
    {
        [SerializeField] private CycleProgress _cycleProgress;
        [SerializeField] private RectTransform _rectTransform;
        
        private void Update()
        {
            _rectTransform.localRotation = Quaternion.Euler(0, 0, _cycleProgress.Progress * -360);
        }
    }
}