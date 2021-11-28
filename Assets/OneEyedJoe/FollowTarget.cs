using UnityEngine;

namespace OneEyedJoe
{
    public class FollowTarget : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _damping;

        private void LateUpdate()
        {
            var position = transform.position;
            var position1 = _target.position;
            var direction = new Vector3(position1.x, position1.y, position.z);
            position = Vector3.Lerp(position, direction, Time.deltaTime * _damping);
            transform.position = position;
        }
    }

}

