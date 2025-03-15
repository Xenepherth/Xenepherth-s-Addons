using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Sets our new hotkeys for equipment.
public partial class UIEquipment : MonoBehaviour
{
    private UCE_UI_SettingsVariables settingsVariables;

    // Grabs our settings variables.
    private void Start()
    {
        settingsVariables = FindObjectOfType<UCE_UI_SettingsVariables>().GetComponent<UCE_UI_SettingsVariables>();
    }

    // Set our hotkey based on the players selection.
    private void FixedUpdate()
    {
        if (settingsVariables != null)
            if (settingsVariables.keybindUpdate[5])
            {
                hotKey = settingsVariables.keybindings[5];
                settingsVariables.keybindUpdate[5] = false;
            }
    }
}