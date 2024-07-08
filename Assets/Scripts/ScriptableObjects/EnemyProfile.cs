using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(
        fileName = "Enemy Profile",
        menuName = "ScriptableObjects/Enemy Profile")]
    public class EnemyProfile : AutoIdScriptableObject
    {
        [field: SerializeField]
        public int hp { get; private set; }

        [field: SerializeField]
        public int damage { get; private set; }

        [field: SerializeField]
        public bool canFlinch { get; private set; }

        [field: SerializeField]
        public int exp { get; private set; }

        [field: SerializeField]
        public int staggerValue { get; private set; }

        [field: SerializeField]
        public int staggerDuration { get; private set; }
    }
}