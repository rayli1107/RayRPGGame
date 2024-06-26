using ScriptableObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public Action updateAction;

    [field: SerializeField]
    public QuestProfile[] quests { get; private set; }

    public Dictionary<string, QuestProfile> questMap { get; private set; }
    public Dictionary<string, QuestProfile> startQuestNpcs { get; private set; }
    public Dictionary<string, QuestProfile> relatedQuestNpcs { get; private set; }


    public bool initialized { get; private set; }

    public static QuestManager Instance;

    private GameSessionData _gameData => GlobalDataManager.Instance.gameData;


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;

        questMap = new Dictionary<string, QuestProfile>();
        startQuestNpcs = new Dictionary<string, QuestProfile>();
        relatedQuestNpcs = new Dictionary<string, QuestProfile>();
    }

    // Start is called before the first frame update
    void Start()
    {
        questMap.Clear();
        startQuestNpcs.Clear();

        foreach (QuestProfile quest in quests)
        {
            questMap[quest.id] = quest;
            startQuestNpcs[quest.questGiver.id] = quest;
        }
        updateAction?.Invoke();
        initialized = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AdvanceQuest(QuestProfile quest, QuestTracker questTracker)
    {
        if (questTracker == null)
        {
            questTracker = new QuestTracker(quest.id);
            _gameData.currentQuests.Add(questTracker);
            startQuestNpcs.Remove(quest.questGiver.id);
        }
        else
        {
            foreach (AutoIdScriptableObject npc in quest.questStages[questTracker.questStage].relatedNpcs)
            {
                relatedQuestNpcs.Remove(npc.id);
            }
            ++questTracker.questStage;
        }
        foreach (AutoIdScriptableObject npc in quest.questStages[questTracker.questStage].relatedNpcs)
        {
            relatedQuestNpcs[npc.id] = quest;
        }

        if (questTracker.questStage == quest.questStages.Length - 1)
        {
            _gameData.currentQuests.RemoveAll(qt => qt.questId == quest.id);
            if (!_gameData.completedQuests.Contains(quest.id))
            {
                _gameData.completedQuests.Add(quest.id);
            }
        }

        updateAction?.Invoke();
    }

    public void AdvanceQuest(string questId)
    {
        QuestProfile quest;
        if (!questMap.TryGetValue(questId, out quest) ||
            _gameData.completedQuests.Contains(questId)) {
            return;
        }

        QuestTracker questTracker = _gameData.currentQuests.Find(qt => qt.questId == questId);
        AdvanceQuest(quest, questTracker);
    }

    public bool IsQuestCompleted(string questId)
    {
        return _gameData.completedQuests.Contains(questId);
    }

    public QuestTracker GetQuestProgress(string questId)
    {
        return _gameData.currentQuests.Find(qt => qt.questId == questId);
    }

    public bool IsQuestNotStarted(string questId)
    {
        return !IsQuestCompleted(questId) && GetQuestProgress(questId) == null;
    }
}
