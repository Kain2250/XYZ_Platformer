using UnityEngine;

namespace OneEyedJoe.Components.GoBased
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

