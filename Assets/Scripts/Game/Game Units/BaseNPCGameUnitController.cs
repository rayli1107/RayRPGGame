using UnityEngine;

public class BaseNPCGameUnitController : BaseGameUnitController
{
    protected PlayerController player => GameController.Instance.player;

    private bool _inTrigger;
    public bool inTrigger
    {
        get => _inTrigger;
        set
        {
            bool changed = _inTrigger != value;
            _inTrigger = value;
            if (changed && canTrigger)
            {
                if (_inTrigger)
                {
                    player.RegisterTarget(this);
                }
                else
                {
                    player.UnregisterTarget(this);
                }
            }
        }
    }

    private bool _canTrigger;
    public bool canTrigger
    {
        get => _canTrigger;
        set
        {
            bool changed = _canTrigger != value;
            _canTrigger = value;
            if (changed && inTrigger)
            {
                if (_canTrigger)
                {
                    player.RegisterTarget(this);
                }
                else
                {
                    player.UnregisterTarget(this);
                }
            }
        }
    }

    protected virtual void OnEnable()
    {
        canTrigger = true;
        inTrigger = false;
    }

    protected virtual void OnDisable()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == player.attackHitBox)
        {
            inTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == player.attackHitBox)
        {
            inTrigger = false;
        }
    }
}
