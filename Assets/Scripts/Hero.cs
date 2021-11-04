using UnityEngine;

namespace PlayerController
{
    public class Hero : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _jumpForce;

        [SerializeField] private LayerCheck _groundCheck;

        private Rigidbody2D _rigidbody;
        private Vector2 _direction;
        private Animator _animator;
        private SpriteRenderer _sprite; 

        private static readonly int IsGroundKey = Animator.StringToHash("is-ground");
        private static readonly int VerticalVelosityKey = Animator.StringToHash("vertical-velosity");
        private static readonly int IsRunningKey = Animator.StringToHash("is-running");

        public static int money;

        private void Awake()
        {
            money = 0;
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _sprite = GetComponent<SpriteRenderer>();
        }

        public void SetDirection(Vector2 direction)
        {
            _direction = direction;
        }

        public void FixedUpdate()
        {
            _rigidbody.velocity = new Vector2(_direction.x * _speed, _rigidbody.velocity.y);

            var isJumping = _direction.y > 0;
            if (isJumping)
            {
                if (IsGrounded() && _rigidbody.velocity.y <= 0.1f)
                {
                    _rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
                }
            }
            else if (_rigidbody.velocity.y > 0)
            {
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y * 0.5f);
            }

            _animator.SetBool(IsGroundKey, IsGrounded());
            _animator.SetFloat(VerticalVelosityKey, _rigidbody.velocity.y);
            _animator.SetBool(IsRunningKey, _direction.x != 0);

            UpdateSpriteDirection();
        }

        private void UpdateSpriteDirection()
        {
            if (_direction.x > 0)
            {
                _sprite.flipX = false;
            }
            else if (_direction.x < 0)
            {
                _sprite.flipX = true;
            }
        }

        private bool IsGrounded()
        {
            return _groundCheck.IsTouchingLayer;
        }

        public void SaySomething()
        {
            {
                Debug.Log("Fire");
            }
        }
    }
}
