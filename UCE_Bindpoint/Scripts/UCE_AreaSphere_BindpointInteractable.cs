// =======================================================================================
// Created and maintained by Fhiz
// Usable for both personal and commercial projects, but no sharing or re-sale
// * Discord Support Server.............: https://discord.gg/YkMbDHs
// * Public downloads website...........: https://www.indie-mmo.net
// * Pledge on Patreon for VIP AddOns...: https://www.patreon.com/IndieMMO
// =======================================================================================
using Mirror;
using UnityEngine;

// =======================================================================================
// BINDPOINT AREA
// =======================================================================================
[RequireComponent(typeof(Collider2D))]
public class UCE_AreaSphere_BindpointInteractable : UCE_InteractableAreaSphere
{
    [Header("-=-=-=- Bindpoint -=-=-=-")]
    public Transform bindpoint;

    [Header("-=-=-=- Popups -=-=-=-")]
    public UCE_PopupClass enterPopup;

    // -----------------------------------------------------------------------------------
    // OnInteractClient
    // @Client
    // -----------------------------------------------------------------------------------
    [ClientCallback]
    public override void OnInteractClient(Player player)
    {
        if (bindpoint != null && player.UCE_myBindpoint.name != bindpoint.gameObject.name)
            player.UCE_ClientShowPopup(enterPopup.message, enterPopup.iconId, enterPopup.soundId);
    }

    // -----------------------------------------------------------------------------------
    // OnInteractServer
    // @Server
    // -----------------------------------------------------------------------------------
    [ServerCallback]
    public override void OnInteractServer(Player player)
    {
        if (bindpoint != null && player.UCE_myBindpoint.name != bindpoint.gameObject.name)
            player.UCE_SetBindpointFromArea(bindpoint.gameObject.name, bindpoint.position.x, bindpoint.position.y, bindpoint.position.z);
    }

    // -------------------------------------------------------------------------------
}

// =======================================================================================