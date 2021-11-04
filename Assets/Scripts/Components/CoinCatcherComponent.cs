using UnityEngine;

namespace PlayerController
{
    public class CoinCatcherComponent : MonoBehaviour
    {
        [SerializeField] private int _nominal;
        [SerializeField] private Hero _hero;

        public void CoinCatch()
        {
            _hero.AddMoney(_nominal);
            Debug.Log("In the piggy bank +" + _nominal + " coins. Total: " + Hero._money);
        }
    }
}

