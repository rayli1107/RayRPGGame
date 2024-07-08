using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerTriggeredActionType
{
    NONE,
    ROLL,
    ATTACK,
    SPIN_ATTACK
}

public class PlayerActionController : MonoBehaviour
{
    [HideInInspector]
    public Action updateAction;

    [field: SerializeField]
    public bool autoFinishInvoke { get; private set; }

    [field: SerializeField]
    public float coolDown { get; private set; }

    private float _cooldownRemaining;
    public float cooldownRemaining
    {
        get => _cooldownRemaining;
        set
        {
            if (value > 0)
            {
                Debug.LogFormat("Cooldown Remaining = {0}", value);
            }
            _cooldownRemaining = value;
            updateAction?.Invoke();
        }
    }

    public virtual Sprite sprite => null;
    public virtual bool hasStackableCount => false;

    private bool _isTriggered;

    public PlayerActionController()
    {
        autoFinishInvoke = true;
    }

    public void Trigger(PlayerController player)
    {
        if (IsAvailable(player))
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

    public void InvokeIfAvailable(PlayerController player)
    {
        if (IsAvailable(player))
        {
            Invoke(player);
        }
    }

    protected virtual void Invoke(PlayerController player)
    {

    }

    public virtual void FinishInvoke(PlayerController player)
    {

    }

    public virtual bool IsAvailable(PlayerController player)
    {
        return _cooldownRemaining <= 0f;
    }

    public void EnterCooldown()
    {
        cooldownRemaining = coolDown;
    }

    private void Update()
    {
        cooldownRemaining = Mathf.Max(cooldownRemaining - Time.deltaTime, 0f);
    }

    public virtual int GetStackCount()
    {
        return 0;
    }
}
