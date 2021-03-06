using OneEyedJoe.Components.Audio;
using OneEyedJoe.Components.ColliderBased;
using OneEyedJoe.Components.GoBased;
using UnityEngine;

namespace OneEyedJoe.Creatures
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Creature : MonoBehaviour
    {
        [Header("Params")]
        [SerializeField] protected float _speed;
        [SerializeField] protected float _dashModifier = 2;
        
        [SerializeField] protected float _jumpForce;
        [SerializeField] protected float _damageJumpForce;
        [SerializeField] private bool _invertScale;
        
        [Header("Checkers")]
        [SerializeField] protected LayerCheck _groundCheck;
        [SerializeField] private CheckCircleOverlap _attackRange;
        [SerializeField] protected SpawnListComponent _particles;
        
        private static readonly int IsGroundKey = Animator.StringToHash("is-ground");
        private static readonly int VerticalVelocityKey = Animator.StringToHash("vertical-velocity");
        private static readonly int IsRunningKey = Animator.StringToHash("is-running");
        private static readonly int Hit = Animator.StringToHash("hit");
        private static readonly int IsAttack = Animator.StringToHash("attack");

        protected PlaySoundsComponent Sounds;
        protected Rigidbody2D Rigidbody;
        protected Animator Animator;
        protected Vector2 Direction;
        protected bool IsGrounded;
        private bool _isDash;
        private bool _isJumping;


        public float Speed => _speed;

        protected virtual void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            Animator = GetComponent<Animator>();
            Sounds = GetComponent<PlaySoundsComponent>();
        }

        protected virtual void Update()
        {
            IsGrounded = _groundCheck.IsTouchingLayer;
        }
        
        public void FixedUpdate()
        {
            var xVelocity = CalculateXVelocity();
            var yVelocity = CalculateYVelocity();
            Rigidbody.velocity = new Vector2(xVelocity, yVelocity);

            Animator.SetBool(IsGroundKey, IsGrounded);
            Animator.SetFloat(VerticalVelocityKey, Rigidbody.velocity.y);
            Animator.SetBool(IsRunningKey, Direction.x != 0);

            UpdateSpriteDirection(Direction);
        }

        protected virtual float CalculateXVelocity()
        {
            return Direction.x * _speed * (_isDash ? _dashModifier : 1f);
        }

        protected virtual float CalculateYVelocity()
        {
            var yVelocity = Rigidbody.velocity.y;
            var isJumpPressing = Direction.y > 0;

            if (IsGrounded)
            {
                _isJumping = false;
            }
            if (isJumpPressing)
            {
                _isJumping = true;
                
                var isFalling = Rigidbody.velocity.y <= 0.001f;
                yVelocity = isFalling ? CalculateJumpVelocity(yVelocity) : yVelocity;
            }
            else if (Rigidbody.velocity.y > 0.01 && _isJumping)
            {
                yVelocity *= 0.5f;
            }
            return yVelocity;
        }

        protected virtual float CalculateJumpVelocity(float yVelocity)
        {
            if (IsGrounded)
            {
                yVelocity += _jumpForce;
                Sounds.Play("Jump");
                _particles.Spawn("Jump");
            }

            return yVelocity;
        }
        
        public void SetDirection(Vector2 direction) => Direction = direction;

        public void UpdateSpriteDirection(Vector2 direction)
        {
            var multiplier = _invertScale ? -1 : 1;
            if (direction.x > 0)
            {
                transform.localScale = new Vector3(multiplier, 1, 1);
            }
            else if (direction.x < 0)
            {
                transform.localScale = new Vector3(-1 * multiplier, 1, 1);
            }
        }
        
        public virtual void TakeDamage()
        {
            _isJumping = false;
            Animator.SetTrigger(Hit);
            Rigidbody.velocity = new Vector2(_damageJumpForce, _damageJumpForce);
            
        }

        private void Attacking()
        {
            _attackRange.Check();
        }

        public virtual void Attack()
        {
            Animator.SetTrigger(IsAttack);
        }

        public void SetDash(bool isDash)
        {
            _isDash = isDash;
        }
    }
}
