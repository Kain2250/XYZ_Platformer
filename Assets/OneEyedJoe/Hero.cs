using OneEyedJoe.Components;
using UnityEngine;

namespace OneEyedJoe
{
    public class Hero : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _jumpForce;
        [SerializeField] private float _landingVelocity;
        [SerializeField] private float _damageJumpForce;
        [SerializeField] private bool _doubleJumpForbidden;

        [Space] [Header("Interactions")]
        [SerializeField] private float _interactionRadius;
        [SerializeField] private LayerMask _interactionLayer;
        private readonly Collider2D[] _interactionResult = new Collider2D[1];

        [Space] [Header("Particles")]
        [SerializeField] private SpawnComponent _footStepParticles;
        [SerializeField] private SpawnComponent _jumpingParticles;
        [SerializeField] private SpawnComponent _landingParticles;
        [SerializeField] private ParticleSystem _hitParticles;

        [SerializeField] private LayerCheck _groundCheck;
        
        private int _money;
        [HideInInspector] public Vector2 _direction;

        private Rigidbody2D _rigidbody;
        private Animator _animator;
        private bool _allowDoubleJump;
        private bool _isJumping;

        private static readonly int IsGroundKey = Animator.StringToHash("is-ground");
        private static readonly int VerticalVelocityKey = Animator.StringToHash("vertical-velosity");
        private static readonly int IsRunningKey = Animator.StringToHash("is-running");
        private static readonly int Hit = Animator.StringToHash("hit");

        private void Awake()
        {
            _money = 0;
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        public void FixedUpdate()
        {
            var xVelocity = _direction.x * _speed;
            var yVelocity = CalculateYVelocity();
            _rigidbody.velocity = new Vector2(xVelocity, yVelocity);

            _animator.SetBool(IsGroundKey, IsGrounded());
            _animator.SetFloat(VerticalVelocityKey, _rigidbody.velocity.y);
            _animator.SetBool(IsRunningKey, _direction.x != 0);

            UpdateSpriteDirection();
        }

        internal void Interact()
        {
            var size = Physics2D.OverlapCircleNonAlloc(
                transform.position,
                _interactionRadius,
                _interactionResult,
                _interactionLayer);
            for (var i = 0; i < size; i++)
            {
                var interact = _interactionResult[i].GetComponent<InteractebleComponent>();
                if (interact != null)
                {
                    interact.Interact();
                }
            }
        }

        public void SetDirection(Vector2 direction) => _direction = direction;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.gameObject.IsInLayer(_groundCheck.CheckLayer)) return;

            var contact = collision.contacts[0];
            if (contact.relativeVelocity.y >= _landingVelocity)
            {
                _landingParticles.Spawn();
            }
        }

        private float CalculateYVelocity()
        {
            var yVelocity = _rigidbody.velocity.y;
            var isJumpPressing = _direction.y > 0;

            if (IsGrounded() && !_doubleJumpForbidden)
            {
                _allowDoubleJump = true;
                _isJumping = false;
            }

            if (isJumpPressing)
            {
                _isJumping = true;
                yVelocity = CalculateJumpVelocity(yVelocity);
            }
            else if (_rigidbody.velocity.y > 0.01 && _isJumping)
            {
                yVelocity *= 0.5f;
            }
            return yVelocity;
        }

        private float CalculateJumpVelocity(float yVelocity)
        {
            var isFalling = _rigidbody.velocity.y <= 0.001f;
            if (!isFalling) return yVelocity;

            if (IsGrounded())
            {
                yVelocity += _jumpForce;
                _jumpingParticles.Spawn();
            }
            else if (_allowDoubleJump && !_doubleJumpForbidden)
            {
                yVelocity = _jumpForce;
                _jumpingParticles.Spawn();
                _allowDoubleJump = false;
            }

            return yVelocity;
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
            Debug.Log("In the piggy bank +" + count + " coins. Total: " + _money);
        }

        public void TakeDamage()
        {
            _isJumping = false;
            _animator.SetTrigger(Hit);
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _damageJumpForce);

            if (_money > 0)
            {
                SpawnCoins();
            }
        }

        private void SpawnCoins()
        {
            var numCoinsToDispose = Mathf.Min(_money, 5);
            _money -= numCoinsToDispose;

            var burst = _hitParticles.emission.GetBurst(0);
            burst.count = numCoinsToDispose;
            _hitParticles.emission.SetBurst(0, burst);

            _hitParticles.gameObject.SetActive(true);
            _hitParticles.Play();
        }

        public void SpawnFootDust() => _footStepParticles.Spawn();
    }
}
