using OneEyedJoe.Components.ColliderBased;
using UnityEngine;

namespace OneEyedJoe.Components.Interactions
{
    public class TakingAnItem : MonoBehaviour
    {
        [SerializeField] private GameObject _hero;
        [SerializeField] private Transform _target;
        [SerializeField] private EnterEvent _action;


        public void TakeIt()
        {
            var instantiate = Instantiate(
                gameObject,
                _target.position,
                Quaternion.AngleAxis(-90, transform.forward),
                _hero.transform);
            instantiate.transform.localScale = _target.lossyScale;

            _action?.Invoke(gameObject);
        }
    }
}

