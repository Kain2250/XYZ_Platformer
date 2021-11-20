using UnityEngine;

namespace OneEyedJoe
{
    public class LayerCheck : MonoBehaviour
    {
        [SerializeField] private LayerMask _checkLayer;
        private Collider2D _collider;

        [HideInInspector] public bool IsTouchingLayer;

        public LayerMask CheckLayer { get => _checkLayer; }

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            IsTouchingLayer = _collider.IsTouchingLayers(_checkLayer);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            IsTouchingLayer = _collider.IsTouchingLayers(_checkLayer);
        }
    }
}
