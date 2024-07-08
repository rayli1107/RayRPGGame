using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillController : PlayerActionController
{
    [field: SerializeField]
    private Sprite _sprite;
    public override Sprite sprite => _sprite;

    [field: SerializeField]
    public int staminaCost { get; private set; }

    [field: SerializeField]
    public PlayerTriggeredActionType actionType { get; private set; }

    [field: SerializeField]
    public int damage { get; private set; }

    public override bool IsAvailable(PlayerController player)
    {
        bool result = base.IsAvailable(player) && player.CheckStaminaCost(staminaCost);
        return result;
    }

    protected override void Invoke(PlayerController player)
    {
        base.Invoke(player);
        if (player.playerTriggeredAction == null &&
            player.CheckStaminaCost(staminaCost))
        {
            player.playerTriggeredAction = this;
        }
    }

    public override void FinishInvoke(PlayerController player)
    {
        base.FinishInvoke(player);
        player.PayStaminaCost(staminaCost);
    }
}
