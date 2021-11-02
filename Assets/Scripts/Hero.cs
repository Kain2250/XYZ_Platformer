using UnityEngine;

namespace PlayerController
{
    public class Hero : MonoBehaviour
    {
        [SerializeField] private float _speed;

        private Vector2 _direction;

        public void SetDirection(Vector2 direction)
        {
            _direction = direction;
        }

        public void Update()
        {
            if (_direction != Vector2.zero)
            {
                var speed = _speed * Time.deltaTime;
                var deltaX = _direction.x * speed;
                var deltaY = _direction.y * speed;
                var newXPosition = transform.position.x + deltaX;
                var newYPosition = transform.position.y + deltaY;
                transform.position = new Vector3(newXPosition, newYPosition, transform.position.z);
            }
        }

        public void SaySomething()
        {
            {
                Debug.Log("Fire");
            }
        }
    }
}
