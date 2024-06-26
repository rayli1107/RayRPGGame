using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestJournalController : MonoBehaviour
{
    [SerializeField]
    private Transform _questJournalPanel;

    [SerializeField]
    private QuestJournalEntry _prefabQuestJournalEntry;

    private GameSessionData _gameData => GlobalDataManager.Instance.gameData;

    private void OnEnable()
    {
        foreach (Transform child in _questJournalPanel)
        {
            if (child.gameObject != _prefabQuestJournalEntry.gameObject &&
                child.GetComponent<QuestJournalEntry>() != null)
            {
                Destroy(child.gameObject);
            }
        }

        foreach (QuestTracker questTracker in _gameData.currentQuests)
        {
            QuestJournalEntry entry = Instantiate(_prefabQuestJournalEntry, _questJournalPanel);
            entry.questTracker = questTracker;
            entry.gameObject.SetActive(true);
        }
    }

    private void OnDisable()
    {
    }

}
