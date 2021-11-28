using OneEyedJoe.Components;
using OneEyedJoe.Model;
using OneEyedJoe.Utils;
using UnityEngine;

namespace OneEyedJoe.Creatures
{
    public class Hero : Creature
    {
        [SerializeField] private float _landingVelocity;
        [SerializeField] private bool _doubleJumpForbidden;
        [SerializeField] private float _defaultGravityScale;
        [SerializeField] private LayerCheck _wallCheck;

        [Space] [Header("Interactions")]
        [SerializeField] private CheckCircleOverlap _interactionCheck;
        
        [Space] [Header("Animators")]
        [SerializeField] private RuntimeAnimatorController _armed;
        [SerializeField] private RuntimeAnimatorController _unArmed;
        [SerializeField] private ParticleSystem _hitParticles;
        
        private GameSession _session;
        private bool _isOnWall;
        private bool _allowDoubleJump;

        protected override void Awake()
        {
            base.Awake();
            _defaultGravityScale = Rigidbody.gravityScale;
            if (_session == null)
              _session = FindObjectOfType<GameSession>();

        }

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            var health = GetComponent<HealthComponent>();

            health.SetHealth(_session.Data.Hp.Value);
            UpdateHeroWeapon();
        }

        protected override void Update()
        {
            base.Update();
            if (_wallCheck.IsTouchingLayer && Direction.x == transform.localScale.x)
            {
                _isOnWall = true;
                Rigidbody.gravityScale = 0;
            }
            else
            {
                _isOnWall = false;
                Rigidbody.gravityScale = _defaultGravityScale;
            }
        }

        internal void Interact()
        {
            _interactionCheck.Check();
        }
        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.gameObject.IsInLayer(_groundCheck.CheckLayer)) return;

            var contact = collision.contacts[0];
            if (contact.relativeVelocity.y >= _landingVelocity)
            {
                _particles.Spawn("Landing");
            }
        }

        protected override float CalculateYVelocity()
        {
            var isJumpPressing = Direction.y > 0;

            if (IsGrounded && !_doubleJumpForbidden || _isOnWall)
            {
                _allowDoubleJump = true;
            }
            
            if (!isJumpPressing && _isOnWall)
            {
                return 0f;
            }
            return base.CalculateYVelocity();
        }

        protected override float CalculateJumpVelocity(float yVelocity)
        {
            if (IsGrounded || !_allowDoubleJump || _doubleJumpForbidden)
                return base.CalculateJumpVelocity(yVelocity);

            Sounds.Play("Jump");
            _particles.Spawn("Jump");
            _allowDoubleJump = false;
            
            return _jumpForce;
        }
        
        public void AddMoney(int count)
        {
            _session.Data.Coin.Value += count;
            Debug.Log($"In the piggy bank + {count} coins. Total: {_session.Data.Coin.Value}");
        }

        public override void TakeDamage()
        {
            base.TakeDamage();
            if (_session.Data.Coin.Value > 0)
            {
                SpawnCoins();
            }
        }

        private void SpawnCoins()
        {
            var numCoinsToDispose = Mathf.Min(_session.Data.Coin.Value, 5);
            _session.Data.Coin.Value -= numCoinsToDispose;

            var burst = _hitParticles.emission.GetBurst(0);
            burst.count = numCoinsToDispose;
            _hitParticles.emission.SetBurst(0, burst);

            _hitParticles.gameObject.SetActive(true);
            _hitParticles.Play();
        }
        
        public override void Attack()
        {
            if (!_session.Data.IsArmed) return;
            
            Sounds.Play("Melee");
            _particles.Spawn("SwordEffect");
            base.Attack();
        }
        
        public void ArmHero()
        {
            _session.Data.IsArmed = true;
            UpdateHeroWeapon();
        }

        private void UpdateHeroWeapon()
        {
            Animator.runtimeAnimatorController = _session.Data.IsArmed ? _armed : _unArmed;
        }
        public void OnHealthChanged(int currentHealth)
        {
            _session.Data.Hp.Value = currentHealth;
        }
    }
}
