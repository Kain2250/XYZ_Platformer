using System;
using System.Collections.Generic;
using OneEyedJoe.Model;
using OneEyedJoe.Utils.Disposables;
using UnityEngine;

namespace OneEyedJoe.UI.Hud.QuickInventory
{
    public class QuickInventoryController : MonoBehaviour
    {
        [SerializeField] private Transform _container;
        [SerializeField] private InventoryItemWidget _prefab;

        private readonly CompositeDisposable _trash = new CompositeDisposable();
        
        private GameSession _session;
        private List<InventoryItemWidget> _createItem = new List<InventoryItemWidget>();

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            _trash.Retain(_session.QuickInventory.Subscribe(Rebuild));
            Rebuild();
        }

        private void Rebuild()
        {
            var inventory = _session.QuickInventory.Inventory;

            for (var i = _createItem.Count; i < inventory.Length; i++)
            {
                var item = Instantiate(_prefab, _container);
                _createItem.Add(item);
            }

            for (var i = 0; i < inventory.Length; i++)
            {
                _createItem[i].SetData(inventory[i], i);
                _createItem[i].gameObject.SetActive(true);
            }

            for (var i = inventory.Length; i < _createItem.Count; i++)
            {
                _createItem[i].gameObject.SetActive(false);
            }
            
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}