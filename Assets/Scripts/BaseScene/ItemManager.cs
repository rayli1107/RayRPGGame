using ScriptableObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [field: SerializeField]
    public ItemProfile[] items { get; private set; }

    public Dictionary<string, ItemProfile> itemMap { get; private set; }


    public bool initialized { get; private set; }

    public static ItemManager Instance;


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;

        itemMap = new Dictionary<string, ItemProfile>();
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (ItemProfile item in items)
        {
            itemMap[item.id] = item;
        }
        initialized = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
