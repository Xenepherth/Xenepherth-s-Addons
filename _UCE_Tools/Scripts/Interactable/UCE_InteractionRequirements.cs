// =======================================================================================
// Created and maintained by iMMO
// Usable for both personal and commercial projects, but no sharing or re-sale
// * Discord Support Server.............: https://discord.gg/YkMbDHs
// * Public downloads website...........: https://www.indie-mmo.net
// * Pledge on Patreon for VIP AddOns...: https://www.patreon.com/iMMOban
// =======================================================================================

using System.Linq;
using UnityEngine;

// =======================================================================================
// INTERACTION REQUIREMENTS CLASS
// THIS CLASS IS PRIMARILY FOR OBJECTS THE PLAYER CAN CHOOSE TO INTERACT WITH
// =======================================================================================
[System.Serializable]
public partial class UCE_InteractionRequirements : UCE_Requirements
{
    [Header("[-=-=-=- UCE COSTS [Removed after checking Requirements] -=-=-=-]")]
    [Tooltip("[Optional] These items will be removed from players inventory")]
    public UCE_ItemRequirement[] removeItems;

    [Tooltip("[Optional] Gold cost to interact")]
    public long goldCost = 0;

    [Tooltip("[Optional] Coins cost to interact")]
    public long coinCost = 0;

    [Tooltip("[Optional] Health cost to interact")]
    public int healthCost = 0;

    [Tooltip("[Optional] Mana cost to interact")]
    public int manaCost = 0;

#if _iMMOHONORSHOP

    [Tooltip("[Optional] Honor Currency costs to interact")]
    public UCE_HonorShopCurrencyDrop[] honorCurrencyCost;
#endif

    [Header("[-=-=-=- UCE REWARDS [awarded after checks & costs (repetitive)] -=-=-=-]")]
    public int expRewardMin = 0;
    public int expRewardMax = 0;
    public int skillExpRewardMin = 0;
    public int skillExpRewardMax = 0;

    // -----------------------------------------------------------------------------------
    // checkRequirements
    // -----------------------------------------------------------------------------------
    public override bool checkRequirements(Player player)
    {
        bool valid = true;

        valid = base.checkRequirements(player);

        valid = checkCosts(player, valid);

        return valid;
    }

    // -----------------------------------------------------------------------------------
    // checkCosts
    // -----------------------------------------------------------------------------------
    public bool checkCosts(Player player, bool valid)
    {
        valid = (removeItems.Length == 0 || player.UCE_checkHasItems(removeItems)) ? valid : false;
        valid = (goldCost == 0 || player.gold >= goldCost) ? valid : false;
        valid = (coinCost == 0 || player.itemMall.coins >= coinCost) ? valid : false;
        valid = (healthCost == 0 || player.health.current >= healthCost) ? valid : false;
        valid = (manaCost == 0 || player.mana.current >= manaCost) ? valid : false;
#if _iMMOHONORSHOP
        valid = (player.UCE_CheckHonorCurrencyCost(honorCurrencyCost)) ? valid : false;
#endif

        return valid;
    }

    // -----------------------------------------------------------------------------------
    // hasCosts
    // -----------------------------------------------------------------------------------
    public bool hasCosts()
    {
        return removeItems.Length > 0 ||
                goldCost > 0 ||
                coinCost > 0 ||
                healthCost > 0 ||
                manaCost > 0
#if _iMMOHONORSHOP
                || honorCurrencyCost.Any(x => x.amount > 0)
#endif
                ;
    }

    // -----------------------------------------------------------------------------------
    // payCosts
    // -----------------------------------------------------------------------------------
    public void payCosts(Player player)
    {
        if (checkRequirements(player))
        {
            player.UCE_removeItems(removeItems, true);

            player.gold -= goldCost;
            player.itemMall.coins -= coinCost;

            player.mana.current -= manaCost;

            if (player.health.current > healthCost)
                player.health.current -= healthCost;
            else
                player.health.current = 1;

#if _iMMOHONORSHOP
            player.UCE_PayHonorCurrencyCost(honorCurrencyCost);
#endif
        }
    }

    // -----------------------------------------------------------------------------------
    // grantRewards
    // -----------------------------------------------------------------------------------
    public void grantRewards(Player player)
    {
        if (checkRequirements(player))
        {
            player.experience.current += Random.Range(expRewardMin, expRewardMax);
            ((PlayerSkills)player.skills).skillExperience += Random.Range(skillExpRewardMin, skillExpRewardMax);
        }
    }

    // -----------------------------------------------------------------------------------
}