using System;
using OneEyedJoe.Model.Data;
using OneEyedJoe.Model.Data.Properties;
using UnityEngine;

namespace OneEyedJoe.Components.Audio
{
    public class AudioSettingsComponent : MonoBehaviour
    {
        [SerializeField] private SoundSetting _mode;
        private AudioSource _source;
        private FloatPersistentProperty _model;

        private void Start()
        {
            _source = GetComponent<AudioSource>();

            _model = FindProperty();
            _model.OnChanged += OnSoundSettingChanged;
            OnSoundSettingChanged(_model.Value, _model.Value);
        }
        
        private void OnSoundSettingChanged(float newValue, float oldValue)
        {
            _source.volume = newValue;
        }

        private FloatPersistentProperty FindProperty()
       {
           switch (_mode)
           {
               case SoundSetting.Music :
                   return GameSettings.I.Music;
               case SoundSetting.Sfx :
                   return GameSettings.I.Sfx;
               default:
                   throw new ArgumentException("Undefined model");
           }
       }

        private void OnDestroy()
        {
            _model.OnChanged -= OnSoundSettingChanged;
        }
    }
}