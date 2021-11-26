using OneEyedJoe.Model.Data;
using OneEyedJoe.UI.Widgets;
using UnityEngine;

namespace OneEyedJoe.UI.Settings
{
    public class SettingsWindow : AnimateWindow
    {
        [SerializeField] private AudioSettingsWidget _music;
        [SerializeField] private AudioSettingsWidget _sfx;
        
        protected override void Start()
        {
            base.Start();
            
            _music.SetModel(GameSettings.I.Music);
            _sfx.SetModel(GameSettings.I.Sfx);
        }

    }
}
