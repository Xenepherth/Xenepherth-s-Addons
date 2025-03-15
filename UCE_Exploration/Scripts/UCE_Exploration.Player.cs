﻿// =======================================================================================
// Created and maintained by iMMO
// Usable for both personal and commercial projects, but no sharing or re-sale
// * Discord Support Server.............: https://discord.gg/YkMbDHs
// * Public downloads website...........: https://www.indie-mmo.net
// * Pledge on Patreon for VIP AddOns...: https://www.patreon.com/IndieMMO
// =======================================================================================

using UnityEngine;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

// =======================================================================================
// PLAYER
// =======================================================================================
public partial class Player
{
    [HideInInspector] public UCE_Area_Exploration UCE_myExploration;

    public readonly SyncList<string> UCE_exploredAreas = new SyncList<string>();

    protected double fTimerCache;
    protected double fTimerInterval = 10f;

    // -----------------------------------------------------------------------------------
    // UCE_ExploreArea
    // -----------------------------------------------------------------------------------
    [ServerCallback]
    public void UCE_ExploreArea()
    {
        if (UCE_myExploration && UCE_myExploration.explorationRequirements.isActive && UCE_myExploration.explorationRequirements.checkRequirements(this))
        {
            // -- explore the area

            if (!UCE_exploredAreas.Contains(UCE_myExploration.name))
            {
                UCE_exploredAreas.Add(UCE_myExploration.name);

                UCE_myExploration.explorationRewards.gainRewards(this);

                var msg = UCE_myExploration.explorePopup.message + UCE_myExploration.name;
                UCE_ShowPopup(msg, UCE_myExploration.explorePopup.iconId, UCE_myExploration.explorePopup.soundId);
                UCE_MinimapSceneText(UCE_myExploration.name);
                fTimerCache = NetworkTime.time + fTimerInterval;

                // -- show notice if already explored
            }
            else if (UCE_myExploration.noticeOnEnter)
            {
                if (NetworkTime.time <= fTimerCache) return;
                var msg = UCE_myExploration.enterPopup.message + UCE_myExploration.name;
                UCE_ShowPopup(msg, UCE_myExploration.enterPopup.iconId, UCE_myExploration.enterPopup.soundId);
                UCE_MinimapSceneText(UCE_myExploration.name);
                fTimerCache = NetworkTime.time + fTimerInterval;
            }
        }
    }

    // -----------------------------------------------------------------------------------
    // UCE_HasExploredArea
    // -----------------------------------------------------------------------------------
    public bool UCE_HasExploredArea(UCE_Area_Exploration simpleExplorationArea)
    {
        return UCE_exploredAreas.Contains(simpleExplorationArea.name);
    }
}

// =======================================================================================