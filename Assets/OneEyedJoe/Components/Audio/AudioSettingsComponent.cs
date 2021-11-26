using System;
using OneEyedJoe.Model.Data;
using OneEyedJoe.Model.Data.Properties;
using UnityEngine;

namespace OneEyedJoe.Components.Audio
{
    public class AudioSettingsComponent : MonoBehaviour
    {
        [SerializeField] private SoundSetting _mode;
        
        private void Start()
        {
        }

       //private FloatPersistentProperty FindProperty()
       //{
       //    switch (_mode)
       //    {
       //        case GameSettings.Music :
       //            return GameSettings.I.Music;
       //        case SoundSetting.Sfx :
       //            return GameSettings.I.Sfx;
       //    }

       //    throw new ArgumentException("Undefined model");

       //}
    }
}