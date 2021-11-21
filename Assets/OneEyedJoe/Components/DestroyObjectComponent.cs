using UnityEngine;

namespace OneEyedJoe.Components
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

