using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillController : PlayerActionController
{
    protected override void Invoke(PlayerController player)
    {
        base.Invoke(player);
        if (player.playerTriggeredAction == null && 
            player.TryAction(staminaCost))
        {
            player.playerTriggeredAction = this;
        }
    }
}
