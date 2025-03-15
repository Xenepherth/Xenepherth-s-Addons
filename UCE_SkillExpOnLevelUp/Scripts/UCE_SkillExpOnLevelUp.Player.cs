﻿using Mirror;
using UnityEngine;

// =======================================================================================
// PLAYER
// =======================================================================================
public partial class Player
{
    [Header("-=-=-=- UCE SKILL EXP ON LEVEL UP -=-=-=-")]
    public LinearInt skillExpOnLevelUp;

    // -----------------------------------------------------------------------------------
    // OnLevelUp_UCE_LevelUpNotice
    // -----------------------------------------------------------------------------------
    [Server]
    //[DevExtMethods("OnLevelUp")]
    public void OnLevelUp_UCE_SkillExpOnLevelUp()
    {
        ((PlayerSkills)skills).skillExperience += skillExpOnLevelUp.Get(level.current);
    }
}

// =======================================================================================