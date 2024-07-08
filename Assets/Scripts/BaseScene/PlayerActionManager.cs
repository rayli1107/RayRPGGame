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

    [field: SerializeField]
    public PlayerActionController playerRollAction { get; private set; }

    public static PlayerActionManager Instance;

    [HideInInspector]
    public PlayerController playerController;

    public PlayerInput playerInput { get; private set; }
    private List<InputAction> _inputHotkeyActions;
    private InputAction _inputAttackAction;
    private InputAction _inputActionAction;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;
        playerInput = GetComponent<PlayerInput>();

        _inputHotkeyActions = new List<InputAction>();
        for (int i = 0; i < playerHotkeyActions.Length; ++i)
        {
            InputAction inputAction = playerInput.actions[string.Format("Hotkey {0}", i + 1)];
            _inputHotkeyActions.Add(inputAction);
        }

        _inputAttackAction = playerInput.actions["Attack"];
        _inputActionAction = playerInput.actions["Action"];
    }

    private void checkTriggerAction(
        InputAction inputAction, PlayerActionController playerAction, string label)
    {
        if (inputAction.triggered && playerController != null)
        {
            playerAction.Trigger(playerController);
        }
    }

    private void Update()
    {
        for (int i = 0; i < Mathf.Min(_inputHotkeyActions.Count, playerHotkeyActions.Length); ++i)
        {
            checkTriggerAction(_inputHotkeyActions[i], playerHotkeyActions[i], string.Format("hotkey {0}", i));
        }
        checkTriggerAction(_inputAttackAction, playerAttackAction, "attack");
        checkTriggerAction(_inputActionAction, playerRollAction, "roll");

    }

    private void runActionIfTriggered(
        PlayerController player,
        InputAction inputAction,
        PlayerActionController playerAction)
    {
        if (inputAction.triggered)
        {
            Debug.Log("IsTriggered");
            playerAction.InvokeIfAvailable(player);
        }
    }

    public void RunActions(PlayerController player)
    {
//        Debug.Log("Checking Attack Action");
//        runActionIfTriggered(player, _inputAttackAction, playerAttackAction);
//        Debug.Log("Checking Roll Action");
//        runActionIfTriggered(player, _inputActionAction, playerRollAction);

        playerAttackAction.InvokeIfTriggered(player);
        playerRollAction.InvokeIfTriggered(player);
        foreach (PlayerActionController action in playerHotkeyActions)
        {
            action.InvokeIfTriggered(player);
        }
    }
}
