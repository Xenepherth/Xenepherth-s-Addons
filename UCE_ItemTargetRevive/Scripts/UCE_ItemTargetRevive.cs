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
// TARGET REVIVE ITEM
// =======================================================================================
[CreateAssetMenu(menuName = "uMMORPG Item/UCE Item Target Revive", order = 999)]
public class UCE_ItemTargetRevive : UsableItem
{
    [Header("-=-=-=- UCE Target Revive Item -=-=-=-")]
    [Range(0, 1)] public float successChance;
    public float range;
    public int healsHealth;
    public int healsMana;

    [Header("-=-=-=- Buff on Target -=-=-=-")]
    public BuffSkill applyBuff;
    public int buffLevel;
    [Range(0, 1)] public float buffChance;

    public string successMessage = "You revived: ";
    public string failedMessage = "You failed to revive: ";

    [Tooltip("Decrease amount by how many each use (can be 0)?")]
    public int decreaseAmount = 1;

    // -----------------------------------------------------------------------------------
    // CheckTarget
    // -----------------------------------------------------------------------------------
    public bool CheckTarget(Entity caster)
    {
        return caster.target != null && caster.CanAttack(caster.target);
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
            if (player.target != null && player.target is Player && !player.target.isAlive)
            {
                // always call base function too
                base.Use(player, inventoryIndex);

                if (UnityEngine.Random.value <= successChance)
                {
                    player.target.health.current += healsHealth;
                    player.target.mana.current += healsMana;
                    player.target.UCE_ApplyBuff(applyBuff, buffLevel, buffChance);
                    player.target.UCE_OverrideState("IDLE");
                    player.UCE_TargetAddMessage(successMessage + player.target.name);
                }
                else
                {
                    player.UCE_TargetAddMessage(failedMessage + player.target.name);
                }

                // decrease amount
                slot.DecreaseAmount(decreaseAmount);
                player.inventory.slots[inventoryIndex] = slot;
            }
        }
    }

    // -----------------------------------------------------------------------------------
}

// =======================================================================================