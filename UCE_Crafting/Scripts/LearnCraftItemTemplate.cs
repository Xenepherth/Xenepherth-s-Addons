﻿// =======================================================================================
// Created and maintained by Fhiz
// Usable for both personal and commercial projects, but no sharing or re-sale
// * Discord Support Server.............: https://discord.gg/YkMbDHs
// * Public downloads website...........: https://www.indie-mmo.net
// * Pledge on Patreon for VIP AddOns...: https://www.patreon.com/IndieMMO
// =======================================================================================
using System.Text;
using UnityEngine;

#if _iMMOCRAFTING

// =======================================================================================
// LEARN CRAFT ITEM
// =======================================================================================
[CreateAssetMenu(menuName = "uMMORPG Item/UCE Learn Craft Item", order = 999)]
public class LearnCraftItemTemplate : UsableItem
{
    [Header("Usage")]
    public UCE_CraftingProfessionTemplate learnProfession;

    [Tooltip("[Optional] Amount of profession experience gained when used (should never be less than 1 - otherwise the profession wont be learned).")]
    public int gainProfessionExp = 1;

    [Tooltip("[Optional] The item can only be used when the profession has not been learned yet.")]
    public bool onlyWhenLearnable;

    public string expProfessionTxt = " Profession experience gained!";
    public string learnProfessionText = "You learned a new craft: ";

    [Tooltip("Decrease amount by how many each use (can be 0)?")]
    public int decreaseAmount = 1;

    // -----------------------------------------------------------------------------------
    // CanUse
    // -----------------------------------------------------------------------------------
    public override bool CanUse(Player player, int inventoryIndex)
    {
        return (onlyWhenLearnable && !player.UCE_HasCraftingProfession(learnProfession) || !onlyWhenLearnable) && minLevel < player.level.current;
    }

    // -----------------------------------------------------------------------------------
    //  Use
    // -----------------------------------------------------------------------------------
    public override void Use(Player player, int inventoryIndex)
    {
        ItemSlot slot = player.inventory.slots[inventoryIndex];

        // -- Only activate if enough charges left
        if (decreaseAmount == 0 || slot.amount >= decreaseAmount)
        {
            // always call base function too
            base.Use(player, inventoryIndex);

            if (!player.UCE_HasCraftingProfession(learnProfession))
            {
                UCE_CraftingProfession tmpProf = new UCE_CraftingProfession(learnProfession.name);

                tmpProf.experience = gainProfessionExp;
                player.UCE_Crafts.Add(tmpProf);

                player.UCE_ShowPopup(learnProfessionText + learnProfession.name);
            }
            else
            {
                UCE_CraftingProfession tmpProf = player.UCE_getCraftingProfession(learnProfession);

                tmpProf.experience += gainProfessionExp;

                player.UCE_setCraftingProfession(tmpProf);
                player.UCE_TargetAddMessage(gainProfessionExp.ToString() + expProfessionTxt);
            }

            slot.DecreaseAmount(decreaseAmount);
            player.inventory.slots[inventoryIndex] = slot;
        }
    }

    // -----------------------------------------------------------------------------------
    // ToolTip
    // -----------------------------------------------------------------------------------
    public override string ToolTip()
    {
        StringBuilder tip = new StringBuilder(base.ToolTip());
        tip.Replace("{MINLEVEL}", minLevel.ToString());
        return tip.ToString();
    }

    // -----------------------------------------------------------------------------------
}

#endif

// =======================================================================================