using OneEyedJoe.Utils;
using UnityEngine;

namespace OneEyedJoe.Components.Audio
{
    public class PlaySfvSound : MonoBehaviour
    {
        [SerializeField] private AudioClip _clip;
        private AudioSource _source;

        public void Play()
        {
            if (_source == null)
                _source = AudioUtils.FindSfxSource();
            
            _source.PlayOneShot(_clip);
        }
    }
}