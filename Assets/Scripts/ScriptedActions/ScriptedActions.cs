using ScriptableObjects;
using System;
using UnityEngine;

namespace ScriptedActions
{
    public delegate void ScriptedActionCallback();

    public enum ActionType
    {
        NPC_DIALOGUE,
        PLAYER_DIALOGUE,
        ADVANCE_QUEST,
    }

    [Serializable]
    public class ScriptedAction
    {
        [field: SerializeField]
        public ActionType actionType { get; private set; }

        [field: SerializeField]
        public string[] stringFields { get; private set; }

        [field: SerializeField]
        public AutoIdScriptableObject[] objectFields { get; private set; }
    }

    public class ScriptedActionUtil
    {
        public static void Run(
            BaseGameUnitController gameUnit,
            ScriptedAction action,
            ScriptedActionCallback callback=null)
        {
            switch (action.actionType)
            {
                case ActionType.NPC_DIALOGUE:
                    runDialogueAction(gameUnit, action, 0, callback);
                    break;

                case ActionType.PLAYER_DIALOGUE:
                    runDialogueAction(GameController.Instance.player, action, 0, callback);
                    break;

                case ActionType.ADVANCE_QUEST:
                    if (action.objectFields.Length > 0)
                    {

                        QuestManager.Instance.AdvanceQuest(action.objectFields[0].id);
                        callback?.Invoke();
                    }
                    break;

                default:
                    break;
            }
        }


        private static void runDialogueAction(
            BaseGameUnitController gameUnit,
            ScriptedAction action,
            int index,
            ScriptedActionCallback callback=null)
        {
            if (index >= action.stringFields.Length)
            {
                callback?.Invoke();
                return;
            }

            GameUIManager.Instance.ShowMessageBox(
                gameUnit,
                action.stringFields[index],
                () => runDialogueAction(gameUnit, action, index + 1, callback));
        }
    }
}