using System;
using UnityEngine;
using UnityEngine.Events;

namespace OneEyedJoe
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteAnimation : MonoBehaviour
    {
        [SerializeField] private int _frameRate;
        [SerializeField] private AnimationClips[] _clips;
        [SerializeField] private UnityEvent<string> _onComplete;

        private SpriteRenderer _renderer;

        private bool _isPlaying;
        private float _secondsPerFrame;
        private int _currentFrameIndex;
        private float _nextFrameTime;
        private int _currentClip;

        private void Start()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _secondsPerFrame = 1f / _frameRate;
            StartAnimation();
        }

        private void OnEnable()
        {
            _nextFrameTime = Time.time + _secondsPerFrame;
        }

        private void OnBecameVisible()
        {
            enabled = _isPlaying;
        }

        private void OnBecameInvisible()
        {
            enabled = false;
        }

        private void StartAnimation()
        {
            _nextFrameTime = Time.time + _secondsPerFrame;
            _isPlaying = true;
            _currentFrameIndex = 0;

        }

        private void Update()
        {
            if (_nextFrameTime > Time.time) return;

            var clip = _clips[_currentClip];
            if (_currentFrameIndex >= clip.Sprites.Length)
            {
                if (clip.Loop)
                {
                    _currentFrameIndex = 0;
                }
                else
                {
                    clip.OnComplete?.Invoke();
                    _onComplete?.Invoke(clip.Name);
                    _isPlaying = clip.AllowNextClip;
                    enabled = _isPlaying;
                    if (clip.AllowNextClip)
                    {
                        _currentFrameIndex = 0;
                        _currentClip = (int)Mathf.Repeat(_currentClip + 1, _clips.Length);
                    }
                }
                return;
            }

            _renderer.sprite = clip.Sprites[_currentFrameIndex];
            _nextFrameTime += _secondsPerFrame;
            _currentFrameIndex++;
        }

        public void SetClip(string name)
        {
            for (int i = 0; i < _clips.Length; i++)
            {
                if (_clips[i].Name == name)
                {
                    _currentClip = i;
                    StartAnimation();
                    return;
                }
            }
            _isPlaying = false;
            enabled = _isPlaying;
        }

    }

    [Serializable]
    public class AnimationClips
    {
        [SerializeField] private string _name;
        [SerializeField] private Sprite[] _sprites;
        [SerializeField] private bool _loop;
        [Tooltip("Может ли clip переключиться на следующий по окончанию спрайтов")]
        [SerializeField] private bool _allowNextClip;
        [SerializeField] private UnityEvent _onComplete;

        public string Name => _name;

        public Sprite[] Sprites { get => _sprites; }
        public bool Loop { get => _loop; }
        public bool AllowNextClip { get => _allowNextClip; }
        public UnityEvent OnComplete { get => _onComplete; }
    }

}

