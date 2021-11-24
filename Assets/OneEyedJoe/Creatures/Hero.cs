using System;
using OneEyedJoe.Components;
using OneEyedJoe.Model;
using OneEyedJoe.Utils;
using UnityEditor.Animations;
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
        [SerializeField] private float _interactionRadius;
        [SerializeField] private LayerMask _interactionLayer;
        private readonly Collider2D[] _interactionResult = new Collider2D[1];
        
        [Space] [Header("Animators")]
        [SerializeField] private AnimatorController _armed;
        [SerializeField] private AnimatorController _unArmed;
        [SerializeField] private ParticleSystem _hitParticles;
        
        private GameSession _session;
        private bool _isOnWall;
        private bool _allowDoubleJump;

        protected override void Awake()
        {
            base.Awake();
            _defaultGravityScale = _rigidbody.gravityScale;
        }

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            var health = GetComponent<HealthComponent>();

            health.SetHealth(_session.Data.Hp);
            UpdateHeroWeapon();
        }

        protected override void Update()
        {
            base.Update();
            if (_wallCheck.IsTouchingLayer) //&& _direction.x == transform.localScale.x)
            {
                _isOnWall = true;
                _rigidbody.gravityScale = 0;
            }
            else
            {
                _isOnWall = false;
                _rigidbody.gravityScale = _defaultGravityScale;
            }
        }
        
        internal void Interact()
        {
            if (this == null) return;

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
            var isJumpPressing = _direction.y > 0;

            if (_isGrounded && !_doubleJumpForbidden || _isOnWall)
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
            if (_isGrounded || !_allowDoubleJump || _doubleJumpForbidden)
                return base.CalculateJumpVelocity(yVelocity);

            _particles.Spawn("Jump");
            _allowDoubleJump = false;
            
            return _jumpForce;
        }
        
        public void AddMoney(int count)
        {
            _session.Data.Coin += count;
            Debug.Log($"In the piggy bank + {count} coins. Total: {_session.Data.Coin}");
        }

        public override void TakeDamage()
        {
            base.TakeDamage();
            if (_session.Data.Coin > 0)
            {
                SpawnCoins();
            }
        }

        private void SpawnCoins()
        {
            var numCoinsToDispose = Mathf.Min(_session.Data.Coin, 5);
            _session.Data.Coin -= numCoinsToDispose;

            var burst = _hitParticles.emission.GetBurst(0);
            burst.count = numCoinsToDispose;
            _hitParticles.emission.SetBurst(0, burst);

            _hitParticles.gameObject.SetActive(true);
            _hitParticles.Play();
        }
        
        public override void Attack()
        {
            if (!_session.Data.IsArmed || this == null) return;
            base.Attack();
        }
        
        public void ArmHero()
        {
            _session.Data.IsArmed = true;
            UpdateHeroWeapon();
        }

        private void UpdateHeroWeapon()
        {
            _animator.runtimeAnimatorController = _session.Data.IsArmed ? _armed : _unArmed;
        }
        public void OnHealthChanged(int currentHealth)
        {
            _session.Data.Hp = currentHealth;
        }
    }
}
