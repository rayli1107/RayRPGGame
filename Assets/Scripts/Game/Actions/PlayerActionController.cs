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

    private bool _isTriggered;



    public void Trigger()
    {
        _isTriggered = true;
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
}
