using UnityEngine;
using UnityEngine.InputSystem;

namespace OneEyedJoe.Creatures.Hero
{
    public class HeroInputReader : MonoBehaviour
    {
        [SerializeField] private Hero _hero;

        private HeroInputAction _inputActions;
        private bool _isPause;

        private void Awake()
        {
            _inputActions = new HeroInputAction();
            _inputActions.Hero.AxisMovement.performed += OnAxisMovement;
            _inputActions.Hero.AxisMovement.canceled += OnAxisMovement;

            _inputActions.Hero.Attack.canceled += OnAttack;
            _inputActions.Hero.Interact.canceled += OnInteract;
            _inputActions.Hero.Throw.performed += OnThrow;
            _inputActions.Hero.Pause.canceled += OnPause;

            _isPause = false;
        }
        private void OnPause(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                _isPause = !_isPause;
                Time.timeScale = _isPause ? 0 : 1f;
            }
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

        public void OnThrow(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _hero.Throw();
            }
        }

        private void OnDestroy()
        {
            _inputActions.Hero.AxisMovement.performed -= OnAxisMovement;
            _inputActions.Hero.AxisMovement.canceled -= OnAxisMovement;

            _inputActions.Hero.Attack.canceled -= OnAttack;
            _inputActions.Hero.Interact.canceled -= OnInteract;
            _inputActions.Hero.Pause.canceled -= OnPause;
            _inputActions.Hero.Throw.performed -= OnThrow;


        }
    }
}

