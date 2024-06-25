using ScriptableObjects;
using System;
using UnityEngine;

namespace ScriptedActions
{
    public delegate void ScriptedActionCallback();

    public enum ActionType
    {
        NPC_DIALOGUE,
        PLAYER_DIALOGUE
    }

    [Serializable]
    public class ScriptedAction
    {
        [SerializeField]
        private ActionType _actionType;
        public ActionType actionType => _actionType;

        [SerializeField]
        private string[] _stringFields;
        public string[] stringFields => _stringFields;

        [SerializeField]
        private AutoIdScriptableObject[] _objectFields;
        public AutoIdScriptableObject[] objectFields;
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