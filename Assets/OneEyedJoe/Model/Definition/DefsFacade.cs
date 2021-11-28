using System;
using UnityEngine;

namespace OneEyedJoe.Model.Definition
{
    [CreateAssetMenu(fileName = "DefsFacade", menuName = "DefsFacade", order = 0)]
    public class DefsFacade : ScriptableObject
    {
        [SerializeField] private PlayerDef _player;

        public PlayerDef Player => _player;

        private static DefsFacade _instance;
        public static DefsFacade I => _instance == null ? LoadDefs() : _instance;

        private static DefsFacade LoadDefs()
        {
            return _instance = Resources.Load<DefsFacade>("DefsFacade");
        }

    }
}