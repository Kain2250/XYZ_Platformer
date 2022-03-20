using UnityEngine;
using UnityEngine.UI;

namespace OneEyedJoe.UI.Widgets
{
    public class WeaponWidget : MonoBehaviour
    {
        [SerializeField] private Image _first;
        [SerializeField] private Image _second;
        [SerializeField] private Sprite[] _sprites;

        private void Awake()
        {
            if (_first == null)
                Debug.Log("first null");
        }
        public void SetWeaponCount(int count)
        {
            var newCount = count;
            var second = newCount % 10;
            newCount /= 10;
            var first = newCount % 10;

            if (_first) _first.sprite = _sprites[first];
            if (_second) _second.sprite = _sprites[second];

        }

    }
}