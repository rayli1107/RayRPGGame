using ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemActionController : PlayerActionController
{
    [field: SerializeField]
    public ItemProfile itemProfile { get; private set; }

    private Inventory _inventory => GlobalDataManager.Instance.gameData.inventory;

    public override Sprite sprite => itemProfile.sprite;
    public override bool hasStackableCount => itemProfile.maxStack > 1;

    public override bool IsAvailable(PlayerController player)
    {
        return base.IsAvailable(player) && GetStackCount() > 0;
    }

    protected override void Invoke(PlayerController player)
    {
        base.Invoke(player);

        if (_inventory.RemoveItem(itemProfile.id, 1) > 0)
        {
            player.playerUnit.HP.value += itemProfile.hpRestoredFlat;
            player.playerUnit.Stamina.value += itemProfile.staminaRestoredFlat;
        }
    }

    public override int GetStackCount()
    {
        if (_inventory != null)
        {
            return _inventory.GetItemCount(itemProfile.id);
        }
        return 0;
    }
}
