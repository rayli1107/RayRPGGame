using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(
        fileName = "Player Profile",
        menuName = "ScriptableObjects/Player Profile")]
    public class PlayerProfile : ScriptableObject
    {
        public string enemyName;
        public int hp;
        public int damage;
        public bool canFlinch;
    }
}