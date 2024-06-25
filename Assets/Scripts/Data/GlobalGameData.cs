using ScriptableObjects;
using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class QuestTracker
{
    [SerializeField, HideInInspector]
    public string questId;
    public int questStage;
    public List<int> intFields;
}
public class QuestProgressMap : SerializedDictionary<string, int> { }

[Serializable]
public class QuestSet : SerializedHashSet<string> { }

[Serializable]
public class MonsterKillCounter : SerializedDictionary<string, int> { }

[Serializable]
public class GameSessionData
{
    [SerializeField, HideInInspector]
    private PlayerGameUnit _playerData;
    public PlayerGameUnit playerData => _playerData;

    [SerializeField, HideInInspector]
    private List<QuestTracker> _currentQuests;
    public List<QuestTracker> currentQuests => _currentQuests;

    [SerializeField, HideInInspector]
    private List<string> _completedQuests;
    public List<string> completedQuests => _completedQuests;

    [SerializeField, HideInInspector]
    private MonsterKillCounter _monsterKillCounter;
    public MonsterKillCounter monsterKillCounter => _monsterKillCounter;

    public GameSessionData(PlayerProfile profile)
    {
        _playerData = new PlayerGameUnit(profile);
        _currentQuests = new List<QuestTracker>();
        _completedQuests = new List<string>();
        _monsterKillCounter = new MonsterKillCounter();
    }
}
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