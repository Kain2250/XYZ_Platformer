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
                    _creature.ChangeSpeed(oldSpeed);
                }
                else
                {
                    _creature.ChangeSpeed(_speedRollAttack);
                    SetDirectionToTarget();
                }

                yield return null;

            }
        }
    }
}