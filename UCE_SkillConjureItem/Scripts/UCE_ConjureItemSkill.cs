// =======================================================================================
// Created and maintained by Fhiz
// Usable for both personal and commercial projects, but no sharing or re-sale
// * Discord Support Server.............: https://discord.gg/YkMbDHs
// * Public downloads website...........: https://www.indie-mmo.net
// * Pledge on Patreon for VIP AddOns...: https://www.patreon.com/IndieMMO
// =======================================================================================
using UnityEngine;

// =======================================================================================
// SKILL CONJURE ITEM
// =======================================================================================
[CreateAssetMenu(menuName = "uMMORPG Skill/UCE Skill Conjure Item", order = 999)]
public class UCE_ConjureItemSkill : ScriptableSkill
{
    [Header("-=-=-=- Conjured Items -=-=-=-")]
    [Tooltip("The conjured item, one per level of skill")]
    public UCE_ConjureableItem[] conjuredItems;

    [Header("-=-=-=- Feedback Message -=-=-=-")]
    public string conjuredMessage;
    public string failedMessage;
    [Range(0, 255)] public byte iconId;
    [Range(0, 255)] public byte soundId;

    // -----------------------------------------------------------------------------------
    // CheckTarget
    // -----------------------------------------------------------------------------------
    public override bool CheckTarget(Entity caster)
    {
        return true;
    }

    // -----------------------------------------------------------------------------------
    // CheckDistance
    // -----------------------------------------------------------------------------------
    public override bool CheckDistance(Entity caster, int skillLevel, out Vector2 destination)
    {
        destination = caster.transform.position;
        return true;
    }

    // -----------------------------------------------------------------------------------
    // Apply
    // -----------------------------------------------------------------------------------
    public override void Apply(Entity caster, int skillLevel, Vector2 destination)
    {
        Player player = (Player)caster;

        skillLevel--;

        if (conjuredItems.Length >= skillLevel)
        {
            if (conjuredItems[skillLevel] != null && player.inventory.CanAdd(new Item(conjuredItems[skillLevel].item), conjuredItems[skillLevel].amount))
            {
                if (UnityEngine.Random.value <= conjuredItems[skillLevel].baseSuccessChance + conjuredItems[skillLevel].bonusChancePerLevel * skillLevel)
                {
                    player.inventory.Add(new Item(conjuredItems[skillLevel].item), conjuredItems[skillLevel].amount);
                    player.UCE_TargetAddMessage(conjuredMessage + conjuredItems[skillLevel].item.name);
                    player.UCE_ShowPopup(conjuredMessage + conjuredItems[skillLevel].item.name, iconId, soundId);
                }
                else
                {
                    player.UCE_TargetAddMessage(failedMessage + conjuredItems[skillLevel].item.name);
                }
            }
            else
            {
                player.UCE_TargetAddMessage(failedMessage + conjuredItems[skillLevel].item.name);
            }
        }
    }

    // -----------------------------------------------------------------------------------
    // UCE_ConjureableItem
    // -----------------------------------------------------------------------------------
    [System.Serializable]
    public class UCE_ConjureableItem
    {
        public ScriptableItem item;
        public int amount;
        [Range(0, 1)] public float baseSuccessChance;
        [Range(0, 1)] public float bonusChancePerLevel;
    }

    // -----------------------------------------------------------------------------------
}

// =======================================================================================