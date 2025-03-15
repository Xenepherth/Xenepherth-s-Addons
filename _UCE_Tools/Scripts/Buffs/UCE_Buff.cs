﻿// =======================================================================================
// Created and maintained by iMMO
// Usable for both personal and commercial projects, but no sharing or re-sale
// * Discord Support Server.............: https://discord.gg/YkMbDHs
// * Public downloads website...........: https://www.indie-mmo.net
// * Pledge on Patreon for VIP AddOns...: https://www.patreon.com/IndieMMO
// =======================================================================================

// =======================================================================================
// BUFF
// =======================================================================================
public partial struct Buff
{
    public bool cannotRemove { get { return data.cannotRemove; } }
    public bool blockNerfs { get { return data.blockNerfs; } }
    public bool blockBuffs { get { return data.blockBuffs; } }

#if _iMMOBUFFBLOCKHEALTHRECOVERY
    public bool blockHealthRecovery { get { return data.blockHealthRecovery; } }
#endif
#if _iMMOBUFFBLOCKMANARECOVERY
    public bool blockManaRecovery { get { return data.blockManaRecovery; } }
#endif
#if _iMMOBUFFENDURE
    public bool endure { get { return data.endure; } }
#endif
#if _iMMOBUFFEXPERIENCE
    public float boostExperience { get { return data.boostExperience; } }
#endif
#if _iMMOBUFFGOLD
    public float boostGold { get { return data.boostGold; } }
#endif
#if _iMMOBUFFINVINCIBILITY
    public bool invincibility { get { return data.invincibility; } }
#endif

    // -----------------------------------------------------------------------------------
    // CheckBuffType
    // -----------------------------------------------------------------------------------
    public bool CheckBuffType(BuffType buffType)
    {
        if (buffType == BuffType.Both) return true;

        return (
                (buffType == BuffType.Buff && !data.disadvantageous) ||
                (buffType == BuffType.Nerf && data.disadvantageous));
    }

    // -----------------------------------------------------------------------------------
}