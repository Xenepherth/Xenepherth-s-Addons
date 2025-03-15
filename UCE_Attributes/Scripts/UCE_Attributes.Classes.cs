﻿// =======================================================================================
// Created and maintained by iMMO
// Usable for both personal and commercial projects, but no sharing or re-sale
// * Discord Support Server.............: https://discord.gg/YkMbDHs
// * Public downloads website...........: https://www.indie-mmo.net
// * Pledge on Patreon for VIP AddOns...: https://www.patreon.com/IndieMMO
// =======================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Mirror;

// =======================================================================================
// PLAYER ATTRIBUTES
// =======================================================================================
[System.Serializable]
public partial class UCE_playerAttributes
{
    public UCE_AttributeTemplate[] UCE_AttributeTypes = { };

    [Tooltip("[Optional] Number of attribute points a new character starts with.")]
    public int startingAttributePoints = 0;

    [Tooltip("[Optional] Number of attribute points rewarded on each reward level.")]
    public int rewardPoints = 1;

    [Tooltip("[Optional] Number of levels a player must achieve between rewards.")]
    public int everyXLevels = 1;

    [Tooltip("[Optional] First level when the rewards start (not counting the initial level).")]
    public int startingRewardLevel = 1;
}

// =======================================================================================
// UCE ATTRIBUTE CACHE
// =======================================================================================
[System.Serializable]
public class UCE_AttributeCache
{
    public float timer = 0f;
    public int value = 0;
}

// =======================================================================================/*