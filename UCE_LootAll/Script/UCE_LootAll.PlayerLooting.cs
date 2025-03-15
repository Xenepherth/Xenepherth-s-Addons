// =======================================================================================
// Maintained by bobatea#9400 on Discord
// Usable for both personal and commercial projects, but no sharing or re-sale
// * Discord Support Server.............: 
 
// * Leave a star on my Github Repo.....: https://github.com/breehuynh/Bree-mmorpg-tools
// * Instructions.......................: https://indie-mmo.net/knowledge-base/
// =======================================================================================
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

public partial class PlayerLooting
{
    public void TakeAllLootItem()
    {
        CmdTakeGold();
        if (player.target is Monster monster)
        {
            List<ItemSlot> items = monster.inventory.slots.Where(item => item.amount > 0).ToList();
            for (int i = 0; i < monster.inventory.slots.Count; ++i)
            {
                int icopy = monster.inventory.slots.FindIndex(
                                // note: .Equals because name AND dynamic variables matter (petLevel etc.)
                                itemSlot => itemSlot.amount > 0 && itemSlot.item.Equals(items[i].item));
                CmdTakeItem(icopy);
            }
        }
    }
}
