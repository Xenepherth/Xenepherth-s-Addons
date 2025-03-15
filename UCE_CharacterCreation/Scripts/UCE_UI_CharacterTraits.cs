// =======================================================================================
// Created and maintained by Fhiz
// Usable for both personal and commercial projects, but no sharing or re-sale
// * Discord Support Server.............: https://discord.gg/YkMbDHs
// * Public downloads website...........: https://www.indie-mmo.net
// * Pledge on Patreon for VIP AddOns...: https://www.patreon.com/IndieMMO
// =======================================================================================
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public partial class UCE_UI_CharacterTraits : MonoBehaviour
{
#if _iMMOTRAITS

    public UCE_UI_CharacterTraits singleton;

    public UCE_UI_CharacterCreation parentPanel;
    public Text traitPointsText;
    public UCE_UI_TraitSlot slotPrefab;
    public Transform content;

    protected List<UCE_TraitTemplate> traitPool;
    protected int traitPoints;
    protected int classIndex;
    [HideInInspector] public List<UCE_TraitTemplate> currentTraits;

    // -----------------------------------------------------------------------------------
    // Awake
    // -----------------------------------------------------------------------------------
    public void Awake()
    {
        if (singleton == null) singleton = this;
    }

    // -----------------------------------------------------------------------------------
    // Show
    // -----------------------------------------------------------------------------------
    public void Show()
    {
        Player player = parentPanel.currentPlayer;
        if (!player) return;

        currentTraits.Clear();
        currentTraits = new List<UCE_TraitTemplate>();
        traitPool = new List<UCE_TraitTemplate>();
        traitPool.AddRange(UCE_TraitTemplate.dict.Values.ToList());

        traitPoints = player.UCE_TraitPoints;

        traitPointsText.text = traitPoints.ToString() + "/" + player.UCE_TraitPoints.ToString();

        gameObject.SetActive(true);
    }

    // -----------------------------------------------------------------------------------
    // Update
    // -----------------------------------------------------------------------------------
    private void Update()
    {
        if (gameObject.activeSelf && parentPanel.gameObject.activeSelf)
        {
            Player player = parentPanel.currentPlayer;
            if (!player) return;

            // -----

            traitPointsText.text = traitPoints.ToString() + "/" + player.UCE_TraitPoints.ToString();

            // ----- update trait list

            UIUtils.BalancePrefabs(slotPrefab.gameObject, traitPool.Count, content);

            for (int i = 0; i < traitPool.Count; ++i)
            {
                int icopy = i;

                UCE_UI_TraitSlot slot = content.GetChild(icopy).GetComponent<UCE_UI_TraitSlot>();
                UCE_TraitTemplate tmpl = traitPool[icopy];

                bool bAllowed = true;

                // -- can afford
                if (traitPoints < tmpl.traitCost) bAllowed = false;

                // -- not unique, not taken?
                if (currentTraits.Any(x => x.uniqueGroup == tmpl.uniqueGroup)) bAllowed = false;

                // -- not limited by class?
                if (tmpl.allowedClasses.Length > 0 && !tmpl.allowedClasses.Any(x => x == player.gameObject)) bAllowed = false;

                // -- enable/disable button add/remove trait
                if (currentTraits.Any(x => x == tmpl))
                    slot.button.interactable = true;
                else
                    slot.button.interactable = bAllowed;

                // -- trait exists already?
                if (currentTraits.Any(x => x == tmpl))
                    slot.image.color = Color.black;
                else
                    slot.image.color = Color.white;

                // -- set/unset trait per click
                slot.button.onClick.SetListener(() =>
                {
                    if (currentTraits.Any(x => x == tmpl))
                    {
                        currentTraits.Remove(tmpl);
                        traitPoints += tmpl.traitCost;
                    }
                    else
                    {
                        currentTraits.Add(tmpl);
                        traitPoints -= tmpl.traitCost;
                    }
                });

                slot.image.sprite = tmpl.image;
                slot.tooltip.enabled = true;
                slot.tooltip.text = tmpl.ToolTip();
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    // -----------------------------------------------------------------------------------
    // Hide
    // -----------------------------------------------------------------------------------
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    // -----------------------------------------------------------------------------------
#endif
}