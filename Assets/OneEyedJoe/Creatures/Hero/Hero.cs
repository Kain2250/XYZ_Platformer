using System;
using System.Collections;
using OneEyedJoe.Components.ColliderBased;
using OneEyedJoe.Components.GoBased;
using OneEyedJoe.Components.Health;
using OneEyedJoe.Model;
using OneEyedJoe.Model.Data;
using OneEyedJoe.Model.Definition;
using OneEyedJoe.Utils;
using UnityEngine;

namespace OneEyedJoe.Creatures.Hero
{
    public class Hero : Creature, ICanAddInventory, ICanDropItem
    {
        [SerializeField] private float _landingVelocity;
        [SerializeField] private bool _doubleJumpForbidden;
        [SerializeField] private float _defaultGravityScale;
        [SerializeField] private LayerCheck _wallCheck;
        [SerializeField] private SpawnDropComponent _spawnDrop;

        [Space] [Header("Throw")]
        [SerializeField] private Cooldown _throwCooldown;
        [SerializeField] private Cooldown _superThrowCooldown;
        [SerializeField] private float _superThrowDelay;
        [SerializeField] private int _countSuperThrowParticles;
        [SerializeField] private SpawnComponent _throwSpawner;
        
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
        private bool _superThrow;


        private const string SwordId = "Sword";
        private const string HealthPotionId = "Potion";
        private int SwordCount => _session.Data.Inventory.Count(SwordId);
        private int CoinCount => _session.Data.Inventory.Count("Coin");
        private int HealthPotionCount => _session.Data.Inventory.Count(HealthPotionId);
        private string SelectedItemId => _session.QuickInventory.SelectedItem.Id;
        
        private bool CanThrow
        {
            get
            {
                if (SelectedItemId == SwordId)
                    return SwordCount > 1;

                var def = DefsFacade.I.Items.Get(SelectedItemId);
                return def.HasTag(ItemTag.Throwable);
            }
        }

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

        private void OnChangeInventory(string id, int value)
        {
            if (id == SwordId)
                UpdateHeroWeapon();
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

        
        public void StartThrowing()
        {
            _superThrowCooldown.Reset();
        }

        public void PerformThrowing()
        {
            if (!_throwCooldown.IsReady || !CanThrow) return;

            if (_superThrowCooldown.IsReady) _superThrow = true;

            Animator.SetTrigger(ThrowKey);
            _throwCooldown.Reset();
        }

        
        public void OnDoThrow()
        {
            if (_superThrow)
            {
                var throwableCount = _session.Data.Inventory.Count(SelectedItemId);
                var possibleCount = SelectedItemId == SwordId ? throwableCount - 1 : throwableCount;
                var numThrows = Mathf.Min(_countSuperThrowParticles, possibleCount);
                StartCoroutine(DoSuperThrow(numThrows));
            }
            else
            {
                ThrowAndRemoveFromInventory();
            }

            _superThrow = false;
        }

        private IEnumerator DoSuperThrow(int numThrows)
        {
            for (int i = 0; i < numThrows; i++)
            {
                ThrowAndRemoveFromInventory();
                yield return new WaitForSeconds(_superThrowDelay);
            }
        }

        private void ThrowAndRemoveFromInventory()
        {
            Sounds.Play("Range");
            
            var throwableDef = DefsFacade.I.Throwable.Get(SelectedItemId);
            _throwSpawner.SetPrefab(throwableDef.Projectile);
            _throwSpawner.Spawn();
            
            _session.Data.Inventory.Remove(SelectedItemId, 1);
        }

        private void UsePotion()
        {
            if (HealthPotionCount > 0)
            {
                var potionValue = _session.Data.Inventory.GetValue(SelectedItemId);
                var currentHeals = _session.Data.Hp.Value;
                if (potionValue + currentHeals >= DefsFacade.I.Player.MaxHealth)
                    potionValue = DefsFacade.I.Player.MaxHealth - currentHeals;
                
                GetComponent<HealthComponent>().Apply(potionValue);
                _session.Data.Inventory.Remove(SelectedItemId, 1);
            }
        }

        public void UseItemIsInventory()
        {
            switch (SelectedItemId)
            {
                case HealthPotionId:
                    UsePotion();
                    break;
                default:
                    Interact();
                    break;
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
        
        public void NextItem()
        {
            _session.QuickInventory.SetNextItem();
        }
        
        private void OnDestroy()
        {
            _session.Data.Inventory.OnChanged -= OnChangeInventory;
        }

    }

}
