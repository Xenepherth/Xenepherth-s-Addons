﻿using System.Text;
using UnityEngine;
using Mirror;

// =======================================================================================
// SKILL EXPERIENCE - ITEM
// =======================================================================================
[CreateAssetMenu(menuName = "uMMORPG Item/UCE Item Skill Experience", order = 998)]
public class UCE_Tmpl_ItemSkillExperience : UsableItem
{
    [Header("-=-=-=- Skill Experience Item -=-=-=-")]
    [Tooltip("The amount of Skill Experience gained when used")]
    public int skillExperienceBonus;

    [Tooltip("Decrease amount by how many each use (can be 0)?")]
    public int decreaseAmount = 1;

    // -----------------------------------------------------------------------------------
    // Use
    // @Server
    // -----------------------------------------------------------------------------------
    public override void Use(Player player, int inventoryIndex)
    {
        ItemSlot slot = player.inventory.slots[inventoryIndex];

        // -- Only activate if enough charges left
        if (decreaseAmount == 0 || slot.amount >= decreaseAmount)
        {
            // always call base function too
            base.Use(player, inventoryIndex);

            // -- Decrease Amount
            if (decreaseAmount != 0)
            {
                slot.DecreaseAmount(skillExperienceBonus);
                player.inventory.slots[inventoryIndex] = slot;
            }

            // -- Activate Teleport
            ((PlayerSkills)player.skills).skillExperience += skillExperienceBonus;
        }
    }

    // -----------------------------------------------------------------------------------
    // Tooltip
    // @Client
    // -----------------------------------------------------------------------------------
    public override string ToolTip()
    {
        StringBuilder tip = new StringBuilder(base.ToolTip());
        tip.Replace("{SKILLEXPERIENCE}", skillExperienceBonus.ToString());
        return tip.ToString();
    }

    // -----------------------------------------------------------------------------------
}