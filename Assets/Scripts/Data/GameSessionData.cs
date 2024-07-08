using ScriptableObjects;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class QuestTracker
{
    [field: SerializeField]
    public string questId { get; private set; }

    [SerializeField]
    private int _questStage;
    public int questStage
    {
        get => _questStage;
        set
        {
            if (_questStage != value)
            {
                intFields.Clear();
            }
            _questStage = value;
        }
    }

    public List<int> intFields;

    public QuestTracker(string questId)
    {
        this.questId = questId;
        questStage = 0;
        intFields = new List<int>();
    }
}
public class QuestProgressMap : SerializedDictionary<string, int> { }

[Serializable]
public class QuestSet : SerializedHashSet<string> { }

[Serializable]
public class MonsterKillCounter : SerializedDictionary<string, int> { }

[Serializable]
public class GameSessionData
{
    [field: SerializeField]
    public PlayerData playerData { get; private set; }

    [field: SerializeField]
    public Inventory inventory { get; private set; }

    [field: SerializeField]
    public List<QuestTracker> currentQuests { get; private set; }

    [field: SerializeField]
    public List<string> completedQuests { get; private set; }

    [field: SerializeField]
    public MonsterKillCounter monsterKillCounter { get; private set; }

    public GameSessionData(PlayerProfile profile)
    {
        playerData = new PlayerData(profile);
        inventory = new Inventory();
        currentQuests = new List<QuestTracker>();
        completedQuests = new List<string>();
        monsterKillCounter = new MonsterKillCounter();
    }

    public void RegisterMonsterKill(string enemyId)
    {
        monsterKillCounter[enemyId] = monsterKillCounter.GetValueOrDefault(enemyId, 0) + 1;

        foreach (QuestTracker questTracker in currentQuests)
        {
            QuestProfile quest;
            if (QuestManager.Instance.questMap.TryGetValue(questTracker.questId, out quest))
            {
                QuestStageProfile stage = quest.questStages[questTracker.questStage];
                if (stage.progressType == QuestProgressType.MONSTER_KILL_COUNT &&
                    stage.objectFields.Length > 0 &&
                    stage.objectFields[0].id == enemyId &&
                    stage.intFields.Length > 0)
                {
                    int count;
                    if (questTracker.intFields.Count == 0)
                    {
                        questTracker.intFields.Add(1);
                        count = 1;
                    }
                    else
                    {
                        count = ++questTracker.intFields[0];
                    }

                    if (count >= stage.intFields[0])
                    {
                        QuestManager.Instance.AdvanceQuest(quest, questTracker);
                    }
                }
            }
        }
    }
}
/*
public class GlobalGameData
{
    public PlayerProfile playerProfile { get; private set; }
    public GameSessionData gameData { get; private set; }
    public Vector3 NextScenePlayerPosition;
    public Quaternion NextScenePlayerRotation;

    public GlobalGameData(PlayerProfile profile)
    {
        playerProfile = profile;
        gameData = new GameSessionData(profile);
    }

    public static GlobalGameData Instance { get; private set; }

    public static void Initialize(PlayerProfile profile)
    {
        Instance = new GlobalGameData(profile);
    }
}
*/
