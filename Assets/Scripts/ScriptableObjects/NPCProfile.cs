using ScriptedActions;
using System;
using UnityEngine;

namespace ScriptableObjects
{
    [Serializable]
    public class ScriptedBehaviourEntry
    {
        [field: SerializeField]
        public int priority { get; private set; }

        [field: SerializeField]
        public Prerequisite prerequisite { get; private set; }

        [field: SerializeField]
        public ScriptedAction[] actions { get; private set; }
    }

    [CreateAssetMenu(
        fileName = "NPC Profile",
        menuName = "ScriptableObjects/NPC Profile")]
    public class NPCProfile : AutoIdScriptableObject
    {
        [field: SerializeField]
        public Sprite face { get; private set; }

        [field: SerializeField]
        public ScriptedBehaviourEntry[] behaviourEntries { get; private set; }
    }
}