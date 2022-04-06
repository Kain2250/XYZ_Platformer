using UnityEngine;

namespace OneEyedJoe.Model.Definition
{
    [CreateAssetMenu(menuName= "Defs/PlayerDef", fileName = "PlayerDef", order = 0)]
    public class PlayerDef : ScriptableObject
    {
        [SerializeField] private int _maxHealth;

        public int MaxHealth => _maxHealth;
    }
}
