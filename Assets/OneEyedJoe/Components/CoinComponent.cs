using UnityEngine;

namespace OneEyedJoe.Components
{
    public class CoinComponent : MonoBehaviour
    {
        [SerializeField] private int _nominal;
        [SerializeField] private Hero _hero;

        public void AddMoney()
        {
            _hero.AddMoney(_nominal);
            Debug.Log("In the piggy bank +" + _nominal + " coins. Total: " + Hero._money);
        }
    }
}

