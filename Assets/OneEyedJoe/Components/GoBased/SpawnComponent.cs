using UnityEngine;

namespace OneEyedJoe.Components.GoBased
{
    public class SpawnComponent : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private GameObject _rpefab;

        [ContextMenu("Spawn")]
        public void Spawn()
        {
            var instance = Instantiate(_rpefab, _target.position, Quaternion.identity);
            instance.transform.localScale = _target.lossyScale;
            instance.SetActive(true);
        }
    }
}
