// =======================================================================================
// Created and maintained by iMMO
// Usable for both personal and commercial projects, but no sharing or re-sale
// * Discord Support Server.............: https://discord.gg/YkMbDHs
// * Public downloads website...........: https://www.indie-mmo.net
// * Pledge on Patreon for VIP AddOns...: https://www.patreon.com/iMMOban
// =======================================================================================
using Eflatun.SceneReference;
using UnityEngine;

// =======================================================================================
// UCE TELEPORTATION TARGET
// =======================================================================================
[System.Serializable]
public partial class UCE_TeleportationTarget
{
    public enum TeleportationType { onScene, offScene }

#if _iMMOZONES
    public TeleportationType teleportationType = TeleportationType.onScene;
    public Transform targetPosition;
    public SceneReference offSceneTarget;
#else
	public Transform targetPosition;
#endif

    // -----------------------------------------------------------------------------------
    // name
    // -----------------------------------------------------------------------------------
    public string name
    {
        get
        {
#if _iMMOZONES
            if (teleportationType == TeleportationType.onScene)
                return targetPosition.name;
            return offSceneTarget.Name;
#else
			if (targetPosition != null)
				return targetPosition.name;
			else
				return "";
#endif
        }
    }

    // -----------------------------------------------------------------------------------
    // getDistance
    // Returns the distance of the stated transform to the target
    // -----------------------------------------------------------------------------------
    public float getDistance(Transform transform)
    {
#if _iMMOZONES
        if (teleportationType == TeleportationType.onScene)
            return Vector2.Distance(targetPosition.position, transform.position);
        return 1;
#else
			return Vector2.Distance(targetPosition.position, transform.position);
#endif
    }

    // -----------------------------------------------------------------------------------
    // Valid
    // -----------------------------------------------------------------------------------
    public bool Valid
    {
        get
        {
#if _iMMOZONES
            if (teleportationType == TeleportationType.onScene)
                return targetPosition != null;
            return offSceneTarget != null;
#else
			return targetPosition != null;
#endif
        }
    }

    // -----------------------------------------------------------------------------------
    // OnTeleport
    // @Server
    // -----------------------------------------------------------------------------------
    public void OnTeleport(Player player)
    {
        if (!player || !Valid) return;

#if _iMMOZONES
        if (teleportationType == TeleportationType.onScene)
            player.UCE_Warp(targetPosition.position);
        else
            player.UCE_OnPortal(offSceneTarget, targetPosition.position, player);
#else
		player.UCE_Warp(targetPosition.position);
#endif
    }

    // -----------------------------------------------------------------------------------
}

// =======================================================================================