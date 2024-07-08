using UnityEngine;

namespace ScriptableObjects
{
    public enum ItemType
    {
        CONSUMABLE,
    }

    [CreateAssetMenu(
        fileName = "Item Profile",
        menuName = "ScriptableObjects/Item Profile")]
    public class ItemProfile : AutoIdScriptableObject
    {
        [field: SerializeField]
        public Sprite sprite { get; private set; }

        [field: SerializeField]
        public ItemType itemType { get; private set; }

        [field: SerializeField]
        public int hpRestoredFlat { get; private set; }

        [field: SerializeField]
        public int staminaRestoredFlat { get; private set; }

        [field: SerializeField]
        public int maxStack { get; private set; }

    }
}