using ScriptableObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalDataManager : MonoBehaviour
{
    [field: SerializeField]
    public PlayerProfile playerProfile { get; private set; }

    [field: SerializeField]
    public GameSessionData gameData { get; private set; }

    public Vector3 NextScenePlayerPosition;
    public Quaternion NextScenePlayerRotation;

    public bool initialized { get; private set; }

    public static GlobalDataManager Instance;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        gameData = new GameSessionData(playerProfile);
        initialized = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
