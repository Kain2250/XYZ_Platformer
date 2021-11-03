using UnityEngine;

namespace PlayerController
{
    public class CoinCatcherComponent : MonoBehaviour
    {
        [SerializeField] private int _nominal;

        public void CoinCatch()
        {
            Hero.money += _nominal;
            Debug.Log("In the piggy bank +" + _nominal + " coins. Total: " + Hero.money);
        }
    }
}

