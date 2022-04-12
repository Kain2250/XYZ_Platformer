using System.Collections;
using UnityEngine;

namespace OneEyedJoe.Creatures.Mobs
{
    public class PinkStarAi : MobAi
    {
        [SerializeField] private float _speedRollAttack;

        protected override IEnumerator GoToHero()
        {
            var oldSpeed = _creature.Speed;
            while (_vision.IsTouchingLayer)
            {
                if (_checkAttack.IsTouchingLayer)
                {
                    StartState(Attack());
                    _creature.SetDash(false);
                }
                else
                {
                    _creature.SetDash(true);
                    SetDirectionToTarget();
                }

                yield return null;

            }
            
        }
    }
}