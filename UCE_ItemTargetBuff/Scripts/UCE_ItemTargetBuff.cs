﻿// =======================================================================================
// Created and maintained by Fhiz
// Usable for both personal and commercial projects, but no sharing or re-sale
// * Discord Support Server.............: https://discord.gg/YkMbDHs
// * Public downloads website...........: https://www.indie-mmo.net
// * Pledge on Patreon for VIP AddOns...: https://www.patreon.com/IndieMMO
// =======================================================================================
using System;
using System.Text;
using UnityEngine;
using Mirror;
using System.Collections.Generic;

// =======================================================================================
// TARGET CLEANSE ITEM
// =======================================================================================
[CreateAssetMenu(menuName = "UCE Item/UCE Item Target Buff", order = 999)]
public class UCE_ItemTargetBuff : UsableItem
{
    [Header("-=-=-=- UCE Target Buff Item -=-=-=-")]
    public BuffSkill applyBuff;
    public int buffLevel;
    [Range(0, 1)] public float buffChance;
    public float range;
    public string successMessage = "You buffed {0} with {1}";

    [Tooltip("Decrease amount by how many each use (can be 0)?")]
    public int decreaseAmount = 1;

    // -----------------------------------------------------------------------------------
    // CheckTarget
    // -----------------------------------------------------------------------------------
    public bool CheckTarget(Entity caster)
    {
        return caster.target != null && (caster.CanAttack(caster.target) || caster.target == caster);
    }

    // -----------------------------------------------------------------------------------
    // CheckDistance
    // -----------------------------------------------------------------------------------
    public bool CheckDistance(Entity caster, int skillLevel, out Vector2 destination)
    {
        if (caster.target != null)
        {
            destination = caster.target.collider.ClosestPointOnBounds(caster.transform.position);
            return Utils.ClosestDistance(caster.collider, caster.target.collider) <= range;
        }
        caster.target = caster;
        destination = caster.transform.position;
        return false;
    }

    // -----------------------------------------------------------------------------------
    // Use
    // -----------------------------------------------------------------------------------
    public override void Use(Player player, int inventoryIndex)
    {
        ItemSlot slot = player.inventory.slots[inventoryIndex];

        // -- Only activate if enough charges left
        if (decreaseAmount == 0 || slot.amount >= decreaseAmount)
        {
            if (player.target != null && player.target.isAlive)
            {
                // always call base function too
                base.Use(player, inventoryIndex);

                player.target.UCE_ApplyBuff(applyBuff, buffLevel, buffChance);

                player.UCE_TargetAddMessage(string.Format(successMessage, player.target.name, applyBuff.name));

                // decrease amount
                slot.DecreaseAmount(decreaseAmount);
                player.inventory.slots[inventoryIndex] = slot;
            }
        }
    }

    // -----------------------------------------------------------------------------------
}

// =======================================================================================