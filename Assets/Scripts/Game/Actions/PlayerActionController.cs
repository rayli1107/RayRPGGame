using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerTriggeredActionType
{
    NONE,
    ATTACK,
    SPIN_ATTACK
}

public class PlayerActionController : MonoBehaviour
{
    [HideInInspector]
    public Action updateAction;

    [field: SerializeField]
    public Sprite sprite { get; private set; }


    [field: SerializeField]
    public int staminaCost { get; private set; }

    [field: SerializeField]
    public float coolDown { get; private set; }

    [field: SerializeField]
    public PlayerTriggeredActionType actionType { get; private set; }

    [field: SerializeField]
    public int damage { get; private set; }

    private float _cooldownRemaining;
    public float cooldownRemaining
    {
        get => _cooldownRemaining;
        set
        {
            _cooldownRemaining = value;
            updateAction?.Invoke();
        }
    }

    private PlayerGameUnit _playerData => GlobalDataManager.Instance.gameData.playerData;
    public bool available => _cooldownRemaining <= 0f && staminaCost <= _playerData.stamina;

    private bool _isTriggered;


    public void Trigger()
    {
        if (available)
        {
            _isTriggered = true;
        }
    }

    private bool getAndResetTrigger()
    {
        bool result = _isTriggered;
        _isTriggered = false;
        return result;
    }

    public void InvokeIfTriggered(PlayerController player) 
    {
        if (getAndResetTrigger())
        {
            Invoke(player);
        }
    }

    protected virtual void Invoke(PlayerController player)
    {

    }

    public void EnterCooldown()
    {
        cooldownRemaining = coolDown;
    }

    private void Update()
    {
        cooldownRemaining = Mathf.Max(cooldownRemaining - Time.deltaTime, 0f);
    }
}
