using UnityEngine;

namespace PlayerController
{
    public class CoinCatcherComponent : MonoBehaviour
    {
        [SerializeField] private int _nominal;

        public void CoinCatch()
        {
            Debug.Log("In the piggy bank +" + _nominal + " coins");
        }
    }
}

