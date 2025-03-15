using UnityEngine;
using Mirror;
using System.Collections;
using System;

// =======================================================================================
// PLAYER
// =======================================================================================
public partial class Player
{
    [Header("-=-=-=- UCE LEVEL UP NOTICE -=-=-=-")]
    public UCE_PopupClass levelUpNotice;

    // -----------------------------------------------------------------------------------
    // OnLevelUp_UCE_LevelUpNotice
    // -----------------------------------------------------------------------------------
    [Server]
    //[DevExtMethods("OnLevelUp")]
    public void OnLevelUp_UCE_LevelUpNotice()
    {
        Target_UCE_ShowPopup(connectionToClient, levelUpNotice.message + this.level.current.ToString(), levelUpNotice.iconId, levelUpNotice.soundId);
    }
}

// =======================================================================================