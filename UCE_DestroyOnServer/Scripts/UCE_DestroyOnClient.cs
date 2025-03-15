﻿// =======================================================================================
// Created and maintained by iMMO
// Usable for both personal and commercial projects, but no sharing or re-sale
// * Discord Support Server.............: https://discord.gg/YkMbDHs
// * Public downloads website...........: https://www.indie-mmo.net
// * Pledge on Patreon for VIP AddOns...: https://www.patreon.com/IndieMMO
// =======================================================================================

using UnityEngine;
using Mirror;

// =======================================================================================
// DESTROY ON CLIENT
// =======================================================================================
public class UCE_DestroyOnClient : MonoBehaviour
{
    // -------------------------------------------------------------------------------
    // Start
    // -------------------------------------------------------------------------------
    private void Start()
    {
        NetworkBehaviour source = GetComponentInParent<NetworkBehaviour>();

        if (source && !NetworkServer.active && source.isClient)
            Destroy(this.gameObject);
    }

    // -------------------------------------------------------------------------------
}

// =======================================================================================