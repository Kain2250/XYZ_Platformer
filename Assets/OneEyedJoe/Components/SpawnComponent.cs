using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnComponent : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private GameObject _rpefab;

    [ContextMenu("Spawn")]
    public void Spawn()
    {
        var instantiate = Instantiate(_rpefab, _target.position, Quaternion.identity);
        instantiate.transform.localScale = _target.lossyScale;
    }
}
