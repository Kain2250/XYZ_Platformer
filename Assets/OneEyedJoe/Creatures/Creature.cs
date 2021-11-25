using System;
using OneEyedJoe.Components;
using UnityEngine;

namespace OneEyedJoe.Creatures
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Creature : MonoBehaviour
    {
        [Header("Params")]
        [SerializeField] protected float _speed;
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

        protected Rigidbody2D Rigidbody;
        protected Animator Animator;
        protected Vector2 Direction;
        protected bool IsGrounded;
        private bool _isJumping;


        protected virtual void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            Animator = GetComponent<Animator>();
        }

        protected virtual void Update()
        {
            IsGrounded = _groundCheck.IsTouchingLayer;
        }
        
        public void FixedUpdate()
        {
            var xVelocity = Direction.x * _speed;
            var yVelocity = CalculateYVelocity();
            Rigidbody.velocity = new Vector2(xVelocity, yVelocity);

            Animator.SetBool(IsGroundKey, IsGrounded);
            Animator.SetFloat(VerticalVelocityKey, Rigidbody.velocity.y);
            Animator.SetBool(IsRunningKey, Direction.x != 0);

            UpdateSpriteDirection();
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
                _particles.Spawn("Jump");
            }

            return yVelocity;
        }
        
        public void SetDirection(Vector2 direction) => Direction = direction;

        private void UpdateSpriteDirection()
        {
            var multiplier = _invertScale ? -1 : 1;
            if (Direction.x > 0)
            {
                transform.localScale = new Vector3(multiplier, 1, 1);
            }
            else if (Direction.x < 0)
            {
                transform.localScale = new Vector3(-1 * multiplier, 1, 1);
            }
        }
        
        public virtual void TakeDamage()
        {
            _isJumping = false;
            Animator.SetTrigger(Hit);
            Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, _damageJumpForce);
        }

        private void Attacking()
        {
            _attackRange.Check();
        }

        public virtual void Attack()
        {
            Animator.SetTrigger(IsAttack);
            _particles.Spawn("SwordEffect");
        }


    }
}
