using ScriptableObjects;
using System;
using TMPro;
using UnityEngine;


public class QuestJournalEntry : MonoBehaviour
{
    /*
        [SerializeField]
        private TextMeshProUGUI _prefabHeader;

        [SerializeField]
        private TextMeshProUGUI _prefabActiveQuestStage;

        [SerializeField]
        private TextMeshProUGUI _prefabCompletedQuestStage;
    */
    [SerializeField]
    private TextMeshProUGUI _labelQuestName;

    [SerializeField]
    private TextMeshProUGUI _labelQuestStage;

    [SerializeField]
    private TextMeshProUGUI _labelQuestStageContext;

    [SerializeField]
    private RectTransform _panelQuestStageContext;

    public QuestTracker questTracker;

    private void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        QuestProfile quest;
        if (questTracker == null ||
            !QuestManager.Instance.questMap.TryGetValue(questTracker.questId, out quest))
        {
            return;
        }

        QuestStageProfile stageProfile = quest.questStages[questTracker.questStage];
        _labelQuestName.text = quest.name;
        _labelQuestStage.text = stageProfile.journalEntry;
        if (stageProfile.progressType == QuestProgressType.MONSTER_KILL_COUNT &&
            stageProfile.intFields.Length > 0)
        {
            _labelQuestStageContext.text = string.Format(
                "Kill Count: {0}/{1}",
                questTracker.intFields.Count > 0 ? questTracker.intFields[0] : 0,
                stageProfile.intFields[0]);
            _panelQuestStageContext.gameObject.SetActive(true);
        }
        else
        {
            _panelQuestStageContext.gameObject.SetActive(false);
        }
        /*
                foreach (Transform child in transform)
                {
                    Destroy(child.gameObject);
                }

                QuestProfile quest;
                if (questTracker == null ||
                    !QuestManager.Instance.questMap.TryGetValue(questTracker.questId, out quest))
                {
                    return;
                }

                TextMeshProUGUI labelHeader = Instantiate(_prefabHeader, transform);
                labelHeader.text = quest.name;*/
    }
}
