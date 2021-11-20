using OneEyedJoe.Components;
using UnityEngine;

namespace OneEyedJoe
{
    public class Hero : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _jumpForce;
        [SerializeField] private float _landingVelosity;
        [SerializeField] private float _damageJumpForce;
        [SerializeField] private bool _doubleJumpForbidden;

        [Space] [Header("Interactions")]
        [SerializeField] private float _interactionRadius;
        private Collider2D[] _interactionResult = new Collider2D[1];
        [SerializeField] private LayerMask _interactionLayer;

        [Space] [Header("Particles")]
        [SerializeField] private SpawnComponent _footStepParticles;
        [SerializeField] private SpawnComponent _jumpingParticles;
        [SerializeField] private SpawnComponent _landingParticles;
        [SerializeField] private ParticleSystem _hitParticles;

        [SerializeField] private LayerCheck _groundCheck;

        private Rigidbody2D _rigidbody;
        private Vector2 _direction;
        private Animator _animator;
        private bool _allowDoubleJump;

        private static readonly int IsGroundKey = Animator.StringToHash("is-ground");
        private static readonly int VerticalVelosityKey = Animator.StringToHash("vertical-velosity");
        private static readonly int IsRunningKey = Animator.StringToHash("is-running");
        private static readonly int Hit = Animator.StringToHash("hit");

        public static int _money;

        private void Awake()
        {
            _money = 0;
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        internal void Interact()
        {
            var size = Physics2D.OverlapCircleNonAlloc(
                transform.position,
                _interactionRadius,
                _interactionResult,
                _interactionLayer);
            for (int i = 0; i < size; i++)
            {
                var interact = _interactionResult[i].GetComponent<InteractebleComponent>();
                if (interact != null)
                {
                    interact.Interact();
                }
            }
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

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.IsInLayer(_groundCheck.CheckLayer))
            {
                var contact = collision.contacts[0];
                if (contact.relativeVelocity.y >= _landingVelosity)
                {
                    _landingParticles.Spawn();
                }
            }
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
                _jumpingParticles.Spawn();
            }
            else if (_allowDoubleJump && !_doubleJumpForbidden)
            {
                yVelosity = _jumpForce;
                _jumpingParticles.Spawn();
                _allowDoubleJump = false;
            }

            return yVelosity;
        }

        private void UpdateSpriteDirection()
        {
            if (_direction.x > 0)
            {
                transform.localScale = Vector3.one;
            }
            else if (_direction.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
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

        public void TakeDamage()
        {
            _animator.SetTrigger(Hit);
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _damageJumpForce);

            if (_money > 0)
            {
                SpawnCoins();
            }
        }

        private void SpawnCoins()
        {
            var numCounsToDispose = Mathf.Min(_money, 5);
            _money -= numCounsToDispose;

            var burst = _hitParticles.emission.GetBurst(0);
            burst.count = numCounsToDispose;
            _hitParticles.emission.SetBurst(0, burst);

            _hitParticles.gameObject.SetActive(true);
            _hitParticles.Play();
        }

        public void SpawnFootDust()
        {
            _footStepParticles.Spawn();
        }
    }
}
