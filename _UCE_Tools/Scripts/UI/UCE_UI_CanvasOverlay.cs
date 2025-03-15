﻿// =======================================================================================
// Created and maintained by iMMO
// Usable for both personal and commercial projects, but no sharing or re-sale
// * Discord Support Server.............: https://discord.gg/YkMbDHs
// * Public downloads website...........: https://www.indie-mmo.net
// * Pledge on Patreon for VIP AddOns...: https://www.patreon.com/iMMOban
// =======================================================================================
using UnityEngine;

// =======================================================================================
// CANVAS OVERLAY - UI
// =======================================================================================
public class UCE_UI_CanvasOverlay : MonoBehaviour
{
    // -----------------------------------------------------------------------------------
    // Awake
    // @Client
    // -----------------------------------------------------------------------------------
    private void Awake()
    {
        LeanTween.init();
        LeanTween.alpha(this.gameObject.GetComponent<RectTransform>(), 0f, 0f);
    }

    // -----------------------------------------------------------------------------------
    // FadeOut
    // @Client
    // -----------------------------------------------------------------------------------
    public void FadeOut(float fDuration = 0f)
    {
        LeanTween.alpha(this.gameObject.GetComponent<RectTransform>(), 1f, fDuration);
    }

    // -----------------------------------------------------------------------------------
    // AutoFadeOut
    // @Client
    // -----------------------------------------------------------------------------------
    public void AutoFadeOut(float fDuration = 0f)
    {
        LeanTween.alpha(this.gameObject.GetComponent<RectTransform>(), 1f, fDuration);
        Invoke("FadeIn", 0.5f);
    }

    // -----------------------------------------------------------------------------------
    // FadeIn
    // @Client
    // -----------------------------------------------------------------------------------
    public void FadeIn(float fDuration = 0.5f)
    {
        LeanTween.alpha(this.gameObject.GetComponent<RectTransform>(), 0f, fDuration);
    }

    // -----------------------------------------------------------------------------------
    // FadeIn
    // Same method without parameters for Invoke
    // @Client
    // -----------------------------------------------------------------------------------
    public void FadeIn()
    {
        LeanTween.alpha(this.gameObject.GetComponent<RectTransform>(), 0f, 0.5f);
    }

    // -----------------------------------------------------------------------------------
    // FadeInDelayed
    // @Client
    // -----------------------------------------------------------------------------------
    public void FadeInDelayed(float fDelay = 0f)
    {
        Invoke("FadeIn", fDelay);
    }

    // -----------------------------------------------------------------------------------
}

// =======================================================================================