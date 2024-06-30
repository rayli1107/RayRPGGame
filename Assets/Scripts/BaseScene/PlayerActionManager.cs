using ScriptableObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActionManager : MonoBehaviour
{
    [field: SerializeField]
    public PlayerActionController[] playerHotkeyActions { get; private set; }

    [field: SerializeField]
    public PlayerActionController playerAttackAction { get; private set; }

    public static PlayerActionManager Instance;

    public PlayerInput playerInput { get; private set; }
    private List<InputAction> _inputHotkeyActions;
    private InputAction _inputAttackAction;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;

        playerInput = GetComponent<PlayerInput>();

        _inputHotkeyActions = new List<InputAction>();
        for (int i = 0; i < playerHotkeyActions.Length; ++i)
        {
            InputAction inputAction = playerInput.actions[string.Format("Hotkey {0}", i + 1)];
            Debug.Log(inputAction);
            _inputHotkeyActions.Add(inputAction);
        }

        _inputAttackAction = playerInput.actions["Attack"];
    }

    private void checkTriggerAction(InputAction inputAction, PlayerActionController playerAction)
    {
        if (inputAction.triggered)
        {
            playerAction.Trigger();
        }
    }

    private void Update()
    {
        for (int i = 0; i < Mathf.Min(_inputHotkeyActions.Count, playerHotkeyActions.Length); ++i)
        {
            checkTriggerAction(_inputHotkeyActions[i], playerHotkeyActions[i]);
        }
        checkTriggerAction(_inputAttackAction, playerAttackAction);
    }

    public void RunActions(PlayerController player)
    {
        playerAttackAction.InvokeIfTriggered(player);
        foreach (PlayerActionController action in playerHotkeyActions)
        {
            action.InvokeIfTriggered(player);
        }
    }
}
