using ScriptableObjects;
using System;
using UnityEngine;

namespace ScriptedActions
{
    public enum PrerequisiteType
    {
        QUEST_COMPLETED,
        QUEST_IN_PROGRESS,
        QUEST_NOT_STARTED,
        NONE,
    }

    [Serializable]
    public class Prerequisite
    {
        [field: SerializeField]
        public PrerequisiteType prerequisiteType { get; private set; }

        [field: SerializeField]
        public string[] stringFields { get; private set; }

        [field: SerializeField]
        public int[] intFields { get; private set; }

        [field: SerializeField]
        public AutoIdScriptableObject[] objectFields { get; private set; }
    }

    public class PrerequisiteUtil
    {
        public static bool Check(Prerequisite entry)
        {
            switch (entry.prerequisiteType)
            {
                case PrerequisiteType.QUEST_NOT_STARTED:
                    return entry.objectFields.Length > 0 &&
                        QuestManager.Instance.IsQuestNotStarted(entry.objectFields[0].id);

                case PrerequisiteType.NONE:
                    return true;
            }
            return false;
        }
    }
}