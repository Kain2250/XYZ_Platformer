using UnityEngine;

namespace OneEyedJoe.Model.Data.Properties
{
    public abstract class ObservableProperty<TPropertyType>
    {
        [SerializeField] protected TPropertyType _value;
        protected TPropertyType _stored;
        private TPropertyType _defaultValue;

        public delegate void OnPropertyChanged(TPropertyType newValue, TPropertyType oldValue);

        public ObservableProperty(TPropertyType defaultValue)
        {
            _defaultValue = defaultValue;
        }

        public event OnPropertyChanged OnChanged;

        public TPropertyType Value
        {
            get => _stored;
            set
            {
                var isEquals = _stored.Equals(value);
                if (isEquals) return;

                var oldValue = _value;
                Write(value);
                _stored = _value = value;
                
                OnChanged?.Invoke(value, oldValue);
            }
        }

        protected void Init()
        {
            _stored = _value = Read(_defaultValue);
        }

        protected abstract void Write(TPropertyType value);
        protected abstract TPropertyType Read(TPropertyType defaultValue);
        
        public void Validate()
        {
            if (!_stored.Equals(_value))
                Value = _value;
        }

    }
}