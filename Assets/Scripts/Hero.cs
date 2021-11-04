using UnityEngine;

namespace PlayerController
{
    public class Hero : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _jumpForce;
        [SerializeField] private bool _doubleJumpForbidden;

        [SerializeField] private LayerCheck _groundCheck;

        private Rigidbody2D _rigidbody;
        private Vector2 _direction;
        private Animator _animator;
        private SpriteRenderer _sprite;
        private bool _allowDoubleJump;

        private static readonly int IsGroundKey = Animator.StringToHash("is-ground");
        private static readonly int VerticalVelosityKey = Animator.StringToHash("vertical-velosity");
        private static readonly int IsRunningKey = Animator.StringToHash("is-running");

        public static int _money;

        private void Awake()
        {
            _money = 0;
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
            var xVelosity = _direction.x * _speed;
            var yVelosity = CalculateYVelosity();
            _rigidbody.velocity = new Vector2(xVelosity, yVelosity);

            _animator.SetBool(IsGroundKey, IsGrounded());
            _animator.SetFloat(VerticalVelosityKey, _rigidbody.velocity.y);
            _animator.SetBool(IsRunningKey, _direction.x != 0);

            UpdateSpriteDirection();
        }

        private float CalculateYVelosity()
        {
            var yVelosity = _rigidbody.velocity.y;
            var isJumpPressing = _direction.y > 0;

            if (IsGrounded() && !_doubleJumpForbidden) _allowDoubleJump = true;

            if (isJumpPressing)
            {
                yVelosity = CalculateJumpVelosity(yVelosity);
            }
            else if (_rigidbody.velocity.y > 0)
            {
                yVelosity *= 0.5f;
            }
            return yVelosity;
        }

        private float CalculateJumpVelosity(float yVelosity)
        {
            var isFalling = _rigidbody.velocity.y <= 0.001f;
            if (!isFalling) return yVelosity;

            if (IsGrounded())
            {
                yVelosity += _jumpForce;
            }
            else if (_allowDoubleJump && !_doubleJumpForbidden)
            {
                yVelosity = _jumpForce;
                _allowDoubleJump = false;

            }

            return yVelosity;
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

        public void AddMoney(int count)
        {
            _money += count;
        }
    }
}
