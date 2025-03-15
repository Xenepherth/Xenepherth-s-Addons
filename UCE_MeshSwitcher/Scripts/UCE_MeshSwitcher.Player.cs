// =======================================================================================
// Created and maintained by Fhiz
// Usable for both personal and commercial projects, but no sharing or re-sale
// * Discord Support Server.............: https://discord.gg/YkMbDHs
// * Public downloads website...........: https://www.indie-mmo.net
// * Pledge on Patreon for VIP AddOns...: https://www.patreon.com/IndieMMO
// =======================================================================================

using System;
using UnityEngine;

// =======================================================================================
// PLAYER
// =======================================================================================
partial class Player
{
    protected SpriteRenderer[] cachedRenderers;

    // -----------------------------------------------------------------------------------
    // OnStartClient_UCE_MeshSwitcher
    // -----------------------------------------------------------------------------------
    //[DevExtMethods("OnStartClient")]
    public void OnStartClient_UCE_MeshSwitcher()
    {
        // -- Cache the default Material

        for (int index = 0; index < ((PlayerEquipment)equipment).slotInfo.Length; ++index)
        {
            EquipmentInfo info = ((PlayerEquipment)equipment).slotInfo[index];
            for (int i = 0; i < info.mesh.Length; ++i)
            {
                if (info.mesh[i].mesh != null)
                {
                    info.mesh[i].defaultMaterial = info.mesh[i].mesh.GetComponent<SpriteRenderer>().material;
                }
            }
            UCE_RefreshMesh(index);
        }
    }

    // -----------------------------------------------------------------------------------
    // OnRefreshLocation_UCE_MeshSwitcher
    // -----------------------------------------------------------------------------------
    //[DevExtMethods("OnRefreshLocation")]
    public void OnRefreshLocation_UCE_MeshSwitcher(int index)
    {
        UCE_RefreshMesh(index);
    }

    // -----------------------------------------------------------------------------------
    // UCE_RefreshMesh
    // -----------------------------------------------------------------------------------
    private void UCE_RefreshMesh(int index)
    {
        ItemSlot slot = equipment.slots[index];
        EquipmentInfo info = ((PlayerEquipment)equipment).slotInfo[index];
        cachedRenderers = new SpriteRenderer[info.mesh.Length];

        for (int i = 0; i < info.mesh.Length; ++i)
        {
            cachedRenderers[i] = info.mesh[i].mesh.GetComponent<SpriteRenderer>();
        }

        if (info.requiredCategory != "" && info.mesh.Length > 0)
        {
            if (slot.amount > 0)
            {
                EquipmentItem itemData = (EquipmentItem)slot.item.data;
                UCE_SwitchToMesh(info, itemData.meshIndex, itemData.meshMaterial, itemData.switchableColors);
            }
            else
            {
                UCE_SwitchToMesh(info, 0, null, null);
            }
        }
    }

    // -----------------------------------------------------------------------------------
    // UCE_SwitchToMesh
    // -----------------------------------------------------------------------------------
    private void UCE_SwitchToMesh(EquipmentInfo info, int[] meshIndex, Material mat, SwitchableColor[] colors)
    {
        for (int i = 0; i < info.mesh.Length; ++i)
        {
            if (info.mesh[i].mesh != null)
            {
                if (Array.Exists(meshIndex, e => e == i))
                {
                    info.mesh[i].mesh.SetActive(true);

                    if (mat != null)
                    {
                        cachedRenderers[i].material = mat;
                    }
                    else if (info.mesh[i].defaultMaterial != null)
                    {
                        cachedRenderers[i].material = info.mesh[i].defaultMaterial;
                    }

                    UCE_SwitchToColor(info, colors);
                }
                else
                {
                    info.mesh[i].mesh.SetActive(false);
                }
            }
        }
    }

    // -----------------------------------------------------------------------------------
    // UCE_SwitchToMesh
    // -----------------------------------------------------------------------------------
    private void UCE_SwitchToMesh(EquipmentInfo info, int meshIndex, Material mat, SwitchableColor[] colors)
    {
        for (int i = 0; i < info.mesh.Length; ++i)
        {
            if (info.mesh[i].mesh != null)
            {
                if (meshIndex == i)
                {
                    info.mesh[i].mesh.SetActive(true);

                    if (mat != null)
                    {
                        info.mesh[i].mesh.GetComponent<SpriteRenderer>().material = mat;
                    }
                    else if (info.mesh[i].defaultMaterial != null)
                    {
                        info.mesh[i].mesh.GetComponent<SpriteRenderer>().material = info.mesh[i].defaultMaterial;
                    }

                    UCE_SwitchToColor(info, colors);
                }
                else
                {
                    info.mesh[i].mesh.SetActive(false);
                }
            }
        }
    }

    // -----------------------------------------------------------------------------------
    // UCE_SwitchToColor
    // -----------------------------------------------------------------------------------
    private void UCE_SwitchToColor(EquipmentInfo info, SwitchableColor[] colors)
    {
        if (colors == null || colors.Length == 0) return;

        for (int i = 0; i < info.mesh.Length; ++i)
        {
            if (info.mesh[i].mesh != null)
            {
                foreach (SwitchableColor color in colors)
                info.mesh[i].mesh.GetComponent<SpriteRenderer>().color = color.color;
            }
        }
    }

    // -----------------------------------------------------------------------------------
}

// =======================================================================================