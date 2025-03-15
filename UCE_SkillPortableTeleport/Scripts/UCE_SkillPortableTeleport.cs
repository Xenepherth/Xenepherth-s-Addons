// =======================================================================================
// Created and maintained by Fhiz
// Usable for both personal and commercial projects, but no sharing or re-sale
// * Discord Support Server.............: https://discord.gg/YkMbDHs
// * Public downloads website...........: https://www.indie-mmo.net
// * Pledge on Patreon for VIP AddOns...: https://www.patreon.com/IndieMMO
// =======================================================================================
using UnityEngine;

// =======================================================================================
// SKILL DEBUFF
// =======================================================================================
[CreateAssetMenu(menuName = "uMMORPG Skill/UCE Skill Portable Teleport", order = 999)]
public class UCE_SkillPortableTeleport : ScriptableSkill
{
    [Header("-=-=-=- UCE Skill Portable Teleport -=-=-=-")]
    [Tooltip("[Required] GameObject prefab with coordinates OR off scene coordinates (requires UCE Network Zones AddOn)")]
    public UCE_TeleportationTarget teleportationTarget;

    [Tooltip("This will ignore the teleport Location and choose the nearest spawn point instead")]
    public bool teleportToClosestSpawnpoint;

    // -----------------------------------------------------------------------------------
    //
    // -----------------------------------------------------------------------------------
    public override bool CheckTarget(Entity caster)
    {
        return true;
    }

    // -----------------------------------------------------------------------------------
    //
    // -----------------------------------------------------------------------------------
    public override bool CheckDistance(Entity caster, int skillLevel, out Vector2 destination)
    {
        destination = caster.transform.position;
        return true;
    }

    // -----------------------------------------------------------------------------------
    //
    // -----------------------------------------------------------------------------------
    public override void Apply(Entity caster, int skillLevel, Vector2 destination)
    {
        // apply only to alive people
        if (caster.isAlive)
        {
            // -- Determine Teleportation Target
            if (teleportToClosestSpawnpoint)
            {
                Transform target = NetworkManagerMMO.GetNearestStartPosition(caster.transform.position);
                ((Player)caster).UCE_Warp(target.position);
            }
            else
            {
                teleportationTarget.OnTeleport((Player)caster);
            }
        }
    }

    // -----------------------------------------------------------------------------------
}

// =======================================================================================