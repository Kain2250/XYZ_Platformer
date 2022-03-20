using OneEyedJoe.Creatures;
using OneEyedJoe.Creatures.Hero;
using UnityEngine;

namespace OneEyedJoe.Components
{
    public class CoinComponent : MonoBehaviour
    {
        [SerializeField] private int _nominal;
        private Hero _hero;

        private void Start()
        {
            _hero = FindObjectOfType<Hero>();
        }
        public void AddMoney()
        {
            _hero.AddMoney(_nominal);
        }
    }
}

