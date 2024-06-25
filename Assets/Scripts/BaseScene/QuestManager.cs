using ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [field: SerializeField]
    public QuestProfile quests { get; private set; }


    public bool initialized { get; private set; }

    public static QuestManager Instance;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        initialized = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
