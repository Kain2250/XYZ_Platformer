using System;
using UnityEngine;

namespace OneEyedJoe.Components
{
    public class DelayDestroyObjectComponent : MonoBehaviour
    {
        [SerializeField] private float _delay;

        private bool _timerIsRunning;
        private float _endTime;

        private void Start()
        {
            _endTime = _delay;
            _timerIsRunning = true;
        }

        public bool TimerIsRunning => _timerIsRunning;

        private void Update()
        {
            if (!_timerIsRunning) return;

            if (_endTime > 0)
            {
                _endTime -= Time.deltaTime;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void StartDelay()
        {
            _timerIsRunning = true;
        }
    }
}
