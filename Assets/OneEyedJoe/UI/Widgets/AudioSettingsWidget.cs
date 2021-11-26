using System;
using OneEyedJoe.Model.Data.Properties;
using UnityEngine;
using UnityEngine.UI;

namespace OneEyedJoe.UI.Widgets
{
    public class AudioSettingsWidget : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private Text _textValue;

        private FloatPersistentProperty _model;
        private void Start()
        {
            _slider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        public void SetModel(FloatPersistentProperty model)
        {
            _model = model;
            model.OnChanged += OnValueChanged;
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
            _slider.onValueChanged.RemoveListener(OnSliderValueChanged);
            _model.OnChanged -= OnValueChanged;
        }
    }
}