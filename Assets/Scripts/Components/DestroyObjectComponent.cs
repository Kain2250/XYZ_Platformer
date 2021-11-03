using UnityEngine;

namespace PlayerController
{
    public class DestroyObjectComponent : MonoBehaviour
    {
        [SerializeField] private GameObject _objectDestroy;

        public void DestroyObject()
        {
            Destroy(_objectDestroy);

        }
    }
}

