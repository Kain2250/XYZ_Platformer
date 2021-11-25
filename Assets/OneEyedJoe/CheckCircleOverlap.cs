using System;
using System.Linq;
using OneEyedJoe.Utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace OneEyedJoe
{
    public class CheckCircleOverlap : MonoBehaviour
    {
        [SerializeField] private float _radius = 1f;
        [SerializeField] private LayerMask _layer;
        [SerializeField] private string[] _tags;
        [SerializeField] private OnOverlapEvent _onOverlap;
        
        private readonly Collider2D[] _interactionResult = new Collider2D[10];

        public void Check()
        {
            var size = Physics2D.OverlapCircleNonAlloc(
                transform.position,
                _radius,
                _interactionResult,
                _layer);

            for (var i = 0; i < size; i++)
            {
                var overlapResult = _interactionResult[i];
                var isInTag = _tags.Any(currentTag => overlapResult.CompareTag(currentTag));
                if (isInTag)
                {
                    _onOverlap?.Invoke(overlapResult.gameObject);
                }
            }
        }

        [Serializable]
        public class OnOverlapEvent : UnityEvent<GameObject>
        {
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Handles.color = HandlesUtils.TransparentRed;
            Handles.DrawSolidDisc(transform.position, Vector3.forward, _radius);
        }
#endif
    }
}

