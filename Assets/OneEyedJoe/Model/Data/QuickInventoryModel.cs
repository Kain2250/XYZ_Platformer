using System;
using OneEyedJoe.Model.Data.Properties;
using OneEyedJoe.Model.Definition;
using OneEyedJoe.Utils.Disposables;
using UnityEngine;

namespace OneEyedJoe.Model.Data
{
    public class QuickInventoryModel : IDisposable
    {
        private readonly PlayerData _data;

        public readonly IntProperty SelectedIndex = new IntProperty();
        public InventoryItemData[] Inventory { get; private set; }

        public event Action OnChanged;

        public InventoryItemData SelectedItem => Inventory[SelectedIndex.Value];
        public QuickInventoryModel(PlayerData data)
        {
            _data = data;

            Inventory = _data.Inventory.GetAll(ItemTag.Usable);
            _data.Inventory.OnChanged += OnChangedInventory;
        }

        public IDisposable Subscribe(Action call)
        {
            OnChanged += call;
            return new ActionDisposable(() => OnChanged -= call);
        }

        private void OnChangedInventory(string id, int countValue)
        {
            Inventory = _data.Inventory.GetAll(ItemTag.Usable);
            SelectedIndex.Value = Mathf.Clamp(SelectedIndex.Value, 0, Inventory.Length - 1);   
            OnChanged?.Invoke();
        }

        public void SetNextItem()
        {
            SelectedIndex.Value = (int) Mathf.Repeat(SelectedIndex.Value + 1, Inventory.Length);
        }

        public void Dispose()
        {
            _data.Inventory.OnChanged -= OnChangedInventory;
        }
    }
}