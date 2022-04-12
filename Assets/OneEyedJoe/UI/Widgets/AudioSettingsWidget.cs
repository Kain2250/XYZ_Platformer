using System;
using OneEyedJoe.Model.Data.Properties;
using OneEyedJoe.Utils.Disposables;
using UnityEngine;
using UnityEngine.UI;

namespace OneEyedJoe.UI.Widgets
{
    public class AudioSettingsWidget : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private Text _textValue;

        private FloatPersistentProperty _model;

        private readonly CompositeDisposable _trash = new CompositeDisposable();
        
        private void Start()
        {
            _trash.Retain(_slider.onValueChanged.Subscribe(OnSliderValueChanged));
        }

        public void SetModel(FloatPersistentProperty model)
        {
            _model = model;
            _trash.Retain(model.Subscribe(OnValueChanged));
            OnValueChanged(model.Value, model.Value);
        }
        
        private void OnSliderValueChanged(float value)
        {
            _model.Value = value;
        }

        private void OnValueChanged(float newValue, float oldValue)
        {
            var textValue = Mathf.Round(newValue * 100);
            _textValue.text = textValue.ToString();
            _slider.normalizedValue = newValue;
        }


        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}