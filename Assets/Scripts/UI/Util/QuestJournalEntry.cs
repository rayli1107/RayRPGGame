using ScriptableObjects;
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
        _labelQuestName.text = quest.name;
        _labelQuestStage.text = quest.questStages[questTracker.questStage].journalEntry;
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
