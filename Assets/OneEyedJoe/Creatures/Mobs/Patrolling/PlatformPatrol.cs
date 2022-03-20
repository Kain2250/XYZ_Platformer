using System.Collections;
using OneEyedJoe.Components.ColliderBased;
using UnityEngine;

namespace OneEyedJoe.Creatures.Mobs.Patrolling
{
    public class PlatformPatrol : Patrol
    {
        [SerializeField] private LayerCheck _endGroundPositionCheck;

        private Creature _creature;

        private void Awake()
        {
            _creature = GetComponent<Creature>();
        }

        public override IEnumerator DoPatrol()
        {
            while (enabled)
            {
                if (_endGroundPositionCheck.IsTouchingLayer)
                {
                    _creature.SetDirection((transform.forward * -1).normalized);
                }

                yield return null;
            }
        }
    }
}
