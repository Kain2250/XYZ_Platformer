using UnityEngine;
using System;
using UnityEngine.Events;

namespace OneEyedJoe.Components
{
    public class EnterTriger : MonoBehaviour
    {
        [SerializeField] private string _tag;
        [SerializeField] private EnterEvent _action;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(_tag))
            {
                _action?.Invoke(collision.gameObject);
            }
        }
    }
}


