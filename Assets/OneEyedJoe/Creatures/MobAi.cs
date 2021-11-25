using System;
using System.Collections;
using OneEyedJoe.Components;
using UnityEngine;

namespace OneEyedJoe.Creatures
{
    public class MobAi : MonoBehaviour
    {
        [SerializeField] private LayerCheck _vizion;
        [SerializeField] private LayerCheck _checkAttack;

        [SerializeField] private float _attackCooldown = 1f;
        [SerializeField] private float _alarmDelay = 0.5f;
        [SerializeField] private float _missDelay = 0.5f;
        [SerializeField] private float _deathCloudDelay = 0.5f;
        
        private Coroutine _current;
        private GameObject _target;

        private SpawnListComponent _particles;
        private Creature _creature;
        private Animator _animator;
        private Patrol _patrol;
        
        private static readonly int IsDie = Animator.StringToHash("is-dead");
        private bool _isDead;
        
        private void Awake()
        {
            _particles = GetComponent<SpawnListComponent>();
            _creature = GetComponent<Creature>();
            _animator = GetComponent<Animator>();
            _patrol = GetComponent<Patrol>();
        }
        
        private void Start()
        {
            StartState(_patrol.DoPatrol());
        }

        public void OnHeroInVision(GameObject go)
        {
            if (_isDead) return;
            
            _target = go;
            
            StartState(AgrToHero());
        }
        
        private IEnumerator AgrToHero()
        {
            _particles.Spawn("Exclamation");
            
            yield return new WaitForSeconds(_alarmDelay);
            
            StartState(GoToHero());
        }
        
        private IEnumerator GoToHero()
        {
            while (_vizion.IsTouchingLayer)
            {
                if (_checkAttack.IsTouchingLayer)
                {
                    StartState(Attack());
                }
                else
                {
                    SetDirectionToTarget();
                }
                
                yield return null;
            }
            
            _particles.Spawn("Miss");
            yield return new WaitForSeconds(_missDelay);
            
            StartState(_patrol.DoPatrol());
        }
        private IEnumerator Attack()
        {
            while (_checkAttack.IsTouchingLayer)
            {
                _creature.Attack();
                yield return new WaitForSeconds(_attackCooldown);
            }
            
            if (!_isDead)
                StartState(GoToHero());
            else
            {
                OnDie();
            }
        }

        private void SetDirectionToTarget()
        {
            var direction = _target.transform.position - transform.position;
            direction.y = 0f;
            _creature.SetDirection(direction.normalized);
        }
        
        public void OnDie()
        {
            _isDead = true;
            _animator.SetBool(IsDie, true);

            if (_current != null || _isDead)
            {
                _creature.SetDirection(Vector2.zero);
                StopCoroutine(_current);
            }
        }

        public void DeathCloudSpawn() => _particles.Spawn("DeathCloud");

        private void StartState(IEnumerator coroutine)
        {
            _creature.SetDirection(Vector2.zero);
            
            if (_current != null || _isDead)
                StopCoroutine(_current);

            _current = StartCoroutine(coroutine);
        }

    }
}
