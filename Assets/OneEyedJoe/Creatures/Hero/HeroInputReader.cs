using OneEyedJoe.Model;
using OneEyedJoe.UI.MainMenu;
using OneEyedJoe.Utils.Disposables;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace OneEyedJoe.Creatures.Hero
{
    public class HeroInputReader : MonoBehaviour
    {
        [SerializeField] private Hero _hero;

        private HeroInputAction _inputActions;
        private GameSession _session;

        private void Awake()
        {
            _session = FindObjectOfType<GameSession>();
            
            _inputActions = new HeroInputAction();
            _inputActions.Hero.AxisMovement.performed += OnAxisMovement;
            _inputActions.Hero.AxisMovement.canceled += OnAxisMovement;

            _inputActions.Hero.Attack.canceled += OnAttack;
            _inputActions.Hero.Interact.canceled += OnInteract;
            _inputActions.Hero.NextItem.canceled += OnNextItem;
            _inputActions.Hero.Throw.started += OnThrow;
            _inputActions.Hero.Throw.canceled += OnThrow;
            _inputActions.Hero.Pause.canceled += OnPause;
            _inputActions.Hero.Dash.started += OnDash;
            _inputActions.Hero.Dash.canceled += OnDash;
            
        }

        public void OnNextItem(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                _hero.NextItem();
            }
        }
        
        public void OnDash(InputAction.CallbackContext context)
        {
            if (context.started)
                _hero.SetDash(true);
            if (context.canceled)
                _hero.SetDash(false);
        }

        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                _session.IsPause = !_session.IsPause;
                Time.timeScale = _session.IsPause ? 0 : 1f;

                if (_session.IsPause)
                    SceneManager.LoadScene("PauseMenu", LoadSceneMode.Additive);
                else
                    SceneManager.UnloadSceneAsync("PauseMenu");
            }
        }

        private void OnEnable()
        {
            _inputActions.Enable();
        }

        public void OnAxisMovement(InputAction.CallbackContext context)
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
            if (context.started)
            {
                _hero.StartThrowing();
            }

            if (context.canceled)
            {
                _hero.PerformThrowing();
            }
        }

        private void OnDestroy()
        {
            _inputActions.Hero.AxisMovement.performed -= OnAxisMovement;
            _inputActions.Hero.AxisMovement.canceled -= OnAxisMovement;

            _inputActions.Hero.NextItem.canceled -= OnNextItem;
            _inputActions.Hero.Attack.canceled -= OnAttack;
            _inputActions.Hero.Interact.canceled -= OnInteract;
            _inputActions.Hero.Pause.canceled -= OnPause;
            _inputActions.Hero.Throw.started -= OnThrow;
            _inputActions.Hero.Throw.canceled -= OnThrow;
            _inputActions.Hero.Dash.started -= OnDash;
            _inputActions.Hero.Dash.canceled -= OnDash;
        }
    }
}

