using ScriptableObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActionManager : MonoBehaviour
{
    [field: SerializeField]
    public PlayerActionController[] playerActions { get; private set; }

    public static PlayerActionManager Instance;

    public PlayerInput playerInput { get; private set; }
    public List<InputAction> inputActions { get; private set; }


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;

        playerInput = GetComponent<PlayerInput>();

        inputActions = new List<InputAction>();
        for (int i = 0; i < playerActions.Length; ++i)
        {
            inputActions.Add(playerInput.actions[string.Format("Hotkey {0}", i + 1)]);
        }
    }

    private void Update()
    {
        
    }

}
