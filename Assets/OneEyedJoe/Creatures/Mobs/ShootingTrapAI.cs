using OneEyedJoe.Components.ColliderBased;
using OneEyedJoe.Components.GoBased;
using OneEyedJoe.Utils;
using UnityEngine;

namespace OneEyedJoe.Creatures.Mobs
{
    public class ShootingTrapAI : MonoBehaviour
    {
        [SerializeField] private LayerCheck _vision;
        
        [Header("Range")]
        [SerializeField] private Cooldown _rangeCooldown;
        [SerializeField] private SpawnComponent _rangeAttack;
        
        protected Animator Animator;
        private static readonly int RangeKey = Animator.StringToHash("throw");
        private static readonly int HitKey = Animator.StringToHash("hit");

        private void Awake()
        {
            Animator = GetComponent<Animator>();
        }

        protected virtual void Update()
        {
            if (_vision.IsTouchingLayer && _rangeCooldown.IsReady)
            {
                RangeAttack();
                _rangeCooldown.Reset();
            }
        }
        
        private void RangeAttack()
        {
            Animator.SetTrigger(RangeKey);
        }
        
        public void OnRangeAttack()
        {
            _rangeAttack.Spawn();
        }
        
        public void TakeDamage()
        {
            Animator.SetTrigger(HitKey);
        }
    }
}