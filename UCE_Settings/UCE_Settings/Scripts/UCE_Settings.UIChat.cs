using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Grabs our dagger settings variables for chat to know if its visible or not.
public partial class UIChat : MonoBehaviour
{
    private UCE_UI_SettingsVariables settingsVariables;

    // Grabs our settings variables.
    private void Start()
    {
        settingsVariables = FindObjectOfType<UCE_UI_SettingsVariables>().GetComponent<UCE_UI_SettingsVariables>();
    }
}