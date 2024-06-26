using System;
using UnityEngine;

namespace ScriptableObjects
{
    public enum QuestProgressType
    {
        MONSTER_KILL_COUNT,
        NONE,
    }

    [Serializable]
    public class QuestStageProfile
    {
        [field: SerializeField]
        public QuestProgressType progressType { get; private set; }

        [field: SerializeField]
        public string journalEntry { get; private set; }

        [field: SerializeField]
        public AutoIdScriptableObject[] relatedNpcs { get; private set; }

        [field: SerializeField]
        public AutoIdScriptableObject[] objectFields { get; private set; }

        [field: SerializeField]
        public string[] stringFields { get; private set; }

        [field: SerializeField]
        public int[] intFields { get; private set; }
    }

    [CreateAssetMenu(
        fileName = "Quest Profile",
        menuName = "ScriptableObjects/Quest Profile")]
    public class QuestProfile : AutoIdScriptableObject
    {
        [field: SerializeField]
        public NPCProfile questGiver { get; private set; }

        [field: SerializeField]
        public QuestStageProfile[] questStages { get; private set; }

        [field: SerializeField]
        public int questRewardCoin { get; private set; }

        [field: SerializeField]
        public int questRewardExp { get; private set; }
    }
}