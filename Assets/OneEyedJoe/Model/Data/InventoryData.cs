using System;

namespace OneEyedJoe.Model.Data
{
    [Serializable]
    public class InventoryData
    {
        public int Coin
        {
            get => _coin;
            set => _coin = value;
        }

        public bool IsArmed
        {
            get => _isArmed;
            set => _isArmed = value;
        }

        private int _coin;

        private bool _isArmed;

    }
}
