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

    public BaseNPCGameUnitController() : base()
    {

    }

    protected override void OnEnable()
    {
        base.OnEnable();
        canTrigger = true;
        inTrigger = false;
    }

    protected virtual void OnDisable()
    {
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other == player.targetCollider)
        {
            inTrigger = true;
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other == player.targetCollider)
        {
            inTrigger = false;
        }
    }
}
