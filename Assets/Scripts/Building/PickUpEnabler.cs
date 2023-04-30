using System;
using System.Collections;
using UnityEngine;

namespace Building
{
    public class PickUpEnabler : MonoBehaviour
    {
        [SerializeField] private PickUpPointInventory _pickUpPointInventory;

        private IEnumerator Start()
        {
            yield return 0;
            _pickUpPointInventory.EnableRandomPickUpPoint();
        }
    }
}