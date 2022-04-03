using UnityEngine;
using OneEyedJoe.Components.ColliderBased;
using OneEyedJoe.Components.GoBased;
using OneEyedJoe.Utils;


namespace OneEyedJoe.Creatures.Mobs
{
    public class SeashellAI : ShootingTrapAI
    {
        [Header("Melee")]
        [SerializeField] private Cooldown _meleeCooldown;
        [SerializeField] private CheckCircleOverlap _meleeAttack;
        [SerializeField] private LayerCheck _meleeCanAttack;
        
        private static readonly int MeleeKey = Animator.StringToHash("attack");

        protected override void Update()
        {
            if (_meleeCanAttack.IsTouchingLayer)
            {
                if (_meleeCooldown.IsReady)
                {
                    MeleeAttack();
                    _meleeCooldown.Reset();
                }

                return;
            }
            
            base.Update();
        }
        
        private void MeleeAttack()
        {
            Animator.SetTrigger(MeleeKey);
        }
        
        public void OnMeleeAttack()
        {
            _meleeAttack.Check();
        }
        
    }
}