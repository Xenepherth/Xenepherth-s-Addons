// =======================================================================================
// Created and maintained by iMMO
// Usable for both personal and commercial projects, but no sharing or re-sale
// * Discord Support Server.............: https://discord.gg/YkMbDHs
// * Public downloads website...........: https://www.indie-mmo.net
// * Pledge on Patreon for VIP AddOns...: https://www.patreon.com/iMMOban
// =======================================================================================
using Mirror;
using UnityEngine;

// =======================================================================================
// UCE INTERACTABLE AREA (BOX) CLASS
// =======================================================================================
[RequireComponent(typeof(Collider2D))]
public partial class UCE_InteractableAreaBox : UCE_Interactable
{
    [Header("[EDITOR]")]
    public Color gizmoColor = new Color(0, 1, 1, 0.25f);
    public Color gizmoWireColor = new Color(1, 1, 1, 0.8f);

    // -----------------------------------------------------------------------------------
    // OnDrawGizmos
    // @Editor
    // -----------------------------------------------------------------------------------
    private void OnDrawGizmos()
    {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();

        // we need to set the gizmo matrix for proper scale & rotation
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
        Gizmos.color = gizmoColor;
        Gizmos.DrawCube(collider.offset, collider.size);
        Gizmos.color = gizmoWireColor;
        Gizmos.DrawWireCube(collider.offset, collider.size);
        Gizmos.matrix = Matrix4x4.identity;
    }

    // -----------------------------------------------------------------------------------
    // OnTriggerEnter
    // @Client
    // -----------------------------------------------------------------------------------
    [ClientCallback]
    private void OnTriggerEnter2D(Collider2D co)
    {
        Player player = co.GetComponentInParent<Player>();

        if (player != null && player == Player.localPlayer)
        {
            if ((!interactionRequirements.hasRequirements() && !interactionRequirements.hasCosts()) || automaticActivation)
            {
                ConfirmAccess();
            }
            else
            {
                ShowAccessRequirementsUI();
            }
        }
    }

    // -----------------------------------------------------------------------------------
    // OnTriggerExit
    // @Client
    // -----------------------------------------------------------------------------------
    [ClientCallback]
    private void OnTriggerExit2D(Collider2D co)
    {
        Player player = co.GetComponentInParent<Player>();

        if (player != null && player == Player.localPlayer)
            HideAccessRequirementsUI();
    }

    // -----------------------------------------------------------------------------------
    // Update
    // @Client
    // -----------------------------------------------------------------------------------
    [ClientCallback]
    private void Update()
    {
        Player player = Player.localPlayer;
        if (!player) return;

        // -- check for interaction Distance
        if (IsWorthUpdating())
            this.GetComponentInChildren<SpriteRenderer>().enabled = UCE_Tools.UCE_CheckSelectionHandling(this.gameObject);
    }

    // -----------------------------------------------------------------------------------
}