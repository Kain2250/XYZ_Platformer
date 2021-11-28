using System;
using UnityEngine;
using UnityEngine.UI;

namespace OneEyedJoe.UI.Widgets
{
    public class CoinWidget : MonoBehaviour
    {
        [SerializeField] private Image _first;
        [SerializeField] private Image _second;
        [SerializeField] private Image _third;
        [SerializeField] private Sprite[] _sprites;

        private void Awake()
        {
            if (_first == null)
                Debug.Log("first null");
        }
        public void SetCoinCount(int count)
        {
            var newCount = count;
            var third = newCount % 10;
            newCount /= 10;
            var second = newCount % 10;
            newCount /= 10;
            var first = newCount % 10;

            if (_first) _first.sprite = _sprites[first];
            if (_second) _second.sprite = _sprites[second];
            if (_third) _third.sprite = _sprites[third];

        }
    }
}
