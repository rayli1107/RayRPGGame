using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(
        fileName = "Enemy Profile",
        menuName = "ScriptableObjects/Enemy Profile")]
    public class EnemyProfile : ScriptableObject
    {
        public string enemyName;
        public int hp;
        public int damage;
        public bool canFlinch;
    }
}