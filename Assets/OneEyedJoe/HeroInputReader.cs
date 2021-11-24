using OneEyedJoe.Creatures;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OneEyedJoe
{
    public class HeroInputReader : MonoBehaviour
    {
        [SerializeField] private Hero _hero;

        private HeroInputAction _inputActions;

        private void Awake()
        {
            _inputActions = new HeroInputAction();
            _inputActions.Hero.AxisMovement.performed += OnAxisMovement;
            _inputActions.Hero.AxisMovement.canceled += OnAxisMovement;

            _inputActions.Hero.Attack.canceled += OnAttack;
            _inputActions.Hero.Interact.canceled += OnInteract;
        }

        private void OnEnable()
        {
            _inputActions.Enable();
        }

        private void OnAxisMovement(InputAction.CallbackContext context)
        {
            var direction = context.ReadValue<Vector2>();
            _hero.SetDirection(direction);
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                _hero.Attack();
            }
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                _hero.Interact();
            }
        }
    }
}

