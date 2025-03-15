using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Sets our new hotkeys for quests.
public partial class UIQuests : MonoBehaviour
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
            if (settingsVariables.keybindUpdate[20])
            {
                hotKey = settingsVariables.keybindings[20];
                settingsVariables.keybindUpdate[20] = false;
            }
    }
}