using ScriptedActions;
using System;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(
        fileName = "Player Profile",
        menuName = "ScriptableObjects/Player Profile")]
    public class PlayerProfile : AutoIdScriptableObject
    {
        [field: SerializeField]
        public Sprite face { get; private set; }

        [field: SerializeField]
        public int hp { get; private set; }

        [field: SerializeField]
        public int stamina { get; private set; }

        [field: SerializeField]
        public int attack { get; private set; }
    }
}