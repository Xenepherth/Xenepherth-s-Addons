// =======================================================================================
// Maintained by bobatea#9400 on Discord
// Usable for both personal and commercial projects, but no sharing or re-sale
// * Discord Support Server.............: 
 
// * Leave a star on my Github Repo.....: https://github.com/breehuynh/Bree-mmorpg-tools
// * Instructions.......................: https://indie-mmo.net/knowledge-base/
// =======================================================================================
using UnityEngine;
using Mirror;
using System;
using System.Linq;
using System.Collections;

// PLAYER

public partial class Player
{
    // -----------------------------------------------------------------------------------
    // OnSelect_UCE_IndicatorProjector
    // @Client
    // -----------------------------------------------------------------------------------
    [Client]
    //[DevExtMethods("LateUpdate")]
    private void LateUpdate_UCE_IndicatorProjector()
    {
        if(target is Monster monster && monster.GetComponent<UCE_IndicatorProjector>() != null)
        {
            UCE_IndicatorProjector ip = monster.GetComponent<UCE_IndicatorProjector>();
            ip.Show();
        }
        else if (target is Pet pet && pet.GetComponent<UCE_IndicatorProjector>() != null)
        {
            UCE_IndicatorProjector ip = pet.GetComponent<UCE_IndicatorProjector>();
            ip.Show();
        }
        else if (target is Npc npc && npc.GetComponent<UCE_IndicatorProjector>() != null)
        {
            UCE_IndicatorProjector ip = npc.GetComponent<UCE_IndicatorProjector>();
            ip.Show();
        }
        else if (target is Mount mount && mount.GetComponent<UCE_IndicatorProjector>() != null)
        {
            UCE_IndicatorProjector ip = mount.GetComponent<UCE_IndicatorProjector>();
            ip.Show();
        }
        else if (target is Player player && player.GetComponent<UCE_IndicatorProjector>() != null)
        {
            UCE_IndicatorProjector ip = player.GetComponent<UCE_IndicatorProjector>();
            ip.Show();
        }
    }

    // -----------------------------------------------------------------------------------
}
