// =======================================================================================
// Created and maintained by Fhiz
// Usable for both personal and commercial projects, but no sharing or re-sale
// * Discord Support Server.............: https://discord.gg/YkMbDHs
// * Public downloads website...........: https://www.indie-mmo.net
// * Pledge on Patreon for VIP AddOns...: https://www.patreon.com/IndieMMO
// =======================================================================================

using Eflatun.SceneReference;
using UnityEngine;

// =======================================================================================
// UCE_BindPoint
// =======================================================================================
[System.Serializable]
public struct UCE_BindPoint
{
    public string name;
    public string SceneName;
    public Vector3 position;

    public SceneReference mapScene
    {
        set { SceneName = value.Name; }
    }

    public bool Valid
    {
        get
        {
            return !string.IsNullOrEmpty(SceneName);
        }
    }
}