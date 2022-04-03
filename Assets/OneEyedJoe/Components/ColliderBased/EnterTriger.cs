using OneEyedJoe.Utils;
using UnityEngine;

namespace OneEyedJoe.Components.ColliderBased
{
    public class EnterTriger : MonoBehaviour
    {
        [SerializeField] private string _tag = "Untagged";
        [SerializeField] private LayerMask _layer = ~0;
        [SerializeField] private EnterEvent _action;

        private void OnTriggerEnter2D(Collider2D collision)
        {
        //    if (!string.IsNullOrEmpty(_tag))
        //    {
        //        if (collision.gameObject.CompareTag(_tag))
        //            _action?.Invoke(collision.gameObject);
        //    }
        //    else
        //    {
        //        if(collision.gameObject.IsInLayer(_layer))
        //            _action?.Invoke(collision.gameObject);
        //    }
            
            if (!collision.gameObject.IsInLayer(_layer)) return;

            if (!string.IsNullOrEmpty(_tag) && !collision.gameObject.CompareTag(_tag)) return;

            _action?.Invoke(collision.gameObject);
        }
    }
}


