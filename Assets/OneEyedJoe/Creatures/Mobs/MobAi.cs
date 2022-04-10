using System.Collections;
using OneEyedJoe.Components.ColliderBased;
using OneEyedJoe.Components.GoBased;
using OneEyedJoe.Creatures.Mobs.Patrolling;
using UnityEngine;

namespace OneEyedJoe.Creatures.Mobs
{
    public class MobAi : MonoBehaviour
    {
        [SerializeField] protected LayerCheck _vision;
        [SerializeField] protected LayerCheck _checkAttack;

        [SerializeField] protected float _attackCooldown = 1f;
        [SerializeField] private float _alarmDelay = 0.5f;
        [SerializeField] private float _missDelay = 0.5f;
        
        private Coroutine _current;
        private GameObject _target;

        private SpawnListComponent _particles;
        protected Creature _creature;
        private Animator _animator;
        private Patrol _patrol;
        
        private static readonly int IsDie = Animator.StringToHash("is-dead");
        protected bool _isDead;
        
        private void Awake()
        {
            _particles = GetComponent<SpawnListComponent>();
            _creature = GetComponent<Creature>();
            _animator = GetComponent<Animator>();
            _patrol = GetComponent<Patrol>();
        }
        
        private void Start()
        {
            if (_patrol)
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
            LookAtHero();
            _particles.Spawn("Exclamation");
            
            yield return new WaitForSeconds(_alarmDelay);
            
            StartState(GoToHero());
        }

        private void LookAtHero()
        {
            var direction = GetDirectionToTarget();
            _creature.SetDirection(Vector2.zero);
            _creature.UpdateSpriteDirection(direction);
        }

        protected virtual IEnumerator GoToHero()
        {
            while (_vision.IsTouchingLayer)
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
            
            _creature.SetDirection(Vector2.zero);
            _particles.Spawn("Miss");
            yield return new WaitForSeconds(_missDelay);
            
            if (_patrol)
                StartState(_patrol.DoPatrol());
        }

        protected IEnumerator Attack()
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

        protected void SetDirectionToTarget()
        {
            var direction = GetDirectionToTarget();
            _creature.SetDirection(direction.normalized);
        }

        private Vector2 GetDirectionToTarget()
        {
            var direction = _target.transform.position - transform.position;
            direction.y = 0f;
            return direction.normalized;

        }
        
        public void OnDie()
        {
            _isDead = true;
            _animator.SetBool(IsDie, true);

            if (_current != null && _isDead)
            {
                _creature.SetDirection(Vector2.zero);
                StopCoroutine(_current);
            }
        }

        protected void StartState(IEnumerator coroutine)
        {
            _creature.SetDirection(Vector2.zero);
            
            if (_current != null)
                StopCoroutine(_current);

            _current = StartCoroutine(coroutine);
        }

    }
}
