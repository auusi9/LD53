using UnityEngine;

namespace Navigation
{
    public class GraphSubNode : MonoBehaviour
    {
        private bool _available = true;
        public void Fill()
        {
            _available = false;
        }

        public void Empty()
        {
            _available = true;
        }

        public bool Available => _available;
    }
}