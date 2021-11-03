using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerController
{
    public class HeroInputReader : MonoBehaviour
    {
        [SerializeField] private Hero _hero;

        private HeroInputActions _inputActions;

        private void Awake()
        {
            _inputActions = new HeroInputActions();
            _inputActions.Hero.AxisMovement.performed += OnAxisMovement;
            _inputActions.Hero.AxisMovement.canceled += OnAxisMovement;

            _inputActions.Hero.SaySomething.canceled += OnSaySomething;
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

        public void OnHorizontalMovement(InputAction.CallbackContext context)
        {
            var direction = context.ReadValue<float>();
            _hero.SetDirection(Vector2.zero);
        }

        public void OnSaySomething(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                _hero.SaySomething();
            }
        }
    }
}

