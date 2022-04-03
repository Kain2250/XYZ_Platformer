﻿using OneEyedJoe.Components.ColliderBased;
using OneEyedJoe.Components.Health;
using OneEyedJoe.Model;
using OneEyedJoe.Utils;
using UnityEngine;

namespace OneEyedJoe.Creatures.Hero
{
    public class Hero : Creature
    {
        [SerializeField] private float _landingVelocity;
        [SerializeField] private bool _doubleJumpForbidden;
        [SerializeField] private float _defaultGravityScale;
        [SerializeField] private LayerCheck _wallCheck;

        [SerializeField] private Cooldown _throwCooldown;

        [Space] [Header("Interactions")]
        [SerializeField] private CheckCircleOverlap _interactionCheck;
        
        [Space] [Header("Animators")]
        [SerializeField] private RuntimeAnimatorController _armed;
        [SerializeField] private RuntimeAnimatorController _unArmed;
        [SerializeField] private ParticleSystem _hitParticles;

        private static readonly int ThrowKey = Animator.StringToHash("throw");
        private static readonly int ClimbKey = Animator.StringToHash("is-on-wall");
        
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

            var moveToSameDirection = Direction.x * transform.lossyScale.x > 0;
            if (_wallCheck.IsTouchingLayer && moveToSameDirection)
            {
                _isOnWall = true;
                Rigidbody.gravityScale = 0;
            }
            else
            {
                _isOnWall = false;
                Rigidbody.gravityScale = _defaultGravityScale;
            }
            
            Animator.SetBool(ClimbKey, _isOnWall);
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
            if (!IsGrounded && _allowDoubleJump && !_doubleJumpForbidden && !_isOnWall)
            {
                Sounds.Play("Jump");
                _particles.Spawn("Jump");
                _allowDoubleJump = false;

                return _jumpForce;
            }

            return base.CalculateJumpVelocity(yVelocity);
        }
        
        public void AddMoney(int count)
        {
            _session.Data.Coin.Value += count;
            //Debug.Log($"In the piggy bank + {count} coins. Total: {_session.Data.Coin.Value}");
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
            if (_session.Data.Weapon.Value < 0) return;
            
            Sounds.Play("Melee");
            _particles.Spawn("SwordEffect");
            base.Attack();
        }
        
        public void ArmHero()
        {
            _session.Data.Weapon.Value += 1;

            UpdateHeroWeapon();
        }

        private void UpdateHeroWeapon()
        {
            Animator.runtimeAnimatorController = _session.Data.Weapon.Value > 0 ? _armed : _unArmed;
        }
        
        public void OnHealthChanged(int currentHealth)
        {
            _session.Data.Hp.Value = currentHealth;
        }

        public void OnWeaponCountChanged(int currentWeaponCount)
        {
            _session.Data.Weapon.Value = currentWeaponCount;
        }

        public void Throw()
        {
            if (_throwCooldown.IsReady && _armed && _session.Data.Weapon.Value > 1)
            {
                Animator.SetTrigger(ThrowKey);
                _throwCooldown.Reset();
            }
        }

        public void OnDoThrow()
        {
            if (_session.Data.Weapon.Value > 1)
            {
                _particles.Spawn("Throw");
                _session.Data.Weapon.Value -= 1;
            }
        }
    }
}
