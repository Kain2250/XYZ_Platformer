using UnityEngine;

namespace OneEyedJoe.Components.GoBased
{
    public class SpawnDropComponent : MonoBehaviour
    {
        [SerializeField] private Transform _target;

        [ContextMenu("SpawnDrop")]
        public void Spawn(GameObject go)
        {
            var instance = Instantiate(go, _target.position, Quaternion.identity);
            instance.transform.localScale = _target.lossyScale;
            instance.SetActive(true);
        }

    }
}