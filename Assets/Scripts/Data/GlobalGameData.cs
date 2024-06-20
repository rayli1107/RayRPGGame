using UnityEditor;
using UnityEngine;

public class GlobalGameData
{
    public PlayerGameUnit PlayerData { get; private set; }

    public Vector3 NextScenePlayerPosition;
    public Quaternion NextScenePlayerRotation;

    public GlobalGameData()
    {
        PlayerData = new PlayerGameUnit();
    }

    public static GlobalGameData Instance { get; private set; }

    public static void Initialize()
    {
        Instance = new GlobalGameData();
    }
}