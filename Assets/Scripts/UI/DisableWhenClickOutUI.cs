using UnityEngine;

namespace UI
{
    public class DisableWhenClickOutUI: MonoBehaviour
    {
        [SerializeField] private GameObject _objectReference;
        
        private bool _stillClicked = false;
        private void OnEnable()
        {
            if (Input.GetMouseButton(0))
            {
                _stillClicked = true;
            }
        }

        private void Update()
        {
            if (Input.GetMouseButton(0) && _stillClicked)
            {
                return;
            }

            _stillClicked = false;
            HideIfClickedOutside();
        }

        private void HideIfClickedOutside() 
        {
            if (Input.GetMouseButton(0) && gameObject.activeSelf && 
                !RectTransformUtility.RectangleContainsScreenPoint(
                    _objectReference.transform as RectTransform, 
                    Input.mousePosition, 
                    null)) {
                gameObject.SetActive(false);
            }
        }
    }
}