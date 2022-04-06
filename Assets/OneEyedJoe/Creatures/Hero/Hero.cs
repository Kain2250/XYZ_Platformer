using System;
using System.Text;
using OneEyedJoe.Components.ColliderBased;
using OneEyedJoe.Components.GoBased;
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
        [SerializeField] private SpawnDropComponent _spawnDrop;

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

        private int SwordCount => _session.Data.Inventory.Count("Sword");
        private int CoinCount => _session.Data.Inventory.Count("Coin");
        private int UseItemCount => _session.Data.Inventory.Count("Potion");

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
            _session.Data.Inventory.OnChanged += OnChangeInventory;

            health.SetHealth(_session.Data.Hp.Value);
            UpdateHeroWeapon();
        }

        private void OnChangeInventory(string id, int value)
        {
            if (id == "Sword")
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

        
        public void AddInInventory(string id, int value)
        {
            _session.Data.Inventory.Add(id, value);
        }
        
        public override void TakeDamage()
        {
            base.TakeDamage();
            
            if (CoinCount > 0)
            {
                SpawnCoins();
            }
        }

        private void SpawnCoins()
        {
            var numCoinsToDispose = Mathf.Min(CoinCount, 5);
            _session.Data.Inventory.Remove("Coin", numCoinsToDispose);

            var burst = _hitParticles.emission.GetBurst(0);
            burst.count = numCoinsToDispose;
            _hitParticles.emission.SetBurst(0, burst);

            _hitParticles.gameObject.SetActive(true);
            _hitParticles.Play();
        }
        
        public override void Attack()
        {
            if (SwordCount <= 0) return;
            
            Sounds.Play("Melee");
            _particles.Spawn("SwordEffect");
            base.Attack();
        }
        
        private void UpdateHeroWeapon()
        {
            Animator.runtimeAnimatorController = SwordCount > 0 ? _armed : _unArmed;
        }
        
        public void OnHealthChanged(int currentHealth)
        {
            _session.Data.Hp.Value = currentHealth;
        }

        public void OnWeaponCountChanged(int currentWeaponCount)
        {
            var currentCountSwords = currentWeaponCount - SwordCount;
            
            AddInInventory("Sword", currentCountSwords);
        }

        public void Throw()
        {
            if (_throwCooldown.IsReady && _armed && SwordCount > 1)
            {
                Animator.SetTrigger(ThrowKey);
                _throwCooldown.Reset();
            }
        }

        public void OnDoThrow()
        {
            if (SwordCount > 1)
            {
                _particles.Spawn("Throw");
                _session.Data.Inventory.Remove("Sword", 1);
            }
        }

        private void OnDestroy()
        {
            _session.Data.Inventory.OnChanged -= OnChangeInventory;
        }

        public void UseItemIsInventory()
        {
            if (UseItemCount > 0)
            {
                var potionValue = _session.Data.Inventory.GetValue("Potion");
                GetComponent<HealthComponent>().Apply(potionValue);
                
                _session.Data.Inventory.Remove("Potion", 1);
            }
        }

        public void Drop(GameObject go)
        {
            var oldName = go.name;
            if (oldName.Contains("(Clone)"))
            {
                go.name = oldName.Replace("(Clone)", String.Empty);
            }
            
            _spawnDrop.Spawn(go);
        }
    }
}
