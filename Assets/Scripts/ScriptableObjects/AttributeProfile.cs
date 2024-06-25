using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(
        fileName = "Attribute Profile",
        menuName = "ScriptableObjects/Attribute Profile")]
    public class AttributeProfile : ScriptableObject
    {
        public string attributeName;
        public string shortName;
        public int baseValue;
        public int multiplier; 

        public int CalculateValue(int level)
        {
            return baseValue + level * multiplier;
        }
    }
}