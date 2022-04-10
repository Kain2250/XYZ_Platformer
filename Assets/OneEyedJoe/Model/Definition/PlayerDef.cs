using UnityEngine;

namespace OneEyedJoe.Model.Definition
{
    [CreateAssetMenu(menuName= "Defs/PlayerDef", fileName = "PlayerDef", order = 0)]
    public class PlayerDef : ScriptableObject
    {
        [SerializeField] private int _maxHealth;
        [SerializeField] private int _sizeInventory;

        public int MaxHealth => _maxHealth;
        public int SizeInventory => _sizeInventory;
    }
}
