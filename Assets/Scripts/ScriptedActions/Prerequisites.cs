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
            GameSessionData gameData = GlobalDataManager.Instance.gameData;

            switch (entry.prerequisiteType)
            {
                case PrerequisiteType.QUEST_NOT_STARTED:
                    return entry.objectFields.Length > 0 &&
                        QuestManager.Instance.IsQuestNotStarted(entry.objectFields[0].id);

                case PrerequisiteType.QUEST_IN_PROGRESS:
                    bool result = false;
                    if (entry.objectFields.Length >= 0)
                    {
                        string questId = entry.objectFields[0].id;
                        QuestTracker questTracker = QuestManager.Instance.GetQuestProgress(questId);
                        if (questTracker != null)
                        {
                            if (entry.intFields.Length > 0)
                            {
                                result = entry.intFields[0] == questTracker.questStage;
                            }
                            else
                            {
                                result = true;
                            }
                        }
                    }
                    return result;

                case PrerequisiteType.QUEST_COMPLETED:
                    return entry.objectFields.Length > 0 &&
                        QuestManager.Instance.IsQuestCompleted(entry.objectFields[0].id);

                case PrerequisiteType.NONE:
                    return true;
            }
            return false;
        }
    }
}